# Updating from `@typespec/compiler@^0.65.0` -> `1.2.0`. Changes to `@service` decorator.

## question 
Hello, looking to migrate versions of the TSP compiler, I ran into a problem with the `@service` decorator. With the older version (0.65.0) we were able to attached more information to the `@service` by defining it like this:
```
@service({
  title: "Azure OpenAI Service",
  termsOfService: "https://openai.com/policies/terms-of-use",
  contact: {
    name: "OpenAI Support",
    url: "https://help.openai.com",
  },
  license: {
    name: "MIT",
    url: "https://github.com/openai/openai-openapi/blob/master/LICENSE",
  },
})
```
But after the update, the only admissible field seems to be `title` . I tried looking at the documentation, but I was not able to find a replacement for this. Currently, in order to compile, I had to change the above code to:
```
@service(#{
  title: "Azure OpenAI Service",
  // termsOfService: "https://openai.com/policies/terms-of-use",
  // contact: #{
  //   name: "OpenAI Support",
  //   url: "https://help.openai.com",
  // },
  // license: #{
  //   name: "MIT",
  //   url: "https://github.com/openai/openai-openapi/blob/master/LICENSE",
  // },
})
```
Thank you for any help you can provide. Apologies if I missed something immensely obvious that I should be doing. 

## answer
In TypeSpec 1.0.0, the `@service` decorator has been simplified to only accept the `title` property. According to the release notes, the `version` property and other metadata options were removed in 1.0.0-rc.0 where they were previously deprecated.

To include additional service metadata like `termsOfService`, `contact`, and `license` information in your OpenAPI document, you should use the `@info` decorator from the `@typespec/openapi` library. Here's how to convert your example:

```typespec
import "@typespec/openapi";

@service({
  title: "Azure OpenAI Service"
})
@TypeSpec.OpenAPI.info({
  termsOfService: "https://openai.com/policies/terms-of-use",
  contact: {
    name: "OpenAI Support",
    url: "https://help.openai.com",
  },
  license: {
    name: "MIT",
    url: "https://github.com/openai/openai-openapi/blob/master/LICENSE",
  },
})
namespace YourNamespace {
  // Your API definitions
}
```

# Examples generation

## question 
Hello, 
I wanted to know if it was possible to provide a sample that would be used in example generation when running the `oav` command using the `@example` decorator [here](https://typespec.io/docs/language-basics/documentation/#:~:text=the%20resource%20type-,%40example,-(unofficial)) or if the @example decorator is supported, thanks!

Something like:
```

@example({
  name: "exampleScheduledAction",
  summary: "Example of a scheduled action resource",
  value: {
    id: "/subscriptions/0000/resourceGroups/my-rg/providers/Microsoft.ScheduledActions/scheduledAction1",
    name: "scheduledAction1",
    type: "Microsoft.ScheduledActions/scheduledActions",
    location: "eastus",
    tags: {
      environment: "test"
    },
    properties: {
      schedule: "0 0 * * *",
      actionType: "Backup",
      enabled: true
    }
  }
})
```

## answer
You should not use the example decorator in Azure specs.  Instead, the typespec-autorest emitter automatically matches and inserts examples by operationId, see: [x-ms-examples example files | TypeSpec Azure](https://azure.github.io/typespec-azure/docs/migrate-swagger/faq/x-ms-examples/)
Regarding how to control the values that will be generated in the examples. This is really an oav question, so notreally an expert, but I know there is a mechanism for con figuring examples, but you can find some details here: [Azure/oav: Tools for validating OpenAPI (Swagger) files](https://github.com/Azure/oav?tab=readme-ov-file#what-does-the-tool-do-what-issues-does-the-tool-catch).
 
You can also update values after the examples are generated.  There is also the api scenario mechanism which provides a bit more configurable example generation as part of servic eapi testing: [azure-rest-api-specs/documentation/api-scenario at main · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/tree/main/documentation/api-scenario)

# Does typespec allow negative lookaheads in name validation?

## question 
 Hi Team, 
Does typespec allow negative lookahead for validating names? If not can you share the limitations? 
Here is the pattern we are trying to implement:
- Length - 4 - 64
- Alphanumerics and hyphens
- Can't end with period. Start with a letter
- Can't start or end with hyphen. Can't us consecutive hyphens
- Can have spaces but not at the start or end of a name.
```
model StorageDiscoveryScope {
  @doc("Display name of the collection")
  @pattern("^(?! )[a-zA-Z0-9]+(?:[ -][a-zA-Z0-9]+)*(?<! )$")
  @minLength(4)
  @maxLength(64)
  displayName: string;
```

## answer
No, TypeSpec doesn't support advanced regex features like negative lookaheads and lookbehinds.

The `@pattern` decorator only supports basic regex syntax similar to OpenAPI:

- Alternations (`|`)
- Basic quantifiers (`?`, `*`, `+`, `{ }`)
- Wildcard (`.`)
- Grouping parentheses

For your requirements:

1. Use `@minLength(4)` and `@maxLength(64)` for length constraints
2. Use a simplified regex pattern
3. Document all constraints in the `@doc` decorator
4. Implement custom validation in your service code

Example:
```typescript
model StorageDiscoveryScope {
  @doc("Display name. Must be 4-64 characters, alphanumerics/hyphens/spaces only. Must start with letter, can't start/end with space or hyphen, no consecutive hyphens.")
  @pattern("^[a-zA-Z][a-zA-Z0-9 -]*[a-zA-Z0-9]$")
  @minLength(4)
  @maxLength(64)
  displayName: string;
}
```

Note: `@pattern` is primarily for documentation; SDK won't validate at runtime.

# Support for @includeInapplicableMetadataInPayload decorator

## question 
After pulling the latest changes, I been getting errors regarding "@includeInapplicableMetadataInPayload(false)" decorator not being supported anymore.
 
Is it not possible to make use of the decorator? If I removed the decorator from the model ts file, it changes the expected model definition for the API path on swagger.

## answer
The issue is related to the deprecation of the @includeInapplicableMetadataInPayload(false) decorator in TypeSpec version 0.67. It has been moved to a private namespace, and you can still use it, but with a warning.
Decorator Usage: You can still use the @includeInapplicableMetadataInPayload decorator, but it will trigger a warning since it's now in a private namespace.

Breaking Change: Removing this decorator causes a breaking change for existing API versions because the Swagger model definition is altered.

Suggestion: The team suggests not relying on the decorator and using existing resource models like TrackedResource<T> instead.

# Visibility Choice

## question 
Hi team,
 
Could someone please help me explain the difference between @visibility(LifeCycle.Read) and  @visibility(Lifecycle.Create, LifeCycle.Read) 

## answer
The difference between @visibility(Lifecycle.Read) and @visibility(Lifecycle.Create, Lifecycle.Read) in TypeSpec is in which phases of the resource lifecycle the property is included:
@visibility(Lifecycle.Read) means the property is only visible when reading the resource (e.g., in HTTP GET responses). It will not appear in requests to create or update the resource. This is typically used for properties that are calculated or set by the service, such as IDs or timestamps.
@visibility(Lifecycle.Create, Lifecycle.Read) means the property is visible both when creating and reading the resource (e.g., in HTTP POST/PUT requests and GET responses). This is used for properties that the client can set during creation and that are also returned by the service when reading the resource, but cannot be updated later.
