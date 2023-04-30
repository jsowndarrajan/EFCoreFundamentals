[![.NET](https://github.com/jsowndarrajan/EFCoreFundamentals/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jsowndarrajan/EFCoreFundamentals/actions/workflows/dotnet.yml)
# Entity Framework Core Fundamentals
This repo contains my practice code of [this](https://app.pluralsight.com/library/courses/ef-core-6-fundamentals/table-of-contents) EFCore pluralsight course

## Notes
When a query contains hardcoded value then it will produce a non-parameterized SQL query
```
var authors = context.Authors.Where(author => author.FirstName = "Sowndarrajan").ToList();
```

*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
WHERE [a].[FirstName] = N'Sowndarrajan'
```

However, if the query contains the variable name, it will produce a parameterized SQL query
```
var firstName = "Sowndarrajan";
var authors = context.Authors.Where(author => author.FirstName = firstName).ToList();
```
*SQL Query*
```
exec sp_executesql N'SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
WHERE [a].[LastName] = @__firstName_0',N'@__firstName_0 nvarchar(4000)',@__firstName_0=N'Sowndarrajan'
```