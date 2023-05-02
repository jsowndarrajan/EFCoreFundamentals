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
It provides CLR methods that get directly translated into database functions when used in LINQ to Entities queries. For instance, Like() method is transalated into SQL *LIKE* operations
```
var authors = context.Authors.Where(author => EF.Functions.Like(author.FirstName, "S%"));
```

*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
WHERE [a].[FirstName] LIKE N'S%'
```

### Find
It will help to find an entity based on the primary key value. If the entity value exists in the DbContext, it will return the value directly from the context. Otherwise, it will make a call to database, and will fetch the record.
```
var authorId = 1;
var author = context.Authors.Find(authorId);
```

Find() method will use the same query execution plan for all the primary key values, and that's why EF Core converts the method into `sp_executesql` statement

*SQL Query*
```
exec sp_executesql N'SELECT TOP(1) [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
WHERE [a].[Id] = @__p_0',N'@__p_0 int',@__p_0=1
```

### Pagination
Skip() and Take() methods is used to fetch records based on the given pageSize and offset value.
```
var offset = 0;
var pageSize = 10;
var authors = context.Authors.Skip(pageSize * offset).Take(pageSize).ToList();
```

*SQL Query*
```
exec sp_executesql N'SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
ORDER BY (SELECT 1)
OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY',N'@__p_0 int,@__p_1 int',@__p_0=0,@__p_1=10
```

### Ordering
OrderBy() method is used to order the records in ascending order
```
var authors =  context.Authors.OrderBy(author => author.FirstName).ToList();
```
*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
ORDER BY [a].[FirstName]
```
OrderByDescending() method is used to order the records in desending order
```
var authors =  context.Authors.OrderByDescending(author => author.FirstName).ToList();
```
*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
ORDER BY [a].[FirstName] DESC
```
ThenBy() method can be used to further order the previously ordered records based on another field in ascending order
```
var authors = context.Authors
                     .OrderBy(author => author.FirstName)
                     .ThenBy(author => author.LastName)
                     .ToList();
```
*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
ORDER BY [a].[FirstName], [a].[LastName]
```
ThenByDescending() method can be used to further order the previously ordered records based on another field in desecnding order
```
var authors = context.Authors
                     .OrderByDescending(author => author.FirstName)
                     .ThenByDescending(author => author.LastName)
                     .ToList();
```
*SQL Query*
```
SELECT [a].[Id], [a].[FirstName], [a].[LastName]
FROM [Authors] AS [a]
ORDER BY [a].[FirstName] DESC, [a].[LastName] DESC
```
