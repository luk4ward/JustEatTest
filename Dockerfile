FROM microsoft/dotnet:2.0.3-sdk AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers
COPY ./src/JustEatTest.Api/JustEatTest.Api.csproj ./JustEatTest.Api/
COPY ./src/JustEatTest.Domain/JustEatTest.Domain.csproj ./JustEatTest.Domain/
RUN dotnet restore JustEatTest.Api/JustEatTest.Api.csproj

# copy everything else and build
COPY ./src/ .
WORKDIR /JustEatTest.Api/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM microsoft/dotnet:2.0.3-runtime 
WORKDIR /app
COPY --from=build /JustEatTest.Api/out ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "JustEatTest.Api.dll"]