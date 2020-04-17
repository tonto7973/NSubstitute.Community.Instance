# NSubstitute.Community.Instance

This little addon to [NSubstitute](https://github.com/nsubstitute/NSubstitute) allows you to create instances of classes used in unit testing with all relevant dependencies automatically substituted by `Substitute.For()`.

## Quick Start

```csharp
var service = Instance.Of<YourService>();
```

## Motivation

Ever had to update dozens of unit tests when you added another dependency to your class? Well, this little addon to NSubstitute can help you save some time. Plus, it can help you reduce the amount of boilerplate code required to arrange your unit tests and simplify them.

Consider the following scenarios:

### A) Reducing boilerplate code

Lets say you have a class defined as:

```csharp
public class DocumentRepository(IRestClient restClient, IJwtTokenProvider tokenProvider, IDocumentRepositoryOptions options, ILogger<DocumentRepositoryOptions> logger)
```

Your typical unit test code might look like this:

```csharp
[Test]
public void GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed()
{
    var restClient = Substitute.For<IRestClient>();
    var tokenProvider = Substitute.For<IJwtTokenProvider>();
    var options = Substitute.For<IDocumentRepositoryOptions>();
    var logger  = Substitute.For<ILogger<DocumentRepositoryOptions>>();
    var documentRepository = new DocumentRepository(restClient, tokenProvider, options, logger);

    var document = documentRepository.GetDocument<CarDetails>(32);

    document.ShouldNotBeNull();
}
```

With `Instance.Of` the arrange part of the unit test can be simplified to:

```csharp
[Test]
public void GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed()
{
    var documentRepository = Instance.Of<DocumentRepository>();

    var document = documentRepository.GetDocument<CarDetails>(32);

    document.ShouldNotBeNull();
}
```

### B) Changing constructor signature does not break unit tests

Lets say you want to add more features to your `DocumentRepository` and decide to add some validation. Thus you change the constructor signature to:

```csharp
public class DocumentRepository(IRestClient restClient, IJwtTokenProvider tokenProvider, IDocumentRepositoryOptions options, IDocumentValidator validator, ILogger<DocumentRepositoryOptions> logger)
```

This could instantly break a lot of your unit tests code as it would not compile and you would need to spend a lot of time to fix all the mess. E.g. in the original `GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed` unit test, you would need to add another line with another substitute to make the test compile. THen you would need to change the line where the instance is created.

Not so with `Instance.Of`. It does all the substitution automatically for you and you do not need to add another line to your code.

The same applies when removing or reordering dependencies.

## Features and limitations

### Supports constructor overloading

Multiple constructors are supported. The parameter matching logic will try to choose the most appropriate constructor.

### Constructor parameters

You can pass optional arguments to `Instance.Of`. These optional arguments will be used instead of substitutions. All other required constructor arguments will be substituted.

```csharp
var restClient = Substitute.For<IRestClient>();
var repository = Instance.Of<DocumentRepository>(restClient);
```

### Constructor parameters with default values

Constructor parameters with default values are not automatically substituted.

```csharp
// Consider this constructor signature:
public class DocumentRepository(IRestClient restClient, ILogger<DocumentRepositoryOptions> logger = null)
...
// Calling
Instance.Of<DocumentRepository>();
// is equivalent to
new DocumentRepository(Substitute.For<IRestClient>());
```

### Supports non-public constructors

Non-public constructors are supported. Yes, this somewhat violates the encapsulation principle, but ... shhh! Don't tell anyone.

```csharp
public class MyService
{
    protected MyService(IDependency dependency) { ... }
}
// Calling
Instance.Of<MyService>();
// does not fail, it will call the protected constructor
```

### Abstract classes and interfaces are not supported

`Instace.Of<TType>` supports only concrete classes. Use `Substitute.For<TType>` for interfaces and abstract classes.

## Suported platforms

* [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) - _See the [implementation support](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)._