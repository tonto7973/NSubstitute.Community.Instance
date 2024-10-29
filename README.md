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

Let's say you have a class with a constructor defined as follows:

```csharp
public class DocumentRepository
{
    public DocumentRepository(IRestClient restClient, IJwtTokenProvider tokenProvider,
        IDocumentRepositoryOptions options, ILogger logger) { ... }

    public TDocument GetDocument<TDocument>(int id) { ... }
}
```

Your typical unit test might look like this:

```csharp
[Test]
public void GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed()
{
    var restClient = Substitute.For<IRestClient>();
    var tokenProvider = Substitute.For<IJwtTokenProvider>();
    var options = Substitute.For<IDocumentRepositoryOptions>();
    var logger  = Substitute.For<ILogger>();
    var documentRepository = new DocumentRepository(restClient, tokenProvider, options, logger);

    var document = documentRepository.GetDocument<CarDetails>(32);

    Assert.IsNotNull(document);
}
```

With `Instance.Of` the arrange part of the unit test can be simplified to:

```csharp
[Test]
public void GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed()
{
    var documentRepository = Instance.Of<DocumentRepository>();

    var document = documentRepository.GetDocument<CarDetails>(32);

    Assert.IsNotNull(document);
}
```

### B) Changing constructor signature does not break unit tests

Now you want to add more features to your `DocumentRepository` and decide to add some validation. Thus you change the constructor signature to:

```csharp
public DocumentRepository(IRestClient restClient, IJwtTokenProvider tokenProvider,
    IDocumentRepositoryOptions options, IDocumentValidator validator, ILogger logger)
```

This could instantly break a lot of your unit tests code as it would not compile and you would need to spend a lot of time to fix all the mess. E.g. in the original `GetDocument_ReturnsValidDocument_WhenDefaultOptionsUsed` unit test, you would need to add another line with another substitute to make the test compile. Then you would need to change the line where the instance is created.

Not so with `Instance.Of`. It does all the substitution automatically for you and you do not need to add another line to your code.

The same applies when removing or reordering dependencies.

## Features

### Supports constructor overloading

Multiple constructors of a class are supported. The parameter matching logic will try to choose the most appropriate constructor.

### Constructor parameters

You can pass optional arguments to `Instance.Of`. These optional arguments will be used instead of substitutions. All other required constructor arguments will be substituted.

```csharp
// Consider this constructor signature:
public DocumentRepository(IRestClient restClient, ILogger logger)
...
// Calling
var repository = Instance.Of<DocumentRepository>(restClient);
// is equivalent to
new DocumentRepository(restClient, Substitute.For<ILogger>());
```

### Constructor parameters with default values

Constructor parameters with default values are not automatically substituted.

```csharp
// Consider this constructor signature:
public DocumentRepository(IRestClient restClient, ILogger logger = null)
...
// Calling
Instance.Of<DocumentRepository>();
// is equivalent to
new DocumentRepository(Substitute.For<IRestClient>());
```

### Null values in constructor parameters

This is currently supported by using `Instance.Null<T>()`. Pasing `null` will throw an exception.

```csharp
// Consider this constructor signature:
public DocumentRepository(IRestClient restClient, ILogger logger)
// Calling 
Instance.Of<DocumentRepository>(Instance.Null<IRestClient>());
// is equivalent to
new DocumentRepository(null, Substitute.For<ILogger>());
```

This is particularly useful in scenarios where constructor arguments need to be sanitised (checked for nulls) and the sanitising logic need to be tested:

```csharp
// Actual contructor
public DocumentRepository(IRestClient restClient, ILogger logger)
{
    _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
}
// And a sample unit test
[Test]
public void DocumentRepositoryConstructor_Throws_WhenLoggerNull()
{
    Action action = () => Instance.Of<DocumentRepository>(Instance.Null<ILogger>());    
    Assert.Throws<ArgumentNullException>(action);
}
```

### Supports non-public constructors

This is supported as long as the constructor is accessible to NSubstitute.

```csharp
public class MyService
{
    protected MyService(IDependency dependency) { ... }
}
// Calling
Instance.Of<MyService>();
// does not fail, it will call the protected constructor
```

### Supports abstract classes

This is supported as long as any of abstract class constructor is accessible to NSubstitute. The actual class is proxied through `Substitute.ForPartsOf`.

```csharp
public abstract class MyBaseClass
{
    protected MyBaseClass(IDependency dependency) { ... }
}
// Calling
Instance.Of<MyBaseClass>();
// does not fail, it will call the protected constructor with IDependency substituted
```

### Interfaces are not supported

`Instance.Of<TType>` supports only classes. Use `Substitute.For<TType>` for interfaces.

### Recursive constructor pattern is not supported

Creating an instance using a constructor with a parameter that takes the declaring type is not supported. Consider the following class:

```csharp
public class Node
{
    public Node(Node parent, int depth) { ... }
}
// Calling
Instance.Of<Node>();
// would fail with `NotSupportedException`
```

This is due to the fact that `Instance.Of` creates substitutes for constructor parameters automatically. In this case the constructor parameter references itself and thus instantiation would cause stack overflow. To bypass this limitation, you need to pass the dependency itself or its null representation:

```csharp
var parent = Instance.Null<Node>(); // or parent = new Node(null, 0);
var node = Instance.Of<Node>(parent);
```

## Suported platforms

* [.NET 8.0](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
