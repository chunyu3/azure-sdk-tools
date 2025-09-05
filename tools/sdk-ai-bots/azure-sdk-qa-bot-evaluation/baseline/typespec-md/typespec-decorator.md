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
All the other options where never available and just silently dropped. If you meant to have this in the openapi document you should use @info from openapi library

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
You should not use the example decorator in Azure specs.  Instead, the typespec-autorest emitter automatically matches and inserts examples by operationId, see: x-ms-examples example files | TypeSpec Azure
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
no, only simple syntax same as openapi. Documented on the `@pattern` decorator https://typespec.io/docs/standard-library/built-in-decorators/#@pattern
note that `@pattern` is also just documentation purpose from the point of view of SDK(it won't validate regardless), you are still free to add extra validation on your side and document those restriction in the property doc

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
If the visibility is read, the client cannot provide the value (it is generated by the service). For create + read, the client can provide the value, but only when the resource is created. Once set, the client cannot change it in subsequent update methods.
