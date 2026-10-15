
# Assumptions

Not all post have Uri, e.g. https://hacker-news.firebaseio.com/v0/item/50059096.json
Where there is no Uri, I will normalise it to an empty string

I have set the cache length to 1 hour for both HackerNews endpoints, that may be too long, it depends on how we intend to consume the data, but it is a good starting point.

We are limiting to 500 stories, to match the underlying API

We are keeping the error handling simple, if the underlying API fails, we will return a 500 error to the client.

# Run Instructions

1. Open `SantanderTest.slnx` in Visual Studio 2026.
2. Build the solution (Ctrl+Shift+B).
3. Run the api project (F5) and it should start local.
4. If you just want to check tests, open Test Explorer and run all tests.

Or from terminal:

```powershell
dotnet restore
dotnet build
dotnet test
dotnet run
```

# Testing the Endpoint

Use the SantanderTest.Test/RestClient/Best.http file to run the endpoint in your IDE, or the scalar web client at http://localhost:5133/scalar/v1

# Enhancements

- It would benefit from increased test coverage.
- It should really have CORS configured for security.
- Health Checks need adding for monitoring in prod, along with logging
- Retry logic on accessing the HN API, currently if one story fails it will blow.
- Limiting the concurrency on the Story fetch, it works at 500 stories, but it's not very graceful or considerate.
