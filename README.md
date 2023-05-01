[![.NET](https://github.com/jsowndarrajan/EFCoreFundamentals/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jsowndarrajan/EFCoreFundamentals/actions/workflows/dotnet.yml)
# Entity Framework Core Fundamentals
This repo contains my practice code of [this](https://app.pluralsight.com/library/courses/ef-core-6-fundamentals/table-of-contents) EFCore pluralsight course

## Notes

### Filter
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
WHERE [a].[FirstName] = @__firstName_0',N'@__firstName_0 nvarchar(4000)',@__firstName_0=N'Sowndarrajan'
```

### Enumeration

*Good Enumeration*: Less impact on the database as the connection will be closed immediately after printing author's firstName
```
foreach(var author in context.Authors)
{
	Console.WriteLine(author.FirstName);
}
```

*Bad Enumeration*: Connection remains open till retrieving the last record from the database
```
foreach(var author in context.Authors)
{
	ValidateFirstName(author.FirstName);
	ValidateLastName(author.FirstName);
	CheckAuthorExistsInOtherSystem(author);
}
```

*Execution*: Smarter way to fetch results as the connection will be closed immediately after retrieving all the required results from database
```
var authors = context.Authors.ToList();
foreach(var author in authors)
{
	ValidateFirstName(author.FirstName);
	ValidateLastName(author.FirstName);
	CheckAuthorExistsInOtherSystem(author);
}
```

### EF Functions
It provides CLR methods that get directly translated into database functions when used in LINQ to Entities queries.

For instance, Like() method is transalated into SQL *LIKE* operations
```
var authors = context.Authors.Where(author => EF.Functions.Like(author.FirstName, "S%"));
```

*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
WHERE [a].[FirstName] LIKE N'S%'
```