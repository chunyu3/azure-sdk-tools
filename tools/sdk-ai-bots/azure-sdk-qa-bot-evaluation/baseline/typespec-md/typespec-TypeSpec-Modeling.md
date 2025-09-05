# Support for pagination

## question 
Hi, I have a typespec API which is a list api 
```
 listResources is ArmResourceActionSync<
    AutoAction,
    void,
    AutoActionResourceListResponse
  >;

model AutoActionResourceListResponse is Azure.Core.Page<AutoActionResource>;
```
This api needs to support pagination. How can I achieve that using Microsoft.TypeSpec.Providerhub.Controller? we use the package to generate our controllers, which doesn't let me add query parameters to our API

## answer
The controller emitter will emit an endpoint for your action, but you will need to add an endpoint manually for the next page endpoint (which is, presumably, a GET).
get is required for paging.  If you are not using GET, then automated paging mechanisms in ARM clients won't work.
It's possible to use a different paging mechanism, but will require custom code in SDKs
typespec-providerhub does not provide extra GET endpoints for resource actions that return lists - you would need to add this endpoint.  Since the controllers are partial, this should be fairly straightforward.
The question of whether RPaaS (providerhub) should support such an endpoint and typespec-providerhub should provide any required extension support for this is a good one.

# Define Proxy object with 200/202 Http response

## question 
How do I define a Put API for ARM proxy object which should return 200 or 202 success response .  Example : ProxyResource

## answer
You shouldn't.  PUT operations should return 200 and 201, per the RPC.  There are some templates that allow overriding the default responses, but this would only be appropriate for older APIs.  Newer APIs should not define such operations.

# How to suppress 200 response code for a LongRunningOperation?

## question 
Hello TypeSpec Discussion team,

I'm from Compute team and I created a new PR [Add 2025-07-01-preview API version to Azure Fleet · Azure/azure-rest-api-specs@5fccb58](https://github.com/Azure/azure-rest-api-specs/actions/runs/16634168616) to add a cancel API to our api spec which is a LRO. We do not return 200 for this API, only 202. Compilation autogenerates 200 and LintDiff complains that 200 if present needs to have a schema:
```
Rule: PostResponseCodes

Message: 200 return code does not have a schema specified. LRO POST must have a 200 return code if only if the final response is intended to have a schema, if not the 200 return code must not be specified.

Location: Microsoft.AzureFleet/preview/2025-07-01-preview/azurefleet.json#L417

Related RPC [for API reviewers]: RPC-Async-V1-11, RPC-Async-V1-14
```
I tried to manually remove 200 response code from swagger as the "ExplainError" button suggested on the UI but when I run npx tsv command, this is added automatically to the generated swagger. Any suggestions to suppress 200 response code here?

## answer
I think the problem is that you explcitly define the rsponse as being OkResponse in your pr:
```
cancel is ArmResourceActionAsync<Fleet, void, Response=OkResponse>;
```
not sure either what is the right way to define a cancel LRO but changing to this probably does what you want
```
cancel is ArmResourceActionNoResponseContentAsync<Fleet, void>
```

# Best replacement for char in TypeSpec?

## question 
Hi TypeSpec Discussion,
We have a char type property in our contract, but I found that TypeSpec does not support char type.
 
Which type should I use in TypeSpec definition?

## answer
it is usually simpler and more explicit to add maxLength and minLength constraints to the custom scalar, [like this](https://typespec.io/playground/?e=%40typespec%2Fopenapi3&c=QG1heExlbmd0aCgxKQ0KQG1pbssPc2NhbGFyIGNoYXIgZXh0ZW5kcyBzdHJpbmc7DQo%3D&options=%7B%7D&vs=%7B%7D), rather than using the pattern decorator
```
@maxLength(1)
@minLength(1)
scalar char extends string;
```

# Resource Provider Implementation - Generating Models from typespec

## question 
We are a new resource provider implementation utilising RPaaS. We are using typespec to generate our open-api specs.
 
When starting our project we decided not to use typespec-providerhub-controller to generate controller code as it was no longer under active development, with plans for new service emitters at the time https://stackoverflow.microsoft.com/a/451677/180368
Is the service code emitter now available?
We are now formalising our typespec model and looking to utilise any code generation tools for C#/.NET that could keep our API models (but not necessarily controller - we are happy to write those) aligned with the api spec definitions
What is the recommended pattern to generate models for C#/.NET? What patterns have teams had success with and are there any pitfalls to note?
Is there an example repository and pattern we should emulate

## answer
Right now there is no recommendation for RPaaS extensions - typespec-providerhub-controller is still supported.

# Guidelines to Implement Custom Patch

## question 
Hi Team! We are planning to implement a custom model for patch. Are there any guidelines available for this? I found some information about "TrackedResourceUpdate," but it doesn't seem to be included in the package anymore.
If you have any examples, especially on implementing the model for Properties and referencing it for patch calls, that would be very helpful.

## answer
You want to make sure to represent the properties that can be PATCHed in the patch operation, generally this includes any properties that are not generated on the service (e.g. type, id, name, systemData) or settable only on creation (e.g. location), and make sure these properties are optional and have no defaults.
 
You can do this either be creating a separate model type for patch, or by using standard transformations to filter out the readOnly and createOnly properties and make these optional

- [Here is a sample of using a fully bespoke patch model](https://azure.github.io/typespec-azure/playground/?options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D&c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhlRoZSBQQVRDSCDGb2Zvcspz8QCFVXBkYXRlyGT2ARBGb3VuZGF05QEHQXJtVGFnc%2BcAoHk7CucBwXJwLeQDJWlmaWMgcMYdaWXlAfEgyxA%2FOsl5x0JpZXPmAIP0AMBkb%2BcAwMxeZcdBz2fuANfQXukCaUFnZSBvZspF5QChYWdlPzogaW50MzLpANFDaXR50ipjaXR5Pzogc3Ry5QPPxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAkZwxjA%2FOiBieXRlc%2BkA%2B%2BkBp%2F8A28gc%2FwDV%2FwDV%2FwDV%2FwDV%2FwDVc8lI5AHQc3RhdHVzxEt0aGUgbGFzdCDkAMXlAlTlBLcgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AKLYWTHXcQg5QVGU3RhdMRn5QNmzBTtAkPMMuUAgOUAymHpA5DFd0Bscm%2FEO3VzCnVu5ARt0VTlAWTmARos7ADSyEcgY3JlxCdyZXF1ZXN0IGhhcyBiZWVuIGFjY2VwdGVkxGcgIEHHDjogIsgLItZQacRA5AC06QDByETsAJw6ICLMD9pMdeQC%2F8RPxUPlAw3GP8gLyjvpBmDpAMTmANxk5wGiU3VjY2VlZOUAxckM0z%2FFNuQBTWZhaWzJPkbFDTogIsYJ3Dh3YXMgY2FuY2XKPkPHD%2BQGuMcL%2FwFAIGRlbGXpAYBExA3mAPnICyLpBbjpA3dtb3bqAcrpA3lNb3ZlUscV6ANyxHNtb3bEaGZyb20gbG9j5gC8xW7EE%2FEDUMszdG%2FPMXRvyi%2F3AJVzcG9uc%2BsFweYAlscW7ACX7gNsxT7FZMZ85gLuzW5pbnRlcmbkB9dP6AOUcyBleHRlbmRz9giZLsspe30K5Qh1yCPKG8tZ6ADN5gSzZ2V05AGnQco15APn7AbLIOcCe09y5wW%2Fzi9Dxx1SZXBsYWNlQXN5bmPOP%2BUC98g3Q3VzdG9tUGF0Y2hTzCos7wcGxTrmAjfPceUCNGVXaXRob3V0T2vTcWxpc3RCecgwR3JvdXDPREzFIlBhcmVudNQ8U3Vic2NyaXDlAhbGO8YzzBnMOegF%2FSBzYW1wbOsDA2FjxUR0aGF05gIA6QXAdG8gZGlmZmXkAITvAoPFKe4AskHFSO8BM%2BsDDsgN5gKF8wCSSEVBROoF6cR%2BY2hlY2vqAKpleGlzdGVu5gkeIMYeRckU7wH1zR3uCPk%3D&e=%40azure-tools%2Ftypespec-autorest&vs=%7B%7D)
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** The PATCH model for Employee */
model EmployeeUpdate {
  ...Azure.ResourceManager.Foundations.ArmTagsProperty;

  /** rp-specific properties */
  properties?: EmployeePropertiesUpdate;
}

/** The PATCH mdoel for rp-specific employee properties */
model EmployeePropertiesUpdate {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<Employee, EmployeeUpdate>;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
- [Here is a sample of using transformations](https://azure.github.io/typespec-azure/playground/?options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D&c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BQAhmFsaWFzIFBhdGNoTcRsPAogIFQgZXh0ZW5kcyB7fSwKICBPbWl0dGVkymLJIHN0cuQCjD0gIiLEKcRpVGVtcGxhdGXJJHZhbHVlb2bLLHvkAhd9VXBkYXRlIgo%2BID0gxgxhYmzrAMc8T3B0aW9uYWzME21pdERlZmF1bHTGDcsc5AC59QCuCj4%2BPj47Cu8BTMogxn3kAVzrAQfSJ8RD5AG0VGhlIFBBVENIIMZRZm9yIOQA9MQu5QKwzmjGXgog12As8wFePSAi5AEiIiB8ICJwyRci5gHY5ACDcnAt5ARgaWZpYyBwxFTkATogyivmAwLKED86%2BQD35QIFxFTIIc9H%2BAE16QCMQWdlIG9m6QECxnthZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5PzrnAh3JLFByb2ZpbNNZQGVuY29kZSgiYmFzZTY0dXJsIuQDZXDGMD86IGJ5dGVzyUjkAaxzdGF0dXPES3RoZSBsYXN0IOQAxWHkAl3lBQEgIEB2aXNpYmlsaXR5KExpZmVjeWNs5APlYWTHXcQg5QWQU3RhdMRn5QOwzBTpAUjEc8wy5QCA5QDKYekD2sV3QGxyb8Q7dXMKdW7kBLfRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx15AJixE%2FFQ%2BUCcMY%2FyAvKO%2BkGqukAxOYA3GTnAaJTdWNjZWVk5QDFyQzTP8U25AFNZmFpbMk%2BRsUNOiAixgncOHdhcyBjYW5jZco%2BQ8cP5AcCxwv%2FAUAgZGVsZekBgETEDeYA%2BcgLIukGAukDd21vduoByukDeU1vdmVSxxXoA3LEc21vdsRoZnJvbSBsb2PmALzFbsQT8QNQyzN0b88xdG%2FKL%2FcAlXNwb25z6waQ5gCWxxbsAJfuA2zFPsVkxnzmAu7NbmludGVyZuQIIU%2FoA5TqBl72B2zLKXt9CuUIv8gjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwHFSDnAntPcuoGGcsvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbeUF%2BVPMKizvBiHFOuYCN89x5QI0ZVdpdGhvdXRPa9NxbGlzdEJ5yDBHcm91cM9ETMUiUGFyZW501DxTdWJzY3Jp5QdxxzvGM8wZzDnoBf0gc2FtcGzrAwNhY8VEdGhhdOYCAOkFwHRvIGRpZmZl5ACE7wKDxSnuALJBxUjvATPrAw7IDeYChfMAkkhFQUTqBenEfmNoZWNr6gCqZXhpc3RlbuYJaCDGHkXJFO8B9c0d7glD&e=%40azure-tools%2Ftypespec-autorest&vs=%7B%7D)
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

alias PatchModel<
  T extends {},
  OmittedProperties extends string = "",
  NameTemplate extends valueof string = "{name}Update"
> = UpdateableProperties<OptionalProperties<OmitDefaults<OmitProperties<
  T,
  OmittedProperties
>>>>;

model EmployeePropertiesUpdate is PatchModel<EmployeeProperties>;

/** The PATCH model for employees */
model EmployeeUpdate
  is PatchModel<Employee, OmittedProperties = "name" | "properties"> {
  /** rp-specific patchable properties */
  properties?: EmployeePropertiesUpdate;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<Employee, EmployeeUpdate>;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
The advantage to using transformations is that, as the model versions, the PATCH model will also version automatically, whereas if you use a completely bespoke patch model, you will need to remember to make appropriate versioning changes there as well as in the resource model.

# We cannot customize the Type for ResourceNameParameter?

## question 
Like [this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTEsIOQAsCA9IFRlc3QsIMYldHRlcm4gPSAiIj475ACldW7kAVHEIsVVIkEiLCAiQiLoAMDITCBwyX7yALbqAJrpAb1BZ2Ugb2YgZcg%2F5QGyYWdlPzogaW50MzI7CscpQ2l0edIqY2l0eT86IHN0cuUDI8csUHJvZmls01lAZW5jb2RlKCJiYXNlNjR1cmwi5AGacMYwPzogYnl0ZXPJSFRoZSBzdGF0dXPES3RoZSBsYXN0IOQAxWF0aW9u5QM2ICBAdmlzaWJpbGl0eShMaWZlY3ljbOQCGmFkx13EIOUDxVN0YXTEZ%2BUB5cwU5QFjyHPMMuUAgOUAymHpAg%2FFd0Bscm%2FEO3Vz5wGb0VTlAWTmARos7ADSyEcgY3JlxCdyZXF15AHXaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QTf6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBTfHC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QQ36QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBMXmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AZWT%2BgDlHMgZXh0ZW5kc%2FYHGC7LKXt9CuUG9MgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BkFSj475ASU5QJ7T3LlAqflAdbLL0PHHVJlcGxhY2VBc3luY84%2F5QL3yDdDdXN0b21QYXRjaFPEKgogICDpAJMsxQ72AOdGb3Vu5AMm5AZ%2FyBzmAJFN5AGEyXMs8wWdPgogIOUAi%2BYCiO8AwuUChWVXaXRob3V0T2vzAMJsaXN0QnnIMEdyb3Vwz0RMxSJQYXJlbnTUPFN1YnNjcmlw5QJnxjvGM8wZzDnoBk4gc2FtcGzrA1RhY8VEdGhhdOYCUekGEXRvIGRpZmZl5ACE7wLUxSnuALJBxUjlAYTIdyzsA1%2FIDeYC1vMAkkhFQUTqBjrEfmNoZWNr6gCqZXhpc3RlbuYH7iDGHkXJFO8CRs0d7AEJfQo%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D), 
```
/** Contoso Resource Provider management API. */
@armProviderNamespace
@service(#{ title: "ContosoProviderHubClient" })
@versioned(Versions)
namespace Microsoft.ContosoProviderHub;

/** Contoso API versions */
enum Versions {
  /** 2021-10-01-preview version */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @armCommonTypesVersion(Azure.ResourceManager.CommonTypes.Versions.v5)
  `2021-10-01-preview`,
}

/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee, Type = Test, NamePattern = "">;
}

union Test {
  "A", "B"
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<Employee, EmployeeProperties>
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
it throws "Cannot apply @pattern decorator to type it is not a string". Look like a bug?
```
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee, Type = Test, NamePattern = "">;
}

union Test {
  "A", "B"
}
```

## answer
As we discussed in this issue: [List resources with two different resources but same resource type · Issue #1911 · Azure/typespec-azure](https://github.com/Azure/typespec-azure/issues/1911)  You can handle this [like this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIkRpYWdub3N0aWNzQ2xpZW50IiB9KQpA5wE8ZWQo5wC4cykKbshJIE1pY3Jvc29mdC7LP%2B8AqEFQSSDHRnPkAJNlbnVt6AEFcyB7CiAgxC4yMDIxLTEwLTAxLXByZXZpZXfINcQ0ICBAdXNlRGVwZW5kZW5jeSj1ASEuyFYudjFfMF9QxkhfMSnEQGFybUNvbW1vblTkAcHHKtdIyynLVDXESGDyAKlgLAp95gDzQegA9egBc0h1YiDoAiLlAY9sb2NhdGlvbshyCm1vZGVsIOoBPiBpcyBUcmFja2VkyCQ8yh5Qcm9wZXJ0aWVzPukBOlRoZSBkykDkAZ3EayAgI3N1cHByZXNz%2FwLC7wLCL2FybcoVxE8tcGF0dGVybiLkAUh2aXNpYmlsaXR5KExpZmVjeWNs5AFBYWTlAWZwYXRoxAhzZWfkAogoIuoAmnMixSJrZXnMFuQClcUZxGk66wEJxBlzO%2BkBWmxsb3dlZMUmcyBmb3LrAPPlAlZ1buQCK88%2F6QEkQSBzcG90IHJlcGxhY%2BYDInJlY29tbeQCUMxMxEsgIFNwb3RSyixSyis6ICLEStYdIiwKyXBkaXNrSW7kAWfkAe%2FRYkTNHzogIs4wxkpzdHJpbmfpAl5FbXBsb3llZSBw6QIGxE3wAkLqAiTqAP9nZSBvZiBlyEHlAINhZ2U%2FOiBpbnQzMjvoALhDaXR50ipjaXR5PzrnAI%2FJLFByb2ZpbNNZQGVuY29kZSgiYmFzZTY0dXJs5QHhcMYwPzogYnl05AHb6wLAc3RhdHVzxEt0aGUgbGFzdCDkAMXlAyvlBMj%2FAnnEXcQg5QVXU3RhdMRn5QOFzBTpAk7Ec8wy5QCA5QDKYekDr8V3QGxyb8Q7dXPnAmHRVOUBZOgBqesA0shHIGNyZcQncmVxdWVzdCBoYXMgYmVlbiBhY2NlcHRlZMRnICBBxw46ICLIC%2BoCTs1QacRA5AC06QDByETsAJw6ICLMD9pMdXBkYXTET8VDVccOOiAiyAvKO%2BkGcekAxOYA3GTnAaJTdWNjZWVk5QDFyQzTP8U25AFNZmFpbMk%2BRsUNOiAixgncOHdhcyBjYW5jZco%2BQ8cPOiAiyAv%2FAUAgZGVsZekBgETEDeYA%2BcgLIvIDeW1vduoByukDe01vdmVSxxXsBZVtb3bEaGZyb20g6AX4xm7EE%2FEDUMszdG%2FPMXRvyi%2F3AJVzcG9uc%2BUDguwAlscW7ACX7gNsxT7FZMZ85gLuzW5pbnRlcmbkB%2B9P6AOUcyBleHRlbmRz9giqLsspe30K5QiGyCPKG8tZ6wg%2B5QCsZ2V05AGpQco35APp6wcJPjvkBJjlAn9PcuUCq%2BUB2ssxQ8cd5wWrQXN5bmPQQeUC%2Fcg5Q3VzdG9tUGF0Y2hTxCwKICDkBajIMSzFEPYA70ZvdW7kAy7kCCfIHOYAlU3kAYzGS9JNzBLqBbbFGz4KICDlAKPmAqbvANzlAqNlV2l0aG91dE9r9QDcbGlzdEJ5yDJHcm91cM9GTMUiUGFyZW501j5TdWJzY3JpcOUCicY9xjXMGc476gdxYW1wbOsDeGHmBwl0aGF05gJ16QY1dG8gZGlmZmXkAIjvAvjFKe4AtkHFSOUBosp5LOwDhcgN5gL88wCUSEVBROoGYOQAgGNoZWNr6gCsZXhpc3RlbuYJtCDGHkXJFO8CaM0d7gENfQo%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D) .  
```
/** A ContosoProviderHub resource */
@locationResource
model Diagnostic is TrackedResource<DiagnosticProperties> {
  /** The diagnostic name */
  #suppress "@azure-tools/typespec-azure-resource-manager/arm-resource-name-pattern"
  @visibility(Lifecycle.Read)
  @path
  @segment("diagnostics")
  @key("diagnosticName")
  name: DiagnosticNames;
}

/** Allowed names for diagnostics */
union DiagnosticNames {
  /** A spot replacement recommender diagnostic */
  SpotReplacementRecommender: "spotReplacementRecommender",

  /** A diskInspection diagnostic */
  DiskInspection: "diskInspection",

  string,
}

/** Employee properties */
model DiagnosticProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Diagnostics {
  get is ArmResourceRead<Diagnostic>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Diagnostic>;
  update is ArmCustomPatchSync<
    Diagnostic,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Diagnostic,
      DiagnosticProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Diagnostic>;
  listByResourceGroup is ArmResourceListByParent<Diagnostic>;
  listBySubscription is ArmListBySubscription<Diagnostic>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Diagnostic, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Diagnostic>;
}
```

The ResourceNameParameter should handle this, but doesn't.
Fixing that bug would help, but, fundamentally, the user really shouldn't have to override both parameters.
 
Filed this issue for a specific fix: [ResourceNameParameter broken for non-string Types · Issue #2759 · Azure/typespec-azure](https://github.com/Azure/typespec-azure/issues/2759)
 
We should not apply the default pattern, and emit a specific diagnostic if the user supplies a custom pattern with a custom type.

# Is it so bad to introduce named types to replace unnamed ones?

## question 
RE [Yuxia/20250401preview by blankor1 · Pull Request #33507 · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/pull/33507#issuecomment-3029062363)
 
I notice a suppression in the typespec PR 
 
#suppress "@azure-tools/typespec-client-generator-core/no-unnamed-types" "Backwards compatibility with existing clients."
 
Sounds like the PR author is under the impression its a bad idea to get rid of unnamed types and replace them with named ones, because of concerns about backwards compatibility. Is this actually correct?
 
Or would we consider this a benign change? And a best practice to 'fix' unnamed types by creating named types to replace them?

## answer
Introducing named types to replace unnamed ones is not a bad idea—in fact, it's preferred. The linter rule @azure-tools/typespec-client-generator-core/no-unnamed-types exists to encourage naming types. However, in the PR you referenced, there was a suppression added with the justification of "Backwards compatibility with existing clients."

After reviewing the context, it seems this suppression may not be necessary. The rule itself is currently disabled due to performance issues, so removing the suppression won’t trigger CI. Conceptually, this is a false positive: the anonymous model created via @bodyRoot is a temporary structure that the SDK won’t use, so it shouldn't be flagged.

Still, the current template resolves to a borderline case where the SDK might auto-generate a name. A cleaner approach would be to use @@clientName to rename the body parameter directly, avoiding the anonymous model altogether. This is considered a better practice.

So yes—replacing unnamed types with named ones is generally a good idea, and in this case, the suppression may not be needed.

# Upper case in action segment

## question 
Last time I confirmed that route is case sensitive. Now I have a customer whose route is like
```
/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ContosoProviderHub
```
`/employees/{employeeName}/All`. The action segment is capitalized. I tried [this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeQEs%2BgEtCBzYW1wbOsBumFjxDcgdGhhdOYAt%2BkEd3RvIGRpZmZlcuQHL%2B4BOkDGNigiQWzmBIfFOmlzIEHqAJNBxSNTeW5j6QV0LOwB1sgN5gFNPjsKCn0K&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D). Seems not work.
```
import "@typespec/http";
import "@typespec/rest";
import "@typespec/versioning";
import "@azure-tools/typespec-azure-core";
import "@azure-tools/typespec-azure-resource-manager";

using Http;
using Rest;
using Versioning;
using Azure.Core;
using Azure.ResourceManager;

/** Contoso Resource Provider management API. */
@armProviderNamespace
@service(#{ title: "ContosoProviderHubClient" })
@versioned(Versions)
namespace Microsoft.ContosoProviderHub;

/** Contoso API versions */
enum Versions {
  /** 2021-10-01-preview version */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @armCommonTypesVersion(Azure.ResourceManager.CommonTypes.Versions.v5)
  `2021-10-01-preview`,
}

/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {

  /** A sample resource action that move employee to different location */
  @action("All")
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

}
```

## answer
For ARM APIs, segments are mandated to be case insensitive, so this should not be an issue for ARM APIs.  Does seem like an issue for `@action` in general, though.
Note that you can use the private decorator `@Rest.Private.actionSegment` to get around the normalization done by `@action`  [like this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwFECDnAntPcuUCp%2BUB1ssvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbVBhdGNoU8QqCiAgIOkAkyzFDvYA50ZvdW7kAybkBkXIHOYAkU3kAYTGSdBLyhDqBarFGT4KICDlAJ3mAprvANTlApdlV2l0aG91dE9r8wDUbGlzdEJ5yDBHcm91cM9ETMUiUGFyZW501DxTdWJzY3JpcOUCecY7xjPMGcw56AZgIHNhbXBs6wNmYWPFRHRoYXTmAmPpBiN0byBkaWZmZeQAhO8C5kDkCVsuUHJpdmF0ZS7GQ1NlZ%2BQJBSgiQWzmBkfFTu4A10HGKuwBESzsA5bIDeYDDfMAt0hFQUTqBnHkAKNjaGVja%2BoAz2V4aXN0ZW7mB%2Bsgxh5FyRTvAn3NHe4Hxg%3D%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D). 
```
interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  @Rest.Private.actionSegment("All")
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
I don't think there will be a need for this in ARM APIs

# Question regarding extendable union types

## question 
Hi API Spec Review team,
We are in the process of writing our first Typespec based GA documentation and I have a question regarding extendable unions.
It seems that from the following docs, that extendable unions (for example of base type "string"), allow for additional values to be accepted besides the provided ones:
https://azure.github.io/typespec-azure/docs/libraries/azure-core/rules/no-enum/
However, having looked at other RP's implementation of this, together with their published docs on MS learn, I can't see evidence of their extendibility, and can't understand how clients are supposed to know that the provided enumeration types are extendable.
 
 
For example, while the following union is marked as extendable in the codebase, I can't see how this gets translated to docs:
```
namespace DevCenterService {
    @doc("The operating system type.")
    union OsType {
        @doc("The Windows operating system.") 
        Windows: "Windows",
        string,
    }

    @doc("Indicates whether hibernate is supported and enabled, disabled, or unsupported by the operating system. Un
    union HibernateSupport {
        @doc("Hibernate is enabled.")
        Enabled: "Enabled",

        @doc("Hibernate is not enabled.")
        Disabled: "Disabled",

        @doc("Hibernate is not supported by the operating system.")
        OsUnsupported: "OsUnsupported",

        string,
    }
}
```

```
HibernateSupport
Enumeration
Indicates whether hibernate is supported and enabled, disabled, or unsupported by the operating system. Unknown hibernate support is represented as null.

Value	        Description
Disabled	        Hibernate is not enabled.
Enabled	        Hibernate is enabled.
OsUnsupported	   Hibernate is not supported by the operating system.
```

This is important to us as we have several fields who's values we would like to be able to extend over time (possibly even within the same version) and we don't want clients taking dependency on any closed set.
 
Thanks

## answer
I don't know how this is represented in api documentation, but the mechanism for how is the x-ms-enum extension that is emitted in Swagger, which is the same thing we do for swagger-based APIs.
 
This would also show up in the typing system of some SDK and CLI client SDKs (depending on the language typing capabilities)
 
This is certainly something you can put into the documentation comments for the particular property involved to guarantee that it is represented in docs.
 
One thing - there is currently no type distinction between extensible unions that can add values within the current api-version, and those that would only add new values in new api-versions, so that is something you should make clear in property documentation.
 
Here is the onboarding page for API Documentation in the Azure SDK Portal: [Publish REST API reference docs](https://eng.ms/docs/products/azure-developer-experience/design/api-docs)
 
This includes a link to the channel for the docs team: https://teams.microsoft.com/dl/launcher/launcher.html?url=%2F_%23%2Fl%2Fchannel%2F19%3A7506cc3e220f430ab89d992c7db5284f%40thread.skype%2FAPI%2520Reference%2520and%2520Samples%3FgroupId%3Dde9ddba4-2574-4830-87ed-41668c07a1ca%26tenantId%3D72f988bf-86f1-41af-91ab-2d7cd011db47&type=channel&deeplinkId=4d931d59-a8ff-489e-a010-100f7f950bd6&directDl=true&msLaunch=true&enableMobilePage=true&suppressPrompt=true

# Does TypeSpec have the option to achieve mutually exclusive?

## question 
We have 2 properties `awsCloudProfile`and `gcpCloudProfilein` our preperties in our typespec update. These 2 properties are mutually exclusive(ensuring that only one of `awsCloudProfile` or `gcpCloudProfile` can exist at a time). Besides setting both of them to optional, is there any other options in TypeSpec that we can achieve the mutually exclusive without causing a breaking change? 

## answer
There isn't currently a built-in way in TypeSpec to enforce mutual exclusivity between properties like `awsCloudProfile` and `gcpCloudProfile`.

In your PR, you're making both properties optional — but that’s actually a breaking change. Clients may have previously assumed that `awsCloudProfile` was always present, and now that constraint is being relaxed, which breaks existing behavior.

Azure's recommended pattern for such cases is to use **explicit discriminated unions** to simplify the client experience. That means defining a `CloudProfile` union with a `@discriminator("kind")` to clearly indicate which cloud provider the profile belongs to. For example:

```typescript
@discriminator("kind")
union CloudProfile {
  aws: AwsCloudProfile;
  gcp: GcpCloudProfile;
}

model AwsCloudProfile {
  kind: "aws";
  awsAccountId: string;
}

model GcpCloudProfile {
  kind: "gcp";
  projectProperties?: GcpProjectProperties;
  organizationProperties?: GcpOrganizationProperties;
  isOrganization?: boolean = false;
}
```

With this structure, your `CloudConnectorProperties` model just needs a single `cloudProfile` property. It allows you to support AWS, GCP, or any other provider in future API versions without breaking changes.

To clarify a few key points:

1. **Is the `kind` property set by the customer?**
   Yes, it appears on the wire. In some SDKs, the customer needs to set it explicitly; in others, it may be inferred by the SDK type system.

2. **Does `kind` already exist in your types?**
   No, it doesn’t. This change would be a breaking change — but one that allows forward compatibility when adding more cloud providers.

3. **Would the `hostType` property still be needed?**
   No, it would become redundant if you adopt this `@discriminator` union pattern.

As a side note: TypeSpec has introduced a `@discriminated` decorator for unions, which supports more flexible patterns, but it is not yet supported by all SDK emitters. It’s under investigation and targeted for support around August. However, the `@discriminator` pattern (which you are using here) is already required to be supported by all SDK emitters and is safe to use today.

# Two kinds of extension resource

## question 
I'm preparing documents related to extension resource for our customers. We seem to have two ways to represent extension resource.
1. `@extensionResource` directly on whatever resource model.
2. Directly use `Azure.ResourceManager.ExtensionResource`. This way, the resource model cannot be a tracked resource. Correct me if I'm wrong.

I feel like option 1 suffices, and it should be the recommended way? 
My thinking is, I don't want to write such lines in the documentation like: if it is not tracked resource, use `Azure.ResourceManager.ExtensionResource`, otherwise you should use `@extensionResource`. I just want to write something like: just use `@extensionResource` to mark an extension resource.

## answer
Talked offline, the `Azure.ResourceManager.ExtensionResource` is the recommended way. Even the resource model has the same properties as those in `TrackedResource`, we still should use `ExtensionResource` because `resourceKind` matters when service side calls `resolveArmResources`.

# Using namespaces and encountering duplicate-symbol error

## question 
I'm trying to decorate some types with a namespace, but I'm getting a duplicate-symbol error. Is there something wrong with the usage in the example below? I don't have any other usages of `namespace Chat` or `namespace OpenAI.Chat`.
```
namespace OpenAI.Chat{

Error message:
Duplicate name: "Chat" Typespec(duplicate-synbol)
namespace OpenAI.Chat
```

Something interesting I noticed is that there is another duplicate error for an interface declaration. Do these two concepts conflict?
```
@route("/chat")
interface Chat{
  @route("completions")
  @post
  @operationId("createChatCompletio
  @tag("Chat")
  @summary("Creates a model respons
  createChatCompletion(
```

## answer
yes names should be unique across all types
Every container (namespace, interface, model) identifies its elements by name.  So, within a container, all types have to have unique names.  Because namespaces can contain different types (including other namespaces), this is where you are most likely to encounter the issue.

# Internal compiler error

## question 
I have an interface in typespec with the following method:
```
  @operationId("VirtualMachines_UnClaim")
  unClaim is ArmResourceActionAsync<LabVirtualMachine, void, AcceptedResponse>;
```
This throw compilation error as follows:
```
TypeSpec compiler v1.1.0

× Compiling...
Internal compiler error!
File issue at https://github.com/microsoft/typespec

Error: Multiple responses are not supported.
    at getResponseBody (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@azure-tools/typespec-azure-resource-manager/dist/src/rules/arm-post-response-codes.js:21:23)
    at validateAsyncPost (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@azure-tools/typespec-azure-resource-manager/dist/src/rules/arm-post-response-codes.js:42:29)
    at root (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@azure-tools/typespec-azure-resource-manager/dist/src/rules/arm-post-response-codes.js:86:29)
    at file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/linter.js:125:49
    at time (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/stats.js:11:5)
    at timedCb (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/linter.js:125:38)
    at EventEmitter.emit (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/semantic-walker.js:367:17)
    at listener.<computed> [as root] (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/semantic-walker.js:63:26)
    at Object.emit (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/semantic-walker.js:81:49)
    at navigateProgram (file:///C:/Users/nandiniy/v3/azure-rest-api-specs/node_modules/@typespec/compiler/dist/src/core/semantic-walker.js:14:13)
```
If the response is OkResponse in the method then I am able to compile it successfully but I want to update the generated swagger such that the response type is either 202 or default based on the lint rule: [azure-openapi-validator/docs/post-response-codes.md at main · Azure/azure-openapi-validator](https://github.com/Azure/azure-openapi-validator/blob/main/docs/post-response-codes.md):
```
Description
Synchronous POST operations must have one of the following combinations of responses - 200 and default ; 204 and default. They also must not have other response codes. Long-running POST operations must have responses with 202 and default return codes. They must also have a 200 return code if only if the final response is intended to have a schema, if not the 200 return code must not be specified. They also must not have other response codes. 202 response for a LRO POST operation must not have a response schema specified.
```

## answer
Since this is an Async operation, the accepted response is part of the template.  What you want to provide in this case is the logical response (here, it sounds like void, [as in this playground](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwFECDnAntPcuUCp%2BUB1ssvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbVBhdGNoU8QqCiAgIOkAkyzFDvYA50ZvdW7kAybkBkXIHOYAkU3kAYTGSdBLyhDqBarFGT4KICDlAJ3mAprvANTlApdlV2l0aG91dE9r8wDUbGlzdEJ5yDBHcm91cM9ETMUiUGFyZW501DxTdWJzY3JpcOUCecY7xjPMGcw56AZgIHNhbXBs6wNmYWPFRHRoYXTmAmPpBiN0byBkaWZmZeQAhO8C5sUp7gCyQcVI5QGWyHcsIHZvaWTGBvMAg0hFQUTqBj3Eb2NoZWNr6gCbZXhpc3RlbuYHtyDGHkXJFM5%2FQ80d7geS&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D)).
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, void, void>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```

# Is it possible to make a field to be optional only on new api version?

## question 
Hi TypeSpec Discussion,
 
In swagger, we have below data model. We want to make StageSpec.specification to be optional from 2025-06-01-preview version, but for older versions, it is still mandatory.
 
I tried below implementation, but it failed during tsp compile. Compiler complains the field name is duplicated.
 
Is there any way to achieve this? Really appreciate if there's any examples.
```
Diagnostics were reported during compilation:
 
[36mWorkflowVersion.tsp[39m:[33m95[39m:[33m3[39m - [31merror[39m [90mduplicate-property[39m: Model already has 
a property named specification
> 95 |   specification?: Record<unknown>;
     |   ^^^^^^^^^^^^^
 
Found 1 error.
```
```
@doc("Stage Properties")
model StageSpec {
  @doc("Name of Stage")
  name: string;

  @removed(Versions.v2025_06_01_preview)
  @doc("Target ARM id")
  targetId?: string;

  #suppress "@azure-tools/typespec-azure-core/no-unknown" "Suppress no-unknown to handle the datatype object used in dependent service"
  #suppress "@azure-tools/typespec-azure-resource-manager/arm-no-record" "Suppress arm-no-record to handle the datatype object used in dependent service"
  @doc("Stage specification")
  @removed(Versions.v2025_06_01_preview)
  specification: Record<unknown>;

  #suppress "@azure-tools/typespec-azure-core/no-unknown" "Suppress no-unknown to handle the datatype object used in dependent service"
  #suppress "@azure-tools/typespec-azure-resource-manager/arm-no-record" "Suppress arm-no-record to handle the datatype object used in dependent service"
  @doc("Stage specification")
  @added(Versions.v2025_06_01_preview)
  specification?: Record<unknown>;

  @added(Versions.v2025_06_01_preview)
  @doc("List of tasks in the stage")
  tasks: TaskSpec[];

  @added(Versions.v2025_06_01_preview)
  @doc("Task option for the stage")
  taskOption?: TaskOption;
}
```

## answer
You should use the @madeOptional decorator, here: [Decorators | TypeSpec](https://typespec.io/docs/libraries/versioning/reference/decorators/#@TypeSpec.Versioning.madeOptional).  Note that, making a property that occurs in responses optional is considered a breaking change.

# Array items with length constraints

## question 
Hi all. I got a model with an [array](https://github.com/Azure/azure-rest-api-specs/blob/72d218043b48fbf70c67382d7d90e2018cfb37da/specification/storage/resource-manager/Microsoft.Storage/stable/2024-01-01/blob.json#L1267) and items is constraint by length. I wonder if this is legal in OpenAPI? 
```
 "tags": {
          "type": "array",
          "items": {
            "type": "string",
            "maxLength": 23,
            "minLength": 3
          },
          "description": "Each tag should be 3 to 23 alphanumeric characters and is normalized to lower case at SRP."
        },
```
 
If it's legal, how could I implement this constraint in typespec?

## answer
If you meant that you wanted to constrain the number of items in the array, you can do this with the @[minItems](https://typespec.io/docs/standard-library/built-in-decorators/#@minItems) @[maxItems](https://typespec.io/docs/standard-library/built-in-decorators/#@maxItems) decorators: 
 
And this is perfectly legal in Azure Specs (it is best that you encode these kinds of limits, if only for documentation purposes).
 
However, changing these limits in the future can result in breaking changes for customers

# Asynchronous resource delete

## question 
Hi!
 
I'm starting to add new functionality to an existing TypeSpec ([azure-rest-api-specs/specification/orbital/Microsoft.PlanetaryComputer at main · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/tree/main/specification/orbital/Microsoft.PlanetaryComputer)). One of the endpoints I need to implement performs an asynchronous deletion of a resource. I modeled it using `LongRunningResourceDelete` operation:
```
@tag("AI Workflows")
@route("ai")
@added(Versions.v2025_09_30_Preview)
interface AiWorkflows {
  ...

  getAiWorkflowResourceOperationStatus is StandardOperations.GetResourceOperationStatus<AiWorkflow>;

  @pollingOperation(AiWorkflows.getAiWorkflowResourceOperationStatus)
  deleteAiWorkflow is StandardOperations.LongRunningResourceDelete<AiWorkflow>;

  ...
}
```
This emits an OpenAPI with the following endpoints:

DELETE /ai/workflows/{workflowId}
GET /ai/workflows/{workflowId}/operations/{operationId}

I'm trying to understand how this should be implemented, and my first surprise is that I couldn't find a single use of LongRunningDeleteResource in the entire azure-rest-api-specs repo (apart from the widgets example ). Also, I've searched this channel and didn't find any useful information.
 
If the operation status monitor is a child of the resource being deleted, how can I access the status once the delete action is completed? As a client, should I check the resource monitor until I get a 404 error response? Should the status monitor still be available even if the resource was deleted? If the latter is true, what should happen if a new resource is created with the same id?

## answer
The API guidance on lro delete operations is here: [api-guidelines/azure/Guidelines.md at vNext · microsoft/api-guidelines](https://github.com/microsoft/api-guidelines/blob/vNext/azure/Guidelines.md#delete-lro-pattern):

```
✅ DO use the following pattern when implementing an LRO operation to delete a resource:

DELETE /UrlToResourceBeingDeleted?api-version=<api-version>
operation-id: <optionalStatusMonitorResourceId>

The response must look like this:

202 Accepted
operation-id: <statusMonitorResourceId>
operation-location: https://operations/<operation-id>

Consistent with non-LRO DELETE operations, if a request body is specified, return 400-Bad Request.

✅ DO allow the client to pass an Operation-Id header with an ID for the operation's status monitor.

✅ DO generate an ID (typically a GUID) for the status monitor if the Operation-Id header was not passed by the client.

✅ DO return a 202-Accepted status code from the request that initiates an LRO if the processing of the operation was successfully initiated.

⚠️ YOU SHOULD NOT return any other 2xx status code from the initial request of an LRO -- return 202-Accepted and a status monitor even if processing was completed before the initiating request returns.
```
 
It is generally required that the StatusMonitor for a particular operation remain for some time after the operation completes.  The simplest way to do this for delete is to use an operations endpoint not using the resource endpoint,  [as in this playground](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3JlIjsKCnVzaW5nIEh0dHA7xwxSZXN0yAxWyVfIEkHEPi5Db3Jl0hIuVHJhaXRzOwoKQHVzZUF1dGgoCiAgQXBpS2V5xA48xgtMb2NhdGlvbi5oZWFkZXIsICJhcGkta2V5Ij4gfCBPxCoyxS9bCiAgICB7xQYgIOQAwTrHH0Zsb3dUeXBlLmltcGxpY2l0LMclYXV0aG9yaXrFYVVybDogIuQBTXM6Ly9sb2dpbi5jb250b3NvLmNvbS9jb21tb24vb8Q1Mi92Mi4wL8hAZSLIUnNjb3BlczogW8lJd2lkZ2V0zUouZGVmYXVsdCJdxjd9CiAgXT4KKQpAc2VydmljZSgjeyB0aXRsZTogIkPGOSBXxUggTWFuYWdlciIgfccvZXLkATcie2VuZHBvaW50fcdy5QCT0D5BUElzxRnmAScvKiogClN1cOQCAWVkyCVT5gCFcyDIU3MgKHByb3RvY29sIGFuZCBob3N0bmFtZSwgZm9yIGV4YW1wbGU6CukA8mVzdHVzLmFwaS7yAP0pLgogKi%2FFfshfOiBzdHJpbmfkAJh95AEG5wKfZWQo5wC%2FLuYAmucA%2FC7nAmhzKQrkAIZzcGFjZSDVKjsKCuQA41RoySDvAUcg5wFxIMdzLuQAnGVudW3oAsxz5QEqxEXHESAyMDIyLTA4LTMxxCwgIOQCvURlcGVuZGVuY3ko6wLgyEMudjFfMF9QcmV2aWV3XzIpCiAgYMlGMGAsCn0KCi8vIE1vZGVscyAv0wHqAMdjb2xvciBvZiBhIOcBXOQAhnVu5ACa5gDbQ8Ui5AC26AFW5wDBQmxhY2vHJSDGJuUAwcUYOiAixQgiyS9XaGl0ZdMvxRg6ICLFCMovUuoCQMstUmVkOiAiUmVkyilHcmVl6ACszCvFGDogIsUIyi9CbHX0AIbEF%2BUAtHXkAITkAUEqKiBB7AEUQHJlc291cmNlKCLGFnMiKQpt5AFkyE%2FoAdrkAVjGIyDkAlDFPiAgQGtlecg7TmFtZSLkAbhAdmlzaWJpbGl0eShMaWZlY3ljbGUuUmVhZMQexD7oAs076ADBy17lAb3HX8UMOuwBtM0ySUTkAeV0yTwncyBtYW51ZmFjdHVyZchFzBNJZM17Li4uRXRhZ1Byb3BlcnR5O%2BcCZU9wZXLlBO74AmlhbGlhc%2BgEA%2BYFoiA9IOcEJHNSZXBlYXRhYmxlUmVxdWVzdHMgJgogyR9Db25kacReYWzWIGxpZW50xxtJZDvIb%2BsAlj3sBiJS5wHDyiA87QCaPuQGPeoB6OQA68Yn7wHrRGVsZXTKRlN0YXR1cyBpcyBGb3VuZMYzLs8fPG5ldmVyPjsKaW50ZXJm5ARkxk3nBBPrAUwgxjLoAuHEI3PGY%2BUDqcY3IOkAnugCYXNoYXJlZFJvdXRlCiAgZ2V0xinzAKfJE3MuR2V08QEWxik8xj8%2BO%2F8AiGTlAQ3%2FAI%2FqAI%2F5ATzJE3PpAajkAwnnAIfVN%2BQBsOUBO8Yky0PnALRDcmVhdGVzIG9yIHVwZMUL6QC4YXN5bmNocm9ub3VzbHnnALVwb2xsaW5nyUkoxi1zLvgBT%2BQDqGPFZk9yVcVjx1%2FuANJMb25nUnVu5AjM6ADd5gCcyDbqAW7qAW%2FqAKzlAJ3pAIP%2FAS7NR%2BYBOPgA9ugBrP8A9%2FgBgOQA%2FeYB%2F%2F8A9ekA9cZH8QCmTGlzdOgAougDgHPmAJxsaXPnAIP4AibEPTzlCALGKOYJBcQWUXVlcnlQYXJhbWV0ZXJz5QPlPFN0YW5kYXJk0yEgJiBTZWxlY88YPgogID7kBPI%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fdata-plane%22%5D%7D%7D). 
```
@service(#{ title: "Contoso Widget Manager" })
@server(
  "{endpoint}/widget",
  "Contoso Widget APIs",
  {
    /** 
Supported Widget Services endpoints (protocol and hostname, for example:
https://westus.api.widget.contoso.com).
 */
    endpoint: string,
  }
)
@versioned(Contoso.WidgetManager.Versions)
namespace Contoso.WidgetManager;

/** The Contoso Widget Manager service version. */
enum Versions {
  /** Version 2022-08-31 */
  @useDependency(Azure.Core.Versions.v1_0_Preview_2)
  `2022-08-30`,
}

// Models ////////////////////

/** The color of a widget. */
union WidgetColor {
  string,

  /** Black Widget Color */
  Black: "Black",

  /** White Widget Color */
  White: "White",

  /** Red Widget Color */
  Red: "Red",

  /** Green Widget Color */
  Green: "Green",

  /** Blue Widget Color */
  Blue: "Blue",
}

/** A widget. */
@resource("widgets")
model Widget {
  /** The widget name. */
  @key("widgetName")
  @visibility(Lifecycle.Read)
  name: string;

  /** The widget color. */
  color: WidgetColor;

  /** The ID of the widget's manufacturer. */
  manufacturerId: string;

  ...EtagProperty;
}

// Operations ////////////////////

alias ServiceTraits = SupportsRepeatableRequests &
  SupportsConditionalRequests &
  SupportsClientRequestId;

alias Operations = Azure.Core.ResourceOperations<ServiceTraits>;

@resource("operations")
model WidgetDeleteOperationStatus is Foundations.OperationStatus<never>;
interface Widgets {
  // Operation Status
  /** Gets status of a Widget operation. */
  @sharedRoute
  getWidgetOperationStatus is Operations.GetResourceOperationStatus<Widget>;
  /** Gets status of a Widget delete operation. */
  @sharedRoute
  getWidgetDeleteOperationStatus is Operations.ResourceRead<WidgetDeleteOperationStatus>;

  // Widget Operations
  /** Creates or updates a Widget asynchronously */
  @pollingOperation(Widgets.getWidgetOperationStatus)
  createOrUpdateWidget is Operations.LongRunningResourceCreateOrUpdate<Widget>;

  /** Get a Widget */
  getWidget is Operations.ResourceRead<Widget>;

  /** Delete a Widget asynchronously. */
  @pollingOperation(Widgets.getWidgetDeleteOperationStatus)
  deleteWidget is Operations.LongRunningResourceDelete<Widget>;

  /** List Widget resources */
  listWidgets is Operations.ResourceList<
    Widget,
    ListQueryParametersTrait<StandardListQueryParameters & SelectQueryParameter>
  >;
}
``` 
 
Any lro over a resource that might change or remove the resource id should not use the resource itself in the StatusMonitor endpoint.
 
It would be nice if both our samples reflected this and our templates made this easier.  If you'd like to file an issue for this, please do so here: https://github.com/Azure/typespec-azure/issues (or I will add this to our existing issue for updating StatusMonitor support in Azure Core)

# How to change the model to accommodate new decorators for validations

## question 
My initial model was:
```
model StorageDiscoveryWorkspace
  is Azure.ResourceManager.TrackedResource<StorageDiscoveryWorkspaceProperties> {
  ...ResourceNameParameter<
    Resource = StorageDiscoveryWorkspace,
    KeyName = "storageDiscoveryWorkspaceName",
    SegmentName = "storageDiscoveryWorkspaces",
    NamePattern = "^[a-z0-9]{3,18}$"
  >;
}
```
I want to add different name patterns and min and max length decorators to it. Can I do this instead to replace this with:
```
/**
 * Legacy name type for StorageDiscoveryWorkspace in older API versions (2024-12-01 through 2025-04-01)
 */
@pattern("^[a-z0-9]{3,18}$")
scalar StorageDiscoveryWorkspaceNameLegacy extends string;

/**
 * A Storage Discovery Workspace resource. This resource configures the collection of storage account metrics.
 */
@doc("A Storage Discovery Workspace resource. This resource configures the collection of storage account metrics.")
model StorageDiscoveryWorkspace
  is Azure.ResourceManager.TrackedResource<StorageDiscoveryWorkspaceProperties> {
  
  @doc("The name of the StorageDiscoveryWorkspace")
  @key("storageDiscoveryWorkspaceName")
  @segment("storageDiscoveryWorkspaces")
  @path
  @visibility(Lifecycle.Read)
  // Starting from v2025_06_01_preview, use enhanced validation
  @typeChangedFrom(ApiVersion.v2025_06_01_preview, StorageDiscoveryWorkspaceNameLegacy)
  @pattern("^[a-zA-Z][a-zA-Z0-9]*(-[a-zA-Z0-9]+)*$")
  @minLength(4)
  @maxLength(64)
  name: string;
}
```

## answer
Yes, the service enforces the same validation rules across all API versions. Although this change technically modifies the validation (such as updating the regex and length constraints), the service behavior is consistent across versions. Therefore, it's appropriate to apply the updated validation pattern universally, rather than scoping it to a specific API version.

This change is not breaking for SDKs since there is no client-side validation involved. SDKs will continue functioning as expected. The purpose of this update is to make the spec more accurately reflect actual service behavior.

Regarding your changes in the PR: the update in models.tsp looks good. The modification in workspace.tsp—particularly for scopes under StorageDiscoveryWorkspace—is applying the same validation, which is fine. However, since the validation is enforced uniformly by the service, there's no need to treat this as version-specific. Also, the legacy type and the @typeChangedFrom annotations are unnecessary and can be removed.

These aren't TypeSpec rules specifically; it's more about accurately modeling how the service behaves. TypeSpec just makes such changes more visible compared to raw Swagger diffs.

# Best way to model a DaysOfWeek Enum/Union?

## question 
Should I use an enum/closed union or an open union for modelling this? Obviously we never expect to add future values. 
 
Example
```
union DayOfWeek {
  @doc("Monday")
  Monday: "Monday",

  @doc("Tuesday")
  Tuesday: "Tuesday",

  @doc("Wednesday")
  Wednesday: "Wednesday",

  @doc("Thursday")
  Thursday: "Thursday",

  @doc("Friday")
  Friday: "Friday",

  @doc("Saturday")
  Saturday: "Saturday",

  @doc("Sunday")
  Sunday: "Sunday",
}
```

## answer
I believe that would be a case where the closed union/enum would get approved.
Side note use doc comments /** */ as a preferred way to do documentation

# Is there way to change property from required to optional?

## Question
Hi, dear, I am working releasing video translation GA API with new version.
And we want to change a property from required to optional in the new version.
 
Previously we have a required property: sourceLocale: localeName
(Here localeName is a defined as: scalar localeName extends string;)
And now we want to change the property to optional: sourceLocale?: localeName
 
Is there anyway I can do to make the change for the new version?
 
I have tried this but doesn't work:
@doc("Translation input.")
model TranslationInput {
  @typeChangedFrom(ApiVersions.v2024_05_20_preview, localeName)
  sourceLocale?: localeName;
}
 
Could anyone help please?

## Answer
use the @madeOptional decorator like this, but it will be considered a breaking change.
```
@doc("Translation input.")
model TranslationInput {
  @typeChangedFrom(ApiVersions.v2024_05_20_preview, localeName)
  @madeOptional
  sourceLocale?: localeName;
}
```

# Discriminators/polymorphism

## Question
I'm looking into introducing polymorphism into one of our APIs. This question provides great insight but is apparently closed as a duplicate of an issue that does not seem exist. Mark Cowlishaw, do you know what the outcome of this no longer existent issue? 
 Note that there is an issue around how we should encourage types using extends in Azure APIs to use discriminators here: https://github.com/Azure/typespec-azure-pr/issues/3510
issue content:
```
title: Add linter to guide to use composition instead of extension when the base type is not used in service

content:
When a base type is not used in the service ( which mean it is not used in any operation or model property), it does not need to define the model hierarchy via extends, it is better to define in composition way via spread or is
e.g.

model A {
  id: string;
}

model B extends A{
  name: string;
}

@route("/get")
@get
@convenientAPI(true)
op get(@body input: B): void;

The Model A is not used in the service, the purpose of model A is to construct model B, in that way, the best way is to define B in composition way via is or spread as following:

model A {
  id: string;
}

model B{
  ...A;
  name: string;
}

@route("/get")
@get
@convenientAPI(true)
op get(@body input: B): void;

or

model A {
  id: string;
}

model B is A{
  name: string;
}

@route("/get")
@get
@convenientAPI(true)
op get(@body input: B): void;

Add a warning linter rule to reject extends and guide to use composition (spread or is).
```
Like Brian Terlson,  I tend to favor the union approach for operations:
 op create(@body body: Cat | Dog // replace the base class with union of subs)
Is this the recommended Azure approach, union (i.e. @body: SubModel1 | SubModel2 for operation with @discriminator in base model type for @body ?

## Answer
Azure recommends using @discriminator to model polymorphism, but there are still some missing pieces before discriminated union types will be allowed in Azure APIs. The polymorphism in the model looks correct, and it is suggested to use PascalCase for union variant names. Additionally, narrowing inherited property types in inheriting classes (except for the discriminator) is not allowed.

# Annotate same model with SubscriptionLocationResource and ResourceGroupLocationResource

## question 
1. We have added a new SubscriptionLocationResource named "ValidatedSolutionRecipe", as per the typespec docs to our RP - Microsoft.AzureStackHCI. Here is the typespec for this resource
2. This is a proxy resource and the URL path for this resource looks like "/subscriptions/921d26b3-c14d-4efc-b56e-93a2439e028c/providers/Microsoft.AzureStackHCI/locations/eastus/validatedSolutionRecipes/10.2502.0?api-version=2023-12-01-preview"
As above API is a subscription level API, the clients of the API need to have subscription level RBAC. Due to security requirements, we need to have a similar API, but at resource group scope.
1. From the typespec docs and our prototyping, we see that we can achieve this by havning a ResourceGroupLocationResource.
2. However, same model in typespec can't be annotated with both SubscriptionLocationResource and ResourceGroupLocationResource. When we do so, the generated swagger only has either subscription level paths or resourcegroup level paths, depending on which annotation is first on the model.
3. Thus, to work around this, we had to introduce a new resource type with an undesirable name - "ResourceGroupValidatedSolutionRecipe".
But it is the same resource. Just because of the limitation of not being able to support both [SubscriptionLocationResource](https://azure.github.io/typespec-azure/docs/libraries/azure-resource-manager/reference/data-types/#Azure.ResourceManager.SubscriptionLocationResource) and [ResourceGroupLocationResource](https://azure.github.io/typespec-azure/docs/libraries/azure-resource-manager/reference/data-types/#Azure.ResourceManager.ResourceGroupLocationResource), we have to create a new model with an undesirable name - "ResourceGroupValidatedSolutionRecipe".
 
Please help us and let us know how can we utilize the model with same name (i.e. the same resource type) for both of the above APIs.

## answer
he main issue is that TypeSpec does not support annotating the same model with both SubscriptionLocationResource and ResourceGroupLocationResource. This leads to the creation of two different models (e.g., ValidatedSolutionRecipe and ResourceGroupValidatedSolutionRecipe), even though they represent the same resource. According to ARM requirements, these resources must be registered as two different types because they have different scopes and operations.

Solution:
Shared Model: If the operations for both resources are identical, you can consider using custom operations to share the same model, avoiding the need to create two different resource types.

RPaaS Proxy Resources: By using RPaaS extensions, you can handle this scenario and simplify the API, reducing confusion for customers.

In summary, although the resources may represent the same entity, due to ARM registration requirements and operational differences, two models may be necessary. However, using custom operations or extension resource patterns can help avoid redundant model creation in certain cases.

# Is path case sensitive?

## Question

I have these swaggers paths:
 
```
"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ElasticSan/elasticSans/{elasticSanName}/volumeGroups"
"/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.ElasticSan/elasticSans/{elasticSanName}/volumegroups/{volumeGroupName}"
```
The second path represents a resource like
```
model VolumeGroup
  is Azure.ResourceManager.ProxyResource<VolumeGroupProperties> {
  ...ResourceNameParameter<
    Resource = VolumeGroup,
    KeyName = "volumeGroupName",
    SegmentName = "volumegroups",
    NamePattern = "^[A-Za-z0-9]+((-|_)[a-z0-9A-Z]+)*$"
  >;
 ```
Pay attention to segment is volumegroups. 
 
The first path is a list operation to this resource. However, its last segment is volumeGroups. If I use ArmResourceListByParent<VolumeGroup> for the first path it produces volumegroups. Can I use it?

## Answer
static segments in ARM urls are meant to be case-insensitive.  In this case, the swagger is incorrect, since this is clearly meant to be the ARM type name.  You should use the correct type name in both cases.

If the url is /subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/..., is that the same as `/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/...? The "G" in resourceGroups has different cases.
These are case insensitive,  we should favor the camel case here, there is no need to match the exact casing of these in existing swagger.

# Discrepancy in the original LRO response and Status Monitor Response

## question 
Hi TypeSpec Discussion team,
 
We have created a long running resource action on our dataplane resource and have created a common status monitor endpoint (`operations/{operationId}`). Now the default status monitor response for the LRO comes out to be :
```
{
    "id": "",
    "status": ""
    "error": ""
}
```
However, following standard resource conventions for the status monitor, the LRO for it comes out to be : 
```
{
    "operationId": "",
    "status": "",
    "kind": ""
    "error": ""
}
```
Given this, how can I change the response of our Long Running Action defined? 
 
Here is Repro Link: https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3JlIjsKCnVzaW5nIEh0dHA7xwxSZXN0yAxWyVfIEkHEPi5Db3Jl0hIuVHJhaXRzOwoKQHVzZUF1dGgoCiAgQXBpS2V5xA48xgtMb2NhdGlvbi5oZWFkZXIsICJhcGkta2V5Ij4gfCBPxCoyxS9bCiAgICB7xQYgIOQAwTrHH0Zsb3dUeXBlLmltcGxpY2l0LMclYXV0aG9yaXrFYVVybDogIuQBTXM6Ly9sb2dpbi5jb250b3NvLmNvbS9jb21tb24vb8Q1Mi92Mi4wL8hAZSLIUnNjb3BlczogW8lJd2lkZ2V0zUouZGVmYXVsdCJdxjd9CiAgXT4KKQpAc2VydmljZSgjeyB0aXRsZTogIkPGOSBXxUggTWFuYWdlciIgfccvZXLkATcie2VuZHBvaW50fcdy5QCT0D5BUElzxRnmAScvKiogClN1cOQCAWVkyCVT5gCFcyDIU3MgKHByb3RvY29sIGFuZCBob3N0bmFtZSwgZm9yIGV4YW1wbGU6CukA8mVzdHVzLmFwaS7yAP0pLgogKi%2FFfshfOiBzdHJpbmfkAJh95AEG5wKfZWQo5wC%2FLuYAmucA%2FC7nAmhzKQrkAIZzcGFjZSDVKjsKCuQA41RoySDvAUcg5wFxIMdzLuQAnGVudW3oAsxz5QEqxEXHESAyMDIyLTA4LTMxxCwgIOQCvURlcGVuZGVuY3ko6wLgyEMudjFfMF9QcmV2aWV3XzIpCiAgYMlGMGAsCn0KCi8vIE1vZGVscyAv0wHqAMdjb2xvciBvZiBhIOcBXOQAhnVu5ACa5gDbQ8Ui5AC26AFW5wDBQmxhY2vHJSDGJuUAwcUYOiAixQgiyS9XaGl0ZdMvxRg6ICLFCMovUuoCQMstUmVkOiAiUmVkyilHcmVl6ACszCvFGDogIsUIyi9CbHX0AIbEF%2BUAtHXkAITkAUEqKiBB7AEUQHJlc291cmNlKCLGFnMiKQpt5AFkyE%2FoAdrkAVjGIyDkAlDFPiAgQGtlecg7TmFtZSLkAbhAdmlzaWJpbGl0eShMaWZlY3ljbGUuUmVhZMQexD7oAs076ADBy17lAb3HX8UMOuwBtM0ySUTkAeV0yTwncyBtYW51ZmFjdHVyZchFzBNJZM17Li4uRXRhZ1Byb3BlcnR5O%2BQBJEBkb2MoIk9wZXLlBPEgSWQgUGF0aCBQYXJhbWV0ZXIu6QEjySVJZMQjySLlATXGSOQAp3VuaXF17ACuxG7FOMRL5gFF%2FwE3IMo07ADD6QCvS2luZMhebG9uZyBydW7kBmPOa%2BYDAukAtsU17wME8AD%2BcmVwcmVzZeQE32EgY2xvbukBqMla5QDEQ8Qb5gGo5AWPyg7mAnnGV1N0YXR1c%2BYDmfoApusClMkY6gKX6QCbx03uAWtzdGF0ZfcBZ8QcdXM6IEZvdW5kxlYuzU5l5QIUyk1r6wFVz0zEGzruAUrLPUVycuQEgGJqZWN0IHRoYXQgZGVzY3JpYmVzxU1lxSB3aGVu5wCVIGlzIFwiRmFpbGVkXCLGY8UlP%2B4Ar8VZ6AK6%2BAJ%2B5QIX5gEt5QGx5gGIUmVxdWVz5gPQbmV3xhVJZD%2FtAk0vL%2BoA1PgFZWFsaWFz6Ab%2F5gieID0g5wcgc1JlcGVhdGFibGXHcXMgJgogyR9Db25kacReYWzWIGxpZW7oAKxJ5AFUxm%2FrAJY97AkeUucCK8ogPO0Amj47CgppbnRlcmbkBv9McstI5AEJZ2V08AJQaXMgU3RhbmRhcmTSXclw5AOWPM87PuUBQ8pu5gFjxGnoBdVldCBhxxfmBJtnZXTHD2nsAOTOZ8Yi5ADKxkflA13pBtvGSUBwb2xsaW5nyUQo7ADsLvIA6OUEeGZpbmFsyjPGacUuxgvFJWFjxR0i5QJM5QKa6wJW%2FwEvTG9uZ1LmA%2BTIHkHFT%2BcA1SzzAqIsIG5ldmVy5ADwfQo%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fdata-plane%22%5D%7D%7D
```
@useAuth(
  ApiKeyAuth<ApiKeyLocation.header, "api-key"> | OAuth2Auth<[
    {
      type: OAuth2FlowType.implicit,
      authorizationUrl: "https://login.contoso.com/common/oauth2/v2.0/authorize",
      scopes: ["https://widget.contoso.com/.default"],
    }
  ]>
)
@service(#{ title: "Contoso Widget Manager" })
@server(
  "{endpoint}/widget",
  "Contoso Widget APIs",
  {
    /** 
Supported Widget Services endpoints (protocol and hostname, for example:
https://westus.api.widget.contoso.com).
 */
    endpoint: string,
  }
)
@versioned(Contoso.WidgetManager.Versions)
namespace Contoso.WidgetManager;

/** The Contoso Widget Manager service version. */
enum Versions {
  /** Version 2022-08-31 */
  @useDependency(Azure.Core.Versions.v1_0_Preview_2)
  `2022-08-30`,
}

// Models ////////////////////

/** The color of a widget. */
union WidgetColor {
  string,

  /** Black Widget Color */
  Black: "Black",

  /** White Widget Color */
  White: "White",

  /** Red Widget Color */
  Red: "Red",

  /** Green Widget Color */
  Green: "Green",

  /** Blue Widget Color */
  Blue: "Blue",
}

/** A widget. */
@resource("widgets")
model Widget {
  /** The widget name. */
  @key("widgetName")
  @visibility(Lifecycle.Read)
  name: string;

  /** The widget color. */
  color: WidgetColor;

  /** The ID of the widget's manufacturer. */
  manufacturerId: string;

  ...EtagProperty;
}

@doc("Operation Id Path Parameter.")
model OperationIdPathParameter {
  @doc("The unique ID of the operation.")
  @key
  @visibility(Lifecycle.Read)
  operationId: string;
}

@doc("Kind of the long running operation.")
union OperationKind {
  string,

  @doc("Operation represents a clone widget operation")
  CloneWidget: "CloneWidget",
}

@doc("Status of a long running operation.")
@resource("operations")
model OperationStatus {
  @doc("The state of the operation.")
  status: Foundations.OperationState;

  @doc("The kind of the operation.")
  kind: OperationKind;

  @doc("Error object that describes the error when status is \"Failed\".")
  error?: Foundations.Error;

  ...OperationIdPathParameter;
}

model cloneWidgetRequest {
  newWidgetId?: string;
}

// Operations ////////////////////

alias ServiceTraits = SupportsRepeatableRequests &
  SupportsConditionalRequests &
  SupportsClientRequestId;

alias Operations = Azure.Core.ResourceOperations<ServiceTraits>;

interface LrOperations {
  getOperationStatus is StandardResourceOperations.ResourceRead<OperationStatus>;
}

interface Widgets {

  /** Get a Widget */
  getWidget is Operations.ResourceRead<Widget>;

  /** Clone a widget */
  @pollingOperation(LrOperations.getOperationStatus)
  @finalOperation(Widgets.getWidget)
  @action("clone")
  cloneWidget is StandardResourceOperations.LongRunningResourceAction<Widget, cloneWidgetRequest, never>;

}
```

## answer
A notice on Guideline on LRO, the "id" of the status monitor should be "id", not "operationId"
https://github.com/microsoft/api-guidelines/blob/vNext/azure/ConsiderationsForServiceDesign.md#long-running-action-operations
To ensure its called {operationId} in the route template but id in the response object, I think you can use this
```
  @key("operationId")
  @visibility(Lifecycle.Read)
  id: string;
```

# Question regarding the unexpected readonly, and customize the enum name

## question 
Hi team,
 
When dealing with the TypeSpec migration, we hit below two issues. Could you help take a look and see if there is any way to fix them? Thanks!
1. Haven't add @visibility(Lifecycle.Read) to the property, but the definition has "readOnly": true on it.
- TSP: https://github.com/Azure/azure-rest-api-specs/blob/45317772ce7c50313eaf55b8d242f4d12ca6fe06/specification/desktopvirtualization/DesktopVirtualization.Management/models.tsp#L3665
`updateState?: UpdateState;`
Swagger: https://github.com/Azure/azure-rest-api-specs/blob/45317772ce7c50313eaf55b8d242f4d12ca6fe06/specification/desktopvirtualization/resource-manager/Microsoft.DesktopVirtualization/preview/2025-04-01-preview/desktopvirtualization.json#L11380
`"readOnly": true`
- TSP: https://github.com/Azure/azure-rest-api-specs/blob/45317772ce7c50313eaf55b8d242f4d12ca6fe06/specification/desktopvirtualization/DesktopVirtualization.Management/models.tsp#L4859
`status: SessionHostManagementUpdateOperationStatus;`
Swagger: https://github.com/Azure/azure-rest-api-specs/blob/45317772ce7c50313eaf55b8d242f4d12ca6fe06/specification/desktopvirtualization/resource-manager/Microsoft.DesktopVirtualization/preview/2025-04-01-preview/desktopvirtualization.json#L10826
`"readOnly": true`
2. How to make enum's "x-ms-enum" name to be different than the type name like below?
- https://github.com/Azure/azure-rest-api-specs/blob/cb262725d128f6dfec4622cca03bc9e04e2d0f1f/specification/desktopvirtualization/resource-manager/Microsoft.DesktopVirtualization/preview/2024-11-01-preview/desktopvirtualization.json#L9487C4-L9493C33
```
 "ScalingMethodType": {
      "enum": [
        "PowerManage",
        "CreateDeletePowerManage"
      ],
      "x-ms-enum": {
        "name": "ScalingMethod",
```

## answer
1. This is because of this https://azure.github.io/typespec-azure/docs/troubleshoot/status-read-only-error/#_top
2. No its not possible change the enum name to be what is in `x-ms-enum.name` it is pointless information otherwise

# Sharing models between data plane and control plane

## question 
Has anyone successfully shared models between control plane and data plane? I'm struggling with this seemingly simple task and could use come guidance or an example other than the trivial one for sharing a TSP file withing control plan or within data plane for a single service.
 
Even to share models across separate versioned data plane APIs, I ended up creating a new data plane shared namespace and associated version to get it working. Do I need to create a control plane shared namespace and version? This makes me a bit nervous about conflicting versions of the same dependency between the service namespace, data plane shared namespace and control plane shared namespace.
 
When I try to cross data plane and control plane TypeSpec, I end up with unhelpful errors like this:
```
<unknown location>:1:1 - error @typespec/versioning/using-versioned-library: Namespace '' is referencing types from versioned namespace 'Azure.Core' but didn't specify which versions with @useDependency.
<unknown location>:1:1 - error @typespec/versioning/using-versioned-library: Namespace '' is referencing types from versioned namespace 'Azure.ResourceManager' but didn't specify which versions with @useDependency.
```
Or errors about multiple namespace or about @service not specifying a namespace even though it does.

## answer
Yes, you **can share models** between data plane and control plane, but you need to be careful:

* ✅ **Create a shared namespace** (e.g., `Discovery.Shared`) that’s not tied to either control or data plane.
* ✅ **Version the shared types independently**, not tied to any API version.
* ✅ **Avoid ARM library dependencies in data plane** — model things like resource IDs using aliases or common types.
* ✅ Use `@useDependency` to reference versioned namespaces like `Azure.Core` or `Azure.ResourceManager`.

This way, you can safely reuse types without introducing versioning conflicts or unwanted dependencies.

# JSON merge-patch support in TypeSpec

## question 
How exactly does TypeSpec support `application/merge-patch+json` i.e., "JSON merge-patch"? Is there explicit types, or is it really just a matter of service authors adding `| null` to their type defs e.g.,

```
model M {
    @key("id")
    id: string;
    name: string | null;
    dob?: utcDateTime | null;
}
```
This will greatly affect how Rust will support this, give a discussion Larry, Johan, and I were having yesterday.

## answer
Currently, TypeSpec does **not** have built-in or explicit support for `application/merge-patch+json` (JSON Merge Patch). That means there are no dedicated keywords or types in the language to model it directly.

The current recommended approach is to **manually define a separate PATCH model** for each resource. This model should mirror the resource structure, but all properties should be made optional. This expresses the correct merge-patch behavior where only specified fields are updated.

Importantly, we **do not use `| null` in the model to indicate erasable fields**. Just like in Azure and Graph, we treat merge-patch support as a **fundamental protocol decision of the service**, not something that should be reflected in the type system. In other words, the ability to pass `null` is not encoded in the TypeSpec models today.

That said, we recognize that for generation-first languages like Rust, it's essential to know whether a field can explicitly be `null` versus being omitted. To address this, we're currently designing a new **`MergePatch` template**, which will help service authors more easily generate accurate merge-patch OpenAPI schemas. It will also make it possible for emitters to trace a merge-patch model back to its original resource definition.

# How to customize "original-uri" in arm template?

## question 
We have a customer who sets ["final-state-via": "original-uri"](https://github.com/Azure/azure-rest-api-specs/blob/11059b2f00c7572b276dc9862c0b41db8702cc78/specification/dashboard/resource-manager/Microsoft.Dashboard/stable/2024-10-01/grafana.json#L1007). 
```
"x-ms-long-running-operation-options": {
          "final-state-via": "original-uri"
        },
```
We have ArmAsyncOperationHeader, ArmLroLocationHeader. Seems we don't have a header for original-uri?

## answer
`original-uri` is only a valid setting for PUT operations, it means that the original URI in the PUT request is used to retrieve the resource, which has a status field that determines its state. Generally, this is the default for LRO PUT operations, but if there are multiple valid pathways to resolve the lro, you can use `@useFinalStateVia` to choose which one should be favored.
[like this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwFEOYGNkZpbmFs5QOGVmlhKCJvcmlnxBItdXJp5QRy5wN7zktDxRVPclJlcGxhY2VBc3luY85b5QMTyDdDdXN0b21QYXRjaFPEKgogICDpAK8sxQ72AQNGb3Vu5ANC5AZhyBzlA1RlTeQBoMZJ0EvKEOoFxsUZPgogIOUAneYCtu8A1OUCs2VXaXRob3V0T2vzANRsaXN0QnnIMEdyb3Vwz0RMxSJQYXJlbnTUPFN1YnNjcmlw5QKVxjvGM8wZzDnoBnwgc2FtcGzrA4JhY8VEdGhhdOYCf%2BkGP3RvIGRpZmZl5ACE7wMCxSnuALJBxUjlAZbIdyzsA43IDeYDBPMAkkhFQUTqBmjEfmNoZWNr6gCqZXhpc3RlbuYH4iDGHkXJFO8CWM0d7ge9&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%2C%22options%22%3A%7B%22%40azure-tools%2Ftypespec-autorest%22%3A%7B%22emit-lro-options%22%3A%22final-state-only%22%7D%7D%7D).  
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  @useFinalStateVia("original-uri")
  create is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
When you have multiple valid pathways to resolve the lro, you might have to use `@useFinalStateVia` to prefer one over the other.  Since this API also has an Azure-AsyncOperation header, and for ARM, looking at it the default for PUT is Azure-AsyncOperation, when it is present.

# Resource Action LRO response modelling

## question 
I want to define an Asyn resource action. Calling this action doesn't produce a  body in the immediate response, but the  long running operation will have a body when it finally reaches a `Succeeded` state. What's the correct way to model this in typespec? This is management plane.

## answer
The way this is currently modeled in swagger (and required by lintdiff rules) is that you have a 200 response that represents the eventual operation return value when the operation is resolved, like the 'move' operation [in this example](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwFECDnAntPcuUCp%2BUB1ssvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbVBhdGNoU8QqCiAgIOkAkyzFDvYA50ZvdW7kAybkBkXIHOYAkU3kAYTGSdBLyhDqBarFGT4KICDlAJ3mAprvANTlApdlV2l0aG91dE9r8wDUbGlzdEJ5yDBHcm91cM9ETMUiUGFyZW501DxTdWJzY3JpcOUCecY7xjPMGcw56AZgIHNhbXBs6wNmYWPFRHRoYXTmAmPpBiN0byBkaWZmZeQAhO8C5sUp7gCyQcVI7gDtLOwDcsgN5gLp8wCTSEVBROoGTcR%2FY2hlY2vqAKtleGlzdGVu5gfHIMYeRckU7wJZzR3uB6I%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D).
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionAsync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}

```
 
We would like to move to a representation where the 200 response is not in the swagger and the return value is represented in the long-running-operation-options extension, but not all emitters for required languages support this yet.

# Augmented decorators on resource in the multi-path scenario

## question 
This is a resource [model](https://github.com/Azure/azure-rest-api-specs/blob/3a8d2effa54913b5f5365e9a4610810825366409/specification/notificationhubs/Notificationhubs.Management/SharedAccessAuthorizationRuleResource.tsp#L18) 
```
@parentResource(NotificationHubResource)
model SharedAccessAuthorizationRuleResource
  is Azure.ResourceManager.TrackedResource<SharedAccessAuthorizationRuleProperties> {
  ...ResourceNameParameter<
    Resource = SharedAccessAuthorizationRuleResource,
    KeyName = "authorizationRuleName",
    SegmentName = "authorizationRules",
    NamePattern = "^[a-zA-Z0-9!()*-._]+$"
  >;
}
```
in the multi-path scenario. It wants to have [minLength and maxLength](https://github.com/Azure/azure-rest-api-specs/blob/3a8d2effa54913b5f5365e9a4610810825366409/specification/notificationhubs/Notificationhubs.Management/SharedAccessAuthorizationRuleResource.tsp#L179-L180).
```
@@maxLength(SharedAccessAuthorizationRuleResource.name, 256);
@@minLength(SharedAccessAuthorizationRuleResource.name, 1);
```
 Its [parent](https://github.com/Azure/azure-rest-api-specs/blob/3a8d2effa54913b5f5365e9a4610810825366409/specification/notificationhubs/Notificationhubs.Management/NotificationHubResource.tsp#L18) 
```
@parentResource(NamespaceResource)
model NotificationHubResource
  is Azure.ResourceManager.TrackedResource<NotificationHubProperties> {
  ...ResourceNameParameter<
    Resource = NotificationHubResource,
    KeyName = "notificationHubName",
    SegmentName = "notificationHubs",
    NamePattern = "^[a-zA-Z][a-zA-Z0-9-./_]*$"
  >;
```
also has [augmented decorator](https://github.com/Azure/azure-rest-api-specs/blob/3a8d2effa54913b5f5365e9a4610810825366409/specification/notificationhubs/Notificationhubs.Management/NotificationHubResource.tsp#L116-L117) 
```
@@maxLength(NotificationHubResource.name, 265);
@@minLength(NotificationHubResource.name, 1);
```
on it. 
 
Actual: No minLength and maxLength [at generated parameter](https://github.com/Azure/azure-rest-api-specs/blob/3a8d2effa54913b5f5365e9a4610810825366409/specification/notificationhubs/resource-manager/Microsoft.NotificationHubs/preview/2023-10-01-preview/notificationhubs.json#L1286-L1308). 
```
{
            "name": "namespaceName",
            "in": "path",
            "description": "The name of the NamespaceResource",
            "required": true,
            "type": "string",
            "pattern": "^[a-zA-Z][a-zA-Z0-9-]*$"
          },
          {
            "name": "notificationHubName",
            "in": "path",
            "description": "The name of the NotificationHubResource",
            "required": true,
            "type": "string",
            "pattern": "^[a-zA-Z][a-zA-Z0-9-./_]*$"
          },
          {
            "name": "authorizationRuleName",
            "in": "path",
            "description": "The name of the SharedAccessAuthorizationRuleResource",
            "required": true,
            "type": "string",
            "pattern": "^[a-zA-Z0-9!()*-._]+$"
```
Expected: [This](https://github.com/Azure/azure-rest-api-specs/blob/5351ac8e1e6fdf48933bae2cd879434b93b36ac0/specification/notificationhubs/resource-manager/Microsoft.NotificationHubs/preview/2023-10-01-preview/notificationhubs.json#L417-L425) 
```
{
            "$ref": "#/parameters/NamespaceName"
          },
          {
            "$ref": "#/parameters/HubName"
          },
          {
            "$ref": "#/parameters/AuthorizationRuleName"
          },
```
is the original swagger. There is limitations on minLength and maxLength.

## answer
Yes, because the actual path parameters do not come from the resource, you would need to decorate the parameters in the LegacyOperations instantiation.

When you construct the LegacyOperations interface, you pass in the parameters - those passed-in parameters would need to be decorated.
 
I wonder if we shouldn't have a legacy resource template that omits the name parameter, just to avoid confusion, it is literally unused in this context.

If you need to decorate the name parameter, you will need to define them directly, or name the resulting model, so they can be decoratred.  You can decorate a model statement, but not a model expression.

# Additional OKResponse

## question 
Like [this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeQEs%2BgEtCBzYW1wbOsBumFjxDcgdGhhdOYAt%2BkEd3RvIGRpZmZlcuQHL%2B4BOsUpaXMgQeoAgkHFSEFzeW5j6QVkLOwBxiwgT2voATvlBX0%3D&e=%40azure-tools%2Ftypespec-client-generator-core&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D). 
```
import "@typespec/http";
import "@typespec/rest";
import "@typespec/versioning";
import "@azure-tools/typespec-azure-core";
import "@azure-tools/typespec-azure-resource-manager";

using Http;
using Rest;
using Versioning;
using Azure.Core;
using Azure.ResourceManager;

/** Contoso Resource Provider management API. */
@armProviderNamespace
@service(#{ title: "ContosoProviderHubClient" })
@versioned(Versions)
namespace Microsoft.ContosoProviderHub;

/** Contoso API versions */
enum Versions {
  /** 2021-10-01-preview version */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @armCommonTypesVersion(Azure.ResourceManager.CommonTypes.Versions.v5)
  `2021-10-01-preview`,
}

/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionAsync<Employee, MoveRequest, OkResponse>;
}
```
My expected response is
```
"200": {
 "description": "ignore"
},
"202": {
 "description": "ignore"
},
"default": {
  "description": "ignore",
    "schema": {
    "$ref": "../../../../../common-types/resource-management/v5/types.json#/definitions/ErrorResponse"
  }
}
```
And it is lro. Therefore I wrote this TypeSpec
```
move is ArmResourceActionAsync<Employee, MoveRequest, OkResponse>;
```
However, this will emit a useless model `TypeSpec.Http.OkResponse` which causes problems in downstream. Can we remove this model? (omit-unreachable-types: true does not work)

## answer
If the `OkResponse` model is being emitted but not actually referenced, that does sound like a bug — we shouldn't be generating unused models. Especially in cases like this where you're doing a long-running POST action and the 200 response is effectively empty, it’s more accurate to model the operation with just a `202` and an error response.

For your case, you can avoid using `OkResponse` entirely by switching to `ArmResourceActionNoResponseContentAsync` or passing `never` as the third argument. That will prevent the generation of the unnecessary `OkResponse` model.

Also, keep in mind that for LROs, the POST operation typically doesn't return 200 anyway. That response is just a placeholder in the old pattern to signal "void". If you're converting from existing Swagger, and it doesn’t actually return 200, you should be able to safely drop that response from the Swagger and avoid the breaking change.

Alternatively, if you do need to customize the final response for the LRO, `getLroResponse` can be used to override the response returned at the end of the operation — no need to rely on `OkResponse` at all.

# Exclude property from list that is in create/update/get

## question 
We have an ARM resource that has a property , e.g.  properties.blob that will contain a very large amount of text not appropriate for list responses. 
 
I see we can override the properties for ArmResourceCreateOrReplaceAsync and ArmResourcePatchAsync, but I don't see a way to do this for ArmResourceRead or ArmResourceListByParent.
 
Is there a way to exclude a property from properties when listing other than defining a new resource type that lacks the specific property?
 
The property is required. The only way I can see to do this would be to make it appear optional but return an error is the user tried to set it to empty or null.

## answer
First, has this been through ARM Review?  Having a very large piece of data as part of a resource could present issues.
 
To answer the question, you can change the response type for a list operation using the `Response` parameter, [as in this playground](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIc29tZSBsb25nIHRleHTGQ8QPVGV4dNJ2VGhlIHN0YXR1c8R5dGhlIGxhc3Qg5ADzYXRpb27lAyogIEB2aXNpYmlsaXR5KExpZmVjeWNs5AIOYWTnAIvEIOUDuVN0YXTkAJXlAdnMFOkBdv8B%2FPABgExpc3T8AgDEIP8CBP8CBP8CBNFk%2FwII%2FwII%2FwII%2FwII%2FwII6wII%2FwHa%2FwHa%2FwHazRTlAUzyATFSZXN1bOUBu%2BgBjMoW7QG%2BPucFCOQAssxx5QC%2F5QEJYekCIOYFumxyb8R6dXMKdW7kBPnxAJPlAaPmAVks7AERyEcgY3JlxCdyZXF1ZXN0IGhhcyBiZWVuIGFjY2VwdGVkxGcgIEHHDjogIsgLItZQacRA5AC06QDByETsAJw6ICLMD9pMdXBkYXTET8VDVccOOiAiyAvKO%2BkG7OkAxOYA3GTnAeFTdWNjZWVk5QDFyQzTP8U25AFNZmFpbMk%2BRsUNOiAixgncOHdhcyBjYW5jZco%2BQ8cP5AdExwv%2FAUAgZGVsZekBgETEDeYA%2BcgLIukGROkDum1vduoByukDvE1vdmVSxxXoA7HEc21vdsRoZnJvbSBsb2PmALzFbsQT9QUhxzN0b88xdG%2FKL%2FcAlXNwb25z6wTW5gCWxxbsAJfuA6vFPsVkxnzmAu7NbmludGVyZuQIY0%2FoA9NzIGV4dGVuZHP2CSUuyyl7fQrlCQHII8oby1noAM3mBPJnZeUDzUHKNeQEJuwFUyDnAntPcuUCp%2BUB1ssvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbVBhdGNoU8QqCiAgIOkAkyzFDvYA50ZvdW7kAybkCIzIHOYAkU3kAYTGSdBLyhDqBenFGT4KICDlAJ3mAprvANTlApdlV2l0aG91dE9r8wDUI3N1cHByZXNz%2Fwst7wstL2FybcoV6QW2xBPlAj0i5AfXaXN0QnnoAItHcm91cO8An%2BQFX0J5UGFyZW509AFB6QKLPfMFu%2BgA%2Bf8Awv8Awv8AwuQAoFN1YnNjcmlwxB%2FnAMHmALnMGf8Av%2FgAv%2BgHqyBzYW1wbOsEcmFjxW90aGF05gNv6QdudG8gZGlmZmXkATXvA%2FLFKe4BY0HFSOUCoukAnewEfcgN5gCq8wCSSEVBROoHl8R%2BY2hlY2vqAKpleGlzdGVu5gkdIMYeRckU7wNkzR3uCPA%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D)
```
@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  #suppress "@azure-tools/typespec-azure-resource-manager/arm-resource-operation-response"
  listByResourceGroup is ArmResourceListByParent<
    Employee,
    Response = EmployeeListResult
  >;
  #suppress "@azure-tools/typespec-azure-resource-manager/arm-resource-operation-response"
  listBySubscription is ArmListBySubscription<
    Employee,
    Response = EmployeeListResult
  >;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```

# Readonly on model

## question 
We have such swagger
```
"ReadonlyOnModel": {
  readonly: true,
  properties: {}
},
"AnotherModel": {
  properties: {
    "a": {
      $ref: "ReadonlyOnModel"
    }
  }
}
```
When the type of property "a" in another model is that "ReadonlyOnModel", M4 will give a readonly on that property "a". I want to confirm if the equivalent TypeSpec should be
```
model AnotherModel {
  @visibility(Lifecycle.Read)
  a: ReadonlyOnModel;
}
model ReadonlyOnModel {}
```
cc Alitzel Mendez : Common type replacement relates to this. I remember we have several models in the original common type have readonly on them and you removed these because TypeSpec cannot represent readonly on model. Then if any service refers to this model, those properties might need to be updated.

## answer
TypeSpec can only attach readOnly to properties, not to models or scalars.  Functionally (assuming all references to ReadOnlyModel are through readOnly properties),  these Swagger docs are equivalent, see: [autorest/docs/openapi/howto/$ref-siblings.md at main · Azure/autorest](https://github.com/Azure/autorest/blob/main/docs/openapi/howto/%24ref-siblings.md) from the perspective of both TypeSpec and autorest, these descriptions are equivalent.

# Setting default value for a union type for only some API versions

## question 
Hi team,
Is it possible to set default value for a union type for only some API versions?
 
For below union type, I want to set default value as ApprovalStatus.Pending from API version 2024-12-01-preview onwards. Is this possible?

```
@doc("Approval Status Enum")
union ApprovalStatus {
  @doc("ApprovalStatus Type Approved")
  Approved: "Approved",

  @doc("ApprovalStatus Type Rejected")
  Rejected: "Rejected",

  @doc("ApprovalStatus Type Pending")
  Pending: "Pending",

  @added(Microsoft.Mission.Versions.v2024_11_01_preview)
  @doc("ApprovalStatus Type Deleted")
  Deleted: "Deleted",

  @added(Microsoft.Mission.Versions.v2024_11_01_preview)
  @doc("ApprovalStatus Type Expired")
  Expired: "Expired",

  string,
}
```

## answer
From a purely "what is the TypeSpec syntax for assigning a default value to a type", no, I don't think we have any way of doing this. You can create a reusable property that you can spread into various places if that is what you are trying to accomplish.
 
From a service API design perspective, what actually changed between the API versions? The pending status was already there in older API versions. Presumably the property that had a type of ApprovalStatus always created things in a pending state (it looks like this is a type that would be used in a persisted model/resource). Wasn't that always conceptually pending? 
I think that shouldn't a problem on the breaking change part. 

# Update API definition in typespec-providerhub

## question 
Hi team, I'm using [providerhub template](https://armwiki.azurewebsites.net/rpaas/gettingstarted.html#bootstrap-your-development-with-typespec-formerly-cadl) to generate a new RP, when trying to add custom API(simple health check for testing) in main.tsp, after build it doesn't generate the new model and controllers that I added, does the template have restrictions on what kind of APIs can be added? Here's what I tried to add into typespec.

```
// Add the health check operation
@doc("Health check endpoint to verify the service is running.")
model HealthCheckResponse {
  message: string;
}

interface HealthCheck {
  @get
  @route("/api/healthcheck")
  @doc("Returns a simple message indicating the service is running.")
  healthCheck(): HealthCheckResponse;
}
```

## answer
Yes, the emitter is specifically about generating RPaaS extensions, not about generating APIs.  You should be able to generate the model, however from your spec.
 
The emitter only updates a specific set of folders, so you can write your own controllers for any APIs outside of generated extensions, and just be sure not to place them in the folder with generated artifacts.

# Seeking Guidance on Defining ResourceStatusCode in TypeSpec

## question 
Hello 
TypeSpec Discussion
I am working on defining a `ResourceStatusCode` in TypeSpec, which is similar to HTTP status codes but specific to resource states. I would appreciate your guidance on the following:
1. Should I use an `enum` or a `int` to define the `ResourceStatusCode`?
2. What are the best practices for defining status codes in TypeSpec?
3. How can I ensure that the `ResourceStatusCode` remains extensible for future updates?
I want to add statuses like:
- `NotSpecified: 204 No Content` - This indicates that the request was successful, but there is no content to return.
- `Pending: 102 Processing` - This indicates that the server has received and is processing the request, but no response is available yet.
- `Running: 202 Accepted` - This indicates that the request has been accepted for processing, but the processing has not been completed.
- `Succeeded: 200 OK` - This indicates that the request has succeeded.
- `Failed: 500 Internal Server Error` - This indicates that the server encountered an unexpected condition that prevented it from fulfilling the request.

Thank you for your assistance.

## answer
As per one of our previous understanding, we defined similarly as an open union: [azure-rest-api-specs-pr/specification/impact/Impact.Management/connectors.tsp at RPSaaSMaster · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/blob/RPSaaSMaster/specification/impact/Impact.Management/connectors.tsp#L88-L94).  
```
@doc("Enum for connector types")
union Platform {
  string,

  @doc("Type of Azure Monitor")
  AzureMonitor: "AzureMonitor",
}
```
This explicitly allows any string value.
 
Generally, the reason for doing this is that you think additional values will be enabled in future versions (or even in this version).  Note that, if you do not make this an open union, then adding any values in any future api-version would be a breaking change (which is why this is recommended).
 
There are RPaaS extensions for validation that would allow you to reject requests for values that are not valid.

# Multiple layers of inheritance for discriminative model

## question 
Like this
```
@discriminator("discountType")
model DiscountTypeProperties {
  discountType: string;
}

model DiscountTypeCustomPrice extends DiscountTypeProperties {
  discountType: "CustomPrice"
}

model DiscountTypeCustomPriceMultiCurrency extends DiscountTypeCustomPrice {
  discountType: "CustomPriceMultiCurrency";
}
```
DiscountTypeCustomPriceMultiCurrency is extending DiscountTypeCustomPrice, but these two have different discriminator values. How could I represent it?

## answer
this is not supported in TypeSpec with the inheritance based discriminator, I think we talked about that in the past and that was an anti pattern.
You could use discriminated union to represent that instead but I don' think they will be supported in the same way in emitters for now

# Extend ResourceModelWithAllowedPropertySet

## question 
My customer has this resource definition:
```
"Discount": {
      "type": "object",
      "x-ms-azure-resource": true,
      "description": "Resource definition for Discounts.",
      "allOf": [
        {
          "$ref": "../../../../../common-types/resource-management/v6/types.json#/definitions/ResourceModelWithAllowedPropertySet"
        }
      ],
      "properties": {
        "properties": {
          "description": "Discount properties",
          "x-ms-client-flatten": true,
          "$ref": "#/definitions/DiscountProperties"
        }
      }
    }
```
I tried this TypeSpec
```
@Azure.ResourceManager.Private.armResourceInternal(DiscountProperties)
@TypeSpec.Http.Private.includeInapplicableMetadataInPayload(false)
model Discount extends Azure.ResourceManager.CommonTypes.ResourceModelWithAllowedPropertySet {
  ...ResourceNameParameter<
    Resource = Discount,
    KeyName = "discountName",
    SegmentName = "discounts",
    NamePattern = "^[a-zA-Z0-9_\\-\\.]+$"
  >;

  @doc("The resource-specific properties for this resource.")
  @Azure.ResourceManager.Private.conditionalClientFlatten
  properties: DiscountProperties;
}
```
Error message is: @azure-tools/typespec-azure-resource-manager/arm-resource-invalid-base-type: The @armResourceInternal decorator can only be used on a type that ultimately extends TrackedResource, ProxyResource, or ExtensionResource.
 
I don't quite understand this error, since ResourceModelWithAllowedPropertySet does extend TrackedResource. How could I represent that swagger in TypeSpec?

## answer
We should not use ResourceModelWithAllowedPropertySet.  Instead, we should spread in the appropriate properties using a tracked resource.
 
The ResourceModelWithAllowedPropertySet is meant as an example, not as something resources should use, and so far usage in the specs repo has been incredibly light.  We should not be afraid of this kind of break to make the resulting spec more accurate and easier to evolve over time.

# Non-resource long running operation

## question 
[This](https://github.com/Azure/azure-rest-api-specs/blob/4e8d16d3793228046ac6171eadda4b8d26ad2b4f/specification/botservice/resource-manager/Microsoft.BotService/preview/2023-09-15-preview/botservice.json#L1235) is a long running operation, which is not a resource operation. 
```
    "/subscriptions/{subscriptionId}/providers/Microsoft.BotService/operationresults/{operationResultId}": {
      "get": {
        "tags": [
          "OperationResults"
        ],
        "description": "Get the operation result for a long running operation.",
        "operationId": "OperationResults_Get",
        "x-ms-examples": {
          "Get operation result": {
            "$ref": "./examples/OperationResultsGet.json"
          }
        },
        "parameters": [
          {
            "$ref": "#/parameters/apiVersionParameter"
          },
          {
            "$ref": "#/parameters/subscriptionIdParameter"
          },
          {
            "$ref": "#/parameters/operationResultIdParameter"
          }
        ],
        "responses": {
          "200": {
            "description": "The body contains all of the properties of the operation result.",
            "schema": {
              "$ref": "#/definitions/OperationResultsDescription"
            }
          },
          "202": {
            "description": "Accepted - Get request accepted; the operation will complete asynchronously."
          },
          "default": {
            "description": "Default error response",
            "x-ms-error-response": true,
            "schema": {
              "$ref": "#/definitions/Error"
            }
          }
        },
        "x-ms-long-running-operation": true
      }
    },
```
[This](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKdhA5AGB1zUyxTVhcm1Db21tb25U5AIExyrXfcspy1Q1xEhg8gDeYCwKfeYBKEHoASrrAUMg6AJl5AD9bW9kZWwgRW1wbG95ZWUgaXMgVHJhY2tlZOgAgjzIHFByb3BlcnRpZXM%2B5QFZLi7pAKbkAgtQYXJhbWV0ZXLJMT476ACGyV9wyUTSfMpg6QG4QWdlIG9mIGXIP%2BUBrWFnZT86IGludDMyOwrHKUNpdHnSKmNpdHk%2FOiBzdHLlAx7HLFByb2ZpbNNZQGVuY29kZSgiYmFzZTY0dXJsIuQBYHDGMD86IGJ5dGVzyUhUaGUgc3RhdHVzxEt0aGUgbGFzdCDkAMVhdGlvbuUDMSAgQHZpc2liaWxpdHkoTGlmZWN5Y2zkAeBhZMddxCDlA8BTdGF0xGflAavMFOkBSMRzzDLlAIDlAMph6QHVxXdAbHJvxDt1cwp1buQC59FU5QFk5gEaLOwA0shHIGNyZcQncmVxdWVzdCBoYXMgYmVlbiBhY2NlcHRlZMRnICBBxw46ICLICyLWUGnEQOQAtOkAwchE7ACcOiAizA%2FaTHVwZGF0xE%2FFQ1XHDjogIsgLyjvpBNrpAMTmANxk5wGiU3VjY2VlZOUAxckM0z%2FFNuQBTWZhaWzJPkbFDTogIsYJ3Dh3YXMgY2FuY2XKPkPHD%2BQFMscL%2FwFAIGRlbGXpAYBExA3mAPnICyLpA%2F3pA3dtb3bqAcrpA3lNb3ZlUscV6ANyxHNtb3bEaGZyb20gbG9j5gC8xW7EE%2FEDUMszdG%2FPMXRvyi%2F3AJVzcG9uc%2BsEi%2BYAlscW7ACX7gNsxT7FZMZ85gLuzW5pbnRlcmbkBlFP6AOUcyBleHRlbmRz9gcTLsspe30K5QbvyCPKG8tZ6ADN5gSzZ2V05AGnQco15APn7AUQIOcCe09y5QKn5QHWyy9Dxx1SZXBsYWNlQXN5bmPOP%2BUC98g3Q3VzdG9tUGF0Y2jGKwogICDpAJQsxQ72AOhGb3Vu5AMn5AZGyBzmAJJN5AGFyXQs8wWePsZZTHJvSGVhZGVycyA95ACN5QCC6QElxhs8RmluYWxSZXN1bHQgPclMPiAmxUPoAJ7lByruAJN0cnlBZnRlcsZICiAg5QD%2B5gL77wE15QL4ZVdpdGhvdXRPa%2FMBNWxpc3RCecgwR3JvdXDPREzFIlBhcmVudNQ8U3Vic2NyaXDlAtrGO8YzzBnMOegGwSBzYW1wbOsDx2FjxUR0aGF05gLE6QaEdG8gZGlmZmXkAITvA0fFKe4AskHFSFPsAOws7APSyA3mA0nzAJJIRUFE6gatxH5jaGVja%2BoAqmV4aXN0ZW7mCCcgxh5FyRTvArnNHe8IAuwDilTmBG3kBxhyb3V0ZSgiL3PrAUNzL3vMD0lkfS%2FlBi%2FkAmcv6goXQm90U%2BYKcy%2FpAMJy5QJmcy97yRLmAnhJZH3lB8dAZ2V05gOkKOUCfS4uLkFwaecJTOkIveYC3C4uLuwB2UlkyyDFIS8qKsUIIOYEm0lE6ASXyX3kAUjkAvZ0b8RzLscu5AFJICBAcGF0aMUK8QCs6ATB5ACIKTrnAWPlAcM87QHQIHzEHOgHc0xyb8kn8AOYTHJvTOcCTfUDlcRX5Antx1jkA5r%2FA5boA5bkAIVFcnJvcsg%2B5QHu&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%2C%22options%22%3A%7B%22%40azure-tools%2Ftypespec-autorest%22%3A%7B%22omit-unreachable-types%22%3Afalse%2C%22emit-common-types-schema%22%3A%22never%22%7D%7D%7D) is what I wrote in TypeSpec.

```
@route("/subscriptions/{subscriptionId}/providers/Microsoft.BotService/operationresults/{operationResultId}")
  @get
  get(
    ...ApiVersionParameter,
    ...SubscriptionIdParameter,

    /**
     * The ID of the operation result to get.
     */
    @path
    operationResultId: string,
  ): ArmResponse<MoveResponse> | ArmAcceptedLroResponse<LroHeaders = ArmLroLocationHeader<FinalResult = MovedResponse> &
  Azure.Core.Foundations.RetryAfterHeader> | ErrorResponse;
```
It's still not LRO. How could I represent this operation in TypeSpec?

## answer
long-running GET is not allowed in ARM, or in Azure at all.  We should not support any such operation, this is undoubtedly a mistake, if it appears in any spec.
It would be allowed to do a non-resource POST operation, which you might model [like this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCn3mAPNB6AD16wEOIOgCMOQAyG1vZGVsIEVtcGxveWVlIGlzIFRyYWNrZWToAII8yBxQcm9wZXJ0aWVzPuUBJC4u6QCm5AHWUGFyYW1ldGVyyTE%2BO%2BgAhslfcMlE0nzKYOkBg0FnZSBvZiBlyD%2FlAXhhZ2U%2FOiBpbnQzMjsKxylDaXR50ipjaXR5Pzogc3Ry5QLpxyxQcm9maWzTWUBlbmNvZGUoImJhc2U2NHVybCLkAWBwxjA%2FOiBieXRlc8lIVGhlIHN0YXR1c8RLdGhlIGxhc3Qg5ADFYXRpb27lAvwgIEB2aXNpYmlsaXR5KExpZmVjeWNs5AHgYWTHXcQg5QOLU3RhdMRn5QGrzBTpAUjEc8wy5QCA5QDKYekB1cV3QGxyb8Q7dXMKdW7kArLRVOUBZOYBGizsANLIRyBjcmXEJ3JlcXVlc3QgaGFzIGJlZW4gYWNjZXB0ZWTEZyAgQccOOiAiyAsi1lBpxEDkALTpAMHIROwAnDogIswP2kx1cGRhdMRPxUNVxw46ICLIC8o76QSl6QDE5gDcZOcBolN1Y2NlZWTlAMXJDNM%2FxTbkAU1mYWlsyT5GxQ06ICLGCdw4d2FzIGNhbmNlyj5Dxw%2FkBP3HC%2F8BQCBkZWxl6QGARMQN5gD5yAsi6QP96QN3bW926gHK6QN5TW92ZVLHFegDcsRzbW92xGhmcm9tIGxvY%2BYAvMVuxBPxA1DLM3RvzzF0b8ov9wCVc3BvbnPrBIvmAJbHFuwAl%2B4DbMU%2BxWTGfOYC7s1uaW50ZXJm5AYcT%2BgDlHMgZXh0ZW5kc%2FYG3i7LKXt9CuUGusgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2BwFECDnAntPcuUCp%2BUB1ssvQ8cdUmVwbGFjZUFzeW5jzj%2FlAvfIN0N1c3RvbVBhdGNoU8QqCiAgIOkAkyzFDvYA50ZvdW7kAybkBkXIHOYAkU3kAYTGSdBLyhDqBarFGT4KICDlAJ3mAprvANTlApdlV2l0aG91dE9r8wDUbGlzdEJ5yDBHcm91cM9ETMUiUGFyZW501DxTdWJzY3JpcOUCecY7xjPMGcw56AZgIHNhbXBs6wNmYWPFRHRoYXTmAmPpBiN0byBkaWZmZeQAhO8C5sUp7gCyQcVI5QGWyHcs7ANxyA3mAujqAJJsb25nLXJ1buUF3uQCUOQC9mlzdGlj5Qcy5QJk5ALrxxPGdegH3MZ15gFi5QGadm9pZOYBveQCn8QBLi4uT2vId%2BQBQSAgfcYi7AErxklTY29wZeYB3PABMEhFQUTqBurkARxjaGVja%2BoBSGV4aXN0ZW7mCGQgxh5FyRTvAvbNHe4IPw%3D%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%2C%22options%22%3A%7B%22%40azure-tools%2Ftypespec-autorest%22%3A%7B%22emit-lro-options%22%3A%22all%22%7D%7D%7D)
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  age?: int32;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmCustomPatchSync<
    Employee,
    Azure.ResourceManager.Foundations.ResourceUpdateModel<
      Employee,
      EmployeeProperties
    >
  >;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** long-running get statistics */
  getStatistics is ArmProviderActionAsync<
    void,
    {
      ...OkResponse;
    },
    SubscriptionActionScope
  >;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
But, in this case, this looks like they are modeling a subscription-level operationResult resource, which should just be modeled as a resource operation get that is not long-running.

# Extending 'Azure.ResourceManager.CommonTypes.ProxyResource' that doesn't define a discriminator.

## question 
Like this. I got warning
```
Model 'Employee' is extending 'Azure.ResourceManager.CommonTypes.ProxyResource' that doesn't define a discriminator. If 'Azure.ResourceManager.CommonTypes.ProxyResource' is meant to be used: - For composition consider using spread `...` or `model is` instead. - As a polymorphic relation, add the `@discriminator` decorator on the base model.
```
Why this gets related to discriminator?

## answer
In general we want to discourage any use of in inheritance if not used with discriminator hence the warning. I assume you have to do this because there is a non standard resource tghat can't do `model is ProxyResource`? If so probably have to suppress that too

# Model validation failures - Newer models introduced in new version adds the parent models in the older version.

## question 
Hi team!
 
I am trying to create a new version with a new models only specific to the latest version. I have also added the @added attribute to it. 
 
Despite this it is adding the models from the parent model - here (Recomendation, MigrationIssues, MigrationSuitability, etc) to all the older versions of the swagger which is not an intended behaviour.
 
Added the model implementation for more context. and adding the PR for a more broader context. 

[WACA changes for assessedWebApps by alphaNewrex · Pull Request #22616 · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/pull/22616/files)

```
@doc("Compound Assessment Recommendations.")
@added(WACAApiVersions.v2025_03_30_preview)
model CompoundAssessmentRecommendations
  is Recommendations<
    MigrationIssues,
    MigrationSuitability,
    Skus<MigrationSuitability>
  > {
  @doc("Arm id of the assessed resource. to get extended details.")
  extendedDetailsAssessedResourceArmId: string;
}
```

## answer
Yes, `@added` does not process a tree of models,  any model you introduce will have to be versioned in the same way (so all of those models would need their own `@added` decorators).

# Override contentType: "application/json" for ResourceCreateOrUpdate

## question
Hi TypeSpec Discussion,
I am migrating an old swagger to typespec. I came across a method which is a PATCH ops with a application/json as content type. The API behaves exactly as a merge-patch route, but I cannot change it since it'll be consider a breaking change. In order to still use the convenient functionalities of typespec traits, I have define a custom function like so [playground](https://azure.github.io/typespec-azure/playground/?c=Ly8gLd8B3wHdAQovLyBDb3B5cmlnaHQgKGMpIE1pY3Jvc29mdCBDb3Jwb3JhdGlvbi4gQWxsIMUlcyByZXNlcnZlZC7EPUxpY2Vuc2VkIHVuZGVyIHRoZSBNSVTIFy4gU2VlyQ10eHQgaW7FJHByb2plY3Qgcm9vdCBmb3IgbMYkIGluZm9ybcZ1xGD%2FAMDfAd4BCgppbXBvcnQgIkB0eXBlc3BlYy9yZXN0IjvTGXZlcnNpb25pbmfVH2h0dHDVGW9wZW5hcGnMHGF6dXJlLXRvb2xzL8goLcYVY29yZSI7Cgp1c2luZyBUeXBlU3BlYy5IdHRwO9AVUmVzdMgVQcQ%2BLkNvcmXSEi5UcmFpdHM7CgpuYW1lc3BhY2XGHjsKCiNzdXBwcmVzc%2F8Al29yZS9uby1wcml2YXRlLXVzYWdlIiAiIgpARm91bmTlAbBzLlDGHy5lbnN1cmVWZXJiKCJSZXNvdXJjZUNyZWF0ZU9yVXBkYXRlIiwgIlBBVENIIikKQGPFG3PIHHPIMyjICSkKQHBhcmFtZXRlclZpc2liaWxpdHkoTGlmZWN5Y2xlLsZdLCDKEsZLxTl0Y2gKb3Ag9gCJV2l0aEpzb25Db250ZW505AFdPAogyS0gZXh0ZW5kc%2BwBdmZsZWPlAqBNb2RlbCwKICDmAWHfLGRlbCA9IHt9xDFJbnRlcmZhY2XfOtI6RXJyb3JSZXNwb%2BQDLj3sAefsAYrNJwo%2BIGlz2CroAOpPcGXmA%2BzsAP%2FEcnsKICAgIC8qKsUIICogVGhlIOQCSiBvZu0DzHRvIHVzZS7HJy%2FFCEBkb2MoIlRoaeQEN3F1ZXN0IGhhcyBhIEpTT04gYm9keS4iKcYq7QLtLmhlYWRlcigi5wGYLcQexypj6gGrOiAiYXBwbGlj5QC6L2pzb27kAz7EJi4u9QDnQm9keTzIDT47yCvxAy7pAszFD1Byb3BlcnRpZXPkAR7EAcYmICbwAcvkATLJIExv5gCZLlDoAr7OIOUA3Xh05wK%2FIHzOFuYCocUw5QCp5QIA%2FwHGxkpkT3JPa%2BgB%2FukA7iAmxUz%2FAOv%2FAOv%2FAOtu5ACKxXf%2FAOn4AOk%2B6wNQ1XLvAto7Cg%3D%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D) and am using it.
```
// --------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// --------------------------------------------------------------------------------------------

import "@typespec/rest";
import "@typespec/versioning";
import "@typespec/http";
import "@typespec/openapi";
import "@azure-tools/typespec-azure-core";

using TypeSpec.Http;
using TypeSpec.Rest;
using Azure.Core;
using Azure.Core.Traits;

namespace Azure;

#suppress "@azure-tools/typespec-azure-core/no-private-usage" ""
@Foundations.Private.ensureVerb("ResourceCreateOrUpdate", "PATCH")
@createsOrUpdatesResource(Resource)
@parameterVisibility(Lifecycle.Create, Lifecycle.Update)
@patch
op ResourceCreateOrUpdateWithJsonContentType<
  Resource extends TypeSpec.Reflection.Model,
  Traits extends TypeSpec.Reflection.Model = {},
  InterfaceTraits extends TypeSpec.Reflection.Model = {},
  ErrorResponse = Azure.Core.Foundations.ErrorResponse
> is Azure.Core.Foundations.ResourceOperation<
  Resource,
  {
    /**
     * The name of the project to use.
     */
    @doc("This request has a JSON body.")
    @TypeSpec.Http.header("Content-Type")
    contentType: "application/json";

    ...Foundations.ResourceBody<Resource>;
    ...Azure.Core.Traits.Private.TraitProperties<
      Traits & InterfaceTraits,
      TraitLocation.Parameters,
      TraitContext.Create | TraitContext.Update
    >;
  },
  Azure.Core.Foundations.ResourceCreatedOrOkResponse<Resource &
    Azure.Core.Traits.Private.TraitProperties<
      Traits & InterfaceTraits,
      TraitLocation.Response,
      TraitContext.Create | TraitContext.Update
    >>,
  Traits & InterfaceTraits,
  ErrorResponse
>;

```
Is there a better alternative?

## answer
Yes. it's expected that if you are using a non-standard content-type for PATCH you will need to use a custom operation or a custom operation template.

# Customizing key for child operation

## question 
Is there a way to customize the key of a parent resource for a specific child operation?
 
In SDKs we are being asked to change the name, but I couldn't find a good place to apply the `@clientName` decorator.

## answer   
In the data plane API, resource keys are typically derived from the parent resource’s key and are not individually specified for each operation. By default, the key names for operations are inherited from the parent resource's key. The @clientName decorator is used to modify the name used by the client, not to change the parent resource's key in operations. To change the parent resource's key name globally, the simplest solution is to use the @key decorator on the name property to ensure consistency.

Changing the key name specifically for certain operations is not straightforward because operation keys are typically tied to the parent resource key. Although you can explicitly define different keys for an operation, this generally requires redefining the operation.

In your case, the most reasonable approach would be to update the parent resource's key name to jobName across all operations to maintain consistency rather than using a different name for some operations. This approach reduces naming conflicts and ensures consistency.

For how to explicitly specify key definitions, you can refer to the Azure Data Plane documentation, which outlines how to define keys for each service's operations and resources.

# Default value starting from a specific API version?

## question 
Hello, we currently have a property (StatelessServiceProperties,minInstancePercentage) that does not currently have a default value in our spec, but in practice is treated as if it is 0.
[azure-rest-api-specs/specification/servicefabricmanagedclusters/resource-manager/Microsoft.ServiceFabric/preview/2025-03-01-preview/servicefabricmanagedclusters.json at main · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/blob/main/specification/servicefabricmanagedclusters/resource-manager/Microsoft.ServiceFabric/preview/2025-03-01-preview/servicefabricmanagedclusters.json)
```
"StatelessServiceProperties": {
      "type": "object",
      "description": "The properties of a stateless service resource.",
      "properties": {
        "instanceCount": {
          "type": "integer",
          "format": "int32",
          "description": "The instance count.",
          "minimum": -1
        },
        "minInstanceCount": {
          "type": "integer",
          "format": "int32",
          "description": "MinInstanceCount is the minimum number of instances that must be up to meet the EnsureAvailability safety check during operations like upgrade or deactivate node. The actual number that is used is max( MinInstanceCount, ceil( MinInstancePercentage/100.0 * InstanceCount) ). Note, if InstanceCount is set to -1, during MinInstanceCount computation -1 is first converted into the number of nodes on which the instances are allowed to be placed according to the placement constraints on the service."
        },
        "minInstancePercentage": {
          "type": "integer",
          "format": "int32",
          "description": "MinInstancePercentage is the minimum percentage of InstanceCount that must be up to meet the EnsureAvailability safety check during operations like upgrade or deactivate node. The actual number that is used is max( MinInstanceCount, ceil( MinInstancePercentage/100.0 * InstanceCount) ). Note, if InstanceCount is set to -1, during MinInstancePercentage computation, -1 is first converted into the number of nodes on which the instances are allowed to be placed according to the placement constraints on the service."
        }
      },
      "required": [
        "instanceCount"
      ],
      "allOf": [
        {
          "$ref": "#/definitions/ServiceResourceProperties"
        }
      ],
      "x-ms-discriminator-value": "Stateless"
    },
```
We would like to treat the default prior to 2025-06-01 as 0, then as a different value from 2025-06-01 onward in our service.
1. We haven't changed the default for an existing property before. Are there concerns about this intended behavior? 
2. In Typespec, is it possible to add a default value in our spec from a specific api version (say 2025-06-01) onward?

## answer
I think you need to run this change by the breaking change board, as a change in the default may be breaking, depending on the details.  Also, the most important thing is to make sure that the API description accurately reflects service behavior - if the default has always been in place, for example, it may be better to just change the default and go through the breaking change process.  Yes, it is possible to do this in TypeSpec, but involves removing and renaming the old property and adding a new property with the new default, [like this](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjQtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCu0AxTH%2FAMX%2FAMX%2FAMX%2FAMX%2FAMXtAMXsAKntAMU1LTAzLTAx%2FwC9%2FwC9%2FwC9%2FwC9%2FgC95wChYCwKfeYCbUHoAm%2FrAogg6AOq5ADAbW9kZWwgRW1wbG95ZWUgaXMgVHJhY2tlZMh6PMgcUHJvcGVydGllcz7lAp4uLukAnuQDUFBhcmFtZXRlcskxPjvoAIbJX3DJRNJ8ymDpAv1BZ2Ugb2YgZcg%2F5gFwcmVtb3brA4Au9AG35QFacmXkA5pkRnJvbd4uLCAiYWdlIsQ1Zm9ybWVyQWdlPzogaW50MzI76AIB9gCOYWRk%2FwCM5QCMYcpRID0gMjHJVkNpdHnSV2NpdHk%2FOiBzdHLlBR7HLFByb2ZpbPQAhmVuY29kZSgiYmFzZTY0dXJs5QDMcMYwPzogYnl0ZXPJSFRoZSBzdGF0dXPES3RoZSBsYXN0IOQBgGF0aW9u5QUxICBAdmlzaWJpbGl0eShMaWZlY3ljbOQCk2Fkx13EIOUFwFN0YXTEZ%2BUCZswU6QIDxHPMMuUAgOUAymHpApDFd0Bscm%2FEO3VzCnVu5ANl0VTlAh%2FmARrpA53EX8hHIGNyZcQncmVxdWVzdCBoYXMgYmVlbiBhY2NlcHRlZMRnICBBxw46ICLICyLWUGnEQOQAtOkAwchE7ACcOiAizA%2FaTHVwZGF0xE%2FFQ1XHDjogIsgLyjvpBtrpAMTmANxk5wGiU3VjY2VlZOUAxckM0z%2FFNuQBTWZhaWzJPkbFDTogIsYJ3Dh3YXMgY2FuY2XKPkPHD%2BQHMscL%2FwFAIGRlbGXpAYBExA3mAPnICyLpBLjpBDLkA%2BvpAcrpBDRNb3ZlUscV6AQtxHNtb3bEaGZyb20gbG9j5gC8xW7EE%2FEDUMszdG%2FPMXRvyi%2F3AJVzcG9uc%2BsFRuYAlscW7ACX7gNsxT7FZMZ85gLuzW5pbnRlcmbkCFFP6AOUcyBleHRlbmRz9gkTLsspe30K5QjvyCPKG8tZ6ADN5gVuZ2V05AGnQco15APn7AXLIOcCe09y5QKn5QHWyy9Dxx1SZXBsYWNlQXN5bmPOP%2BUC99A3UGF0Y2hTzCws8wYRxUDmAj3PQOUCOmVXaXRob3V0T2vTd2xpc3RCecgwR3JvdXDPREzFIlBhcmVudNQ8U3Vic2NyaXDlAhzGO8YzzBnMOegGMCBzYW1wbOsDCWFjxUR0aGF05gIG6QXGdG8gZGlmZmXkAITvAonFKe4AskHFSO8BN%2BsDFMgN5gKL8wCSSEVBROoF78R%2BY2hlY2vqAKpleGlzdGVu5ggkIMYeRckU7wH7zR3uB%2F8%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D)
```
/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
}

/** Employee properties */
model EmployeeProperties {
  /** Age of employee */
  @removed(Versions.`2024-11-01-preview`)
  @renamedFrom(Versions.`2024-11-01-preview`, "age")
  formerAge?: int32;

  /** Age of employee */
  @added(Versions.`2024-11-01-preview`)
  age?: int32 = 21;

  /** City of employee */
  city?: string;

  /** Profile of employee */
  @encode("base64url")
  profile?: bytes;

  /** The status of the last operation. */
  @visibility(Lifecycle.Read)
  provisioningState?: ProvisioningState;
}

/** The provisioning state of a resource. */
@lroStatus
union ProvisioningState {
  string,

  /** The resource create request has been accepted */
  Accepted: "Accepted",

  /** The resource is being provisioned */
  Provisioning: "Provisioning",

  /** The resource is updating */
  Updating: "Updating",

  /** Resource has been created. */
  Succeeded: "Succeeded",

  /** Resource creation failed. */
  Failed: "Failed",

  /** Resource creation was canceled. */
  Canceled: "Canceled",

  /** The resource is being deleted */
  Deleting: "Deleting",
}

/** Employee move request */
model MoveRequest {
  /** The moving from location */
  from: string;

  /** The moving to location */
  to: string;
}

/** Employee move response */
model MoveResponse {
  /** The status of the move */
  movingStatus: string;
}

interface Operations extends Azure.ResourceManager.Operations {}

@armResourceOperations
interface Employees {
  get is ArmResourceRead<Employee>;
  createOrUpdate is ArmResourceCreateOrReplaceAsync<Employee>;
  update is ArmResourcePatchSync<Employee, EmployeeProperties>;
  delete is ArmResourceDeleteWithoutOkAsync<Employee>;
  listByResourceGroup is ArmResourceListByParent<Employee>;
  listBySubscription is ArmListBySubscription<Employee>;

  /** A sample resource action that move employee to different location */
  move is ArmResourceActionSync<Employee, MoveRequest, MoveResponse>;

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```
