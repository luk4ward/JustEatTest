# Just Eat Engineer Recruitment Test
## Technical questions
1. 
I spent something around 3-3,5h - it was when I finished reading through the task and started coding. At the beginning I thought that I have a lot of time but after facing few problems with `VS Code` and `dotnet` I realized I don't have that much time.

I was able to add some logging (using Serilog, have my own template for API already so used that) and also docerized the app so it's really easy to run now.
If I had more time I would add some extra tests for sure - right now I have only unit tests included but some integration tests are still needed. Also in `RestaurantSerivce` I can see some parts can be extracted to other more generic class that will handle building proper headers and calling APIs.

2.
I think it will be `HttpClientFactory` added in _ASP.NET Core 2.1_ that solves some really popular problems when using `HttpClient`.
You have three different ways to use it and one of them is called _Named Clients_
```csharp
services.AddHttpClient();
services.AddHttpClient("just-eat", c =>
{
    c.BaseAddress = new Uri("https://public.je-apis.com/");
    c.DefaultRequestHeaders.Add("Accept-Tenant", "uk");
    c.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
    c.DefaultRequestHeaders.Add("Host", "public.je-apis.com");
});
```
So with this you can define clients with some pre-configured settings (would be useful in this test!)

3.
Performance is really important in FinTech so in my current job we need to do it quite often. Right now in our stack we have both _Serilog_ and _Kibana_ with _ElasticSearch_. Kibana with it's boards is really useful to notice some problem with performance and it's quick to provide some more details from past months if needed. Also some time ago we introduced _Stackify Retrace_ where we can easly trace some performance problems as well.
Some problems are also not code related and since we host everything on AWS we can use built-it tools as well but that is more `DevOps` work and we are trying to separate responsibilities when company is growing so fast.

4.
A really nice thing to do would be support for `GraphQL`. Now endpoint returns list of quite complicated objects but not all properties are probably required by user. With a little touch of `GraphQL` magic responses would be really slim, without unnecessary data.

I tried sending many requests in short period of time and was always getting a response - didn't want to send too many requests though. I assume there is no throttling on server so can be easly `DDoS`ed so worth adding something to increase security (if I'm wrong and there is already something just skip about this part). 

Also there is no version of the API included in the response header which in my opinion is really useful when introduction new features.

5.
```json
{
   "Name":"Maciej",
   "Surname":"Baczynski",
   "Email":"luk4ward@gmail.com",
   "BirthDate":"14-12-1990",
   "Sex":"male",
   "Eyes":"blue",
   "Languages":[
      "Polish",
      "English",
      "C#",
      "Go",
      "SQL"
   ],
   "Dependants":[
      {
         "Name":"Marcelina",
         "Surname":"Dobra",
         "Relationship":"wife"
      },
      {
         "Name":"Falka",
         "Surname":"The Whippet",
         "Relationship":"pet"
      }
   ],
   "Education":[
      {
         "University":"Poznan University of Technology",
         "Level":"Master",
         "GraduatedOn":"05-07-2015"
      },
      {
         "University":"Poznan University of Technology",
         "Level":"Bachelor",
         "GraduatedOn":"13-02-2014"
      }
   ],
   "Music":[
      "Rock",
      "Metal"
   ],
   "Interests":[
      "Coding",
      "Cars",
      "Travels"
   ]
}
```
