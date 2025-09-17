# How to version a spread property (ManagedServiceIdentityProperty)?

## question 
I'm trying to add Managed Identity support and want to avoid a breaking change.
 
However, adding `...Azure.ResourceManager.ManagedServiceIdentityProperty;` would update all my existing API versions and introduce a breaking change.
 
That's the right way to introduce the MSI property so I can only add it in a new version of my model?
 
If instead I'd directly add to my tracked resource `identity?: Foundations.ManagedServiceIdentity;` I get a warning about this not being valid in the resource envelope.

And how without having to create an entirely separate model. What I want is to introduce the property in a new API version `2025-05-04-preview` only.

## answer
You can do it using an extension decorator [like this](https://azure.github.io/typespec-azure/playground/?options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D&c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwoKdXNpbmcgSHR0cDvHDFJlc3TIDFbpAI7IEkHESi5Db3JlzhJSx1xNxls7CgovKiogQ29udG9zb8RUxR4gUHJvdmlkZXIg5gCDbWVudCBBUEkuICovCkBhcm3IIE5hbWVzcGFjZQpAc2VydmljZSgjeyB0aXRsZTogIsdXyC1IdWJDbGllbnQiIH0pCkDnAUNlZCjnAL9zKQpuyFAgTWljcm9zb2Z0LtJG7wC2QVBJIMdNc%2BQAoWVudW3oARNzIHsKICDELjIwMjEtMTAtMDEtcHJldmlld8g1xDQgIEB1c2VEZXBlbmRlbmN5KPUBLy7IVi52MV8wX1DGSF8xKcRAYXJtQ29tbW9uVOQBz8cq10jLKctUNcRIYPIAqWAsCv8Axf8Axf8Axf8Axf8AxfgAxTTxAMV95gG4QegBuusB0yDoAvXkAMhtb2RlbCBFbXBsb3llZSBpcyBUcmFja2Vk6ACCPMgcUHJvcGVydGllcz7lAekuLukApuQCm1BhcmFtZXRlcskxPjvGJuYAxGRT5gK1SWRlbnRpdHnHT3k75ACrQEBhZGRlZCjIOy5pxyYs6QJdLvQA6CnnAp%2FIOCBwxlZpZeUCnu4A3cdyxBzoAqlBZ2Ugb2YgZcg%2F5QHZYWdlPzogaW50MzI76AINQ2l0edIqY2l0eT86IHN0cuUED8csUHJvZmls01lAZW5jb2RlKCJiYXNlNjR1cmwi5AHBcMYwPzogYnl0ZXPJSFRoZSBzdGF0dXPES3RoZSBsYXN0IOQAxWF0aW9u5QQiICBAdmlzaWJpbGl0eShMaWZlY3ljbOQCQWFkx13EIOUEsVN0YXTEZ%2BUCDMwU5QGEyHPMMuUAgOUAymHpAjbFd0Bscm%2FEO3VzCnVu5AMT0VTlAWTmARrpA1PEX8hHIGNyZcQncmVxdWVzdCBoYXMgYmVlbiBhY2NlcHRlZMRnICBBxw46ICLICyLWUGnEQOQAtOkAwchE7ACcOiAizA%2FaTHVwZGF0xE%2FFQ1XHDjogIsgLyjvpBcvpAMTmANxk5wGiU3VjY2VlZOUAxckM0z%2FFNuQBTWZhaWzJPkbFDTogIsYJ3Dh3YXMgY2FuY2XKPkPHD%2BQGI8cL%2FwFAIGRlbGXpAYBExA3mAPnICyLpBF7pA3dtb3bqAcrpA3lNb3ZlUscV6ANyxHNtb3bEaGZyb20gbG9j5gC8xW7EE%2FEDUMszdG%2FPMXRvyi%2F3AJVzcG9uc%2BsE7OYAlscW7ACX7gNsxT7FZMZ85gLuzW5pbnRlcmbkB0JP6AOUcyBleHRlbmRz9ggELsspe%2BQE%2BGFybcgjyhvLWegAzeYEs2dldOQBp0HKNeQD5%2B4FceYCe09y5QKn5QHWyy9Dxx1SZXBsYWNlQXN5bmPOP%2BUC98g3Q3VzdG9tUGF0Y2hTxCoKICAg6QCTLMUO9gDnRm91buQDJuQFuMgc5gCRTeQBhMZJ0EvKEOoFqsUZPgogIOUAneYCmu8A1OUCl2VXaXRob3V0T2vzANRsaXN0QnnIMEdyb3Vwz0RMxSJQYXJlbnTUPFN1YnNjcmlw5QJ5xjvGM8wZzDnoBmAgc2FtcGzrA2ZhY8VEdGhhdOYCY%2BkGI3RvIGRpZmZl5ACE7wLmxSnuALJBxUjlAZbIdyzsA3HIDeYC6PMAkkhFQUTqBkzEfmNoZWNr6gCqZXhpc3RlbuYIJyDGHkXJFO8CWM0d7AEJfQo%3D&e=%40azure-tools%2Ftypespec-autorest&vs=%7B%7D):
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

  /** 2021-10-01-preview version */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @armCommonTypesVersion(Azure.ResourceManager.CommonTypes.Versions.v5)
  `2024-10-01-preview`,
}

/** A ContosoProviderHub resource */
model Employee is TrackedResource<EmployeeProperties> {
  ...ResourceNameParameter<Employee>;
  ...ManagedServiceIdentityProperty;
}

@@added(Employee.identity, Versions.`2024-10-01-preview`);

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

  /** A sample HEAD operation to check resource existence */
  checkExistence is ArmResourceCheckExistence<Employee>;
}
```

# TypeSpec Versioning Generic Question

## question 
Hello, as of now our repository ([azure-rest-api-specs-pr/specification/azuredatatransfer/AzureDataTransfer.Management at RPSaaSDev · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/tree/RPSaaSDev/specification/azuredatatransfer/AzureDataTransfer.Management)) just introduced typespec and previously was using manual updates.
 
Regarding API Version updates, I wanted to know if we can remove previous preview versions (4-11) from our [main.tsp](https://github.com/Azure/azure-rest-api-specs-pr/blob/RPSaaSDev/specification/azuredatatransfer/AzureDataTransfer.Management/main.tsp#L35):
```
/**
 * PLEASE DO NOT REMOVE - USED FOR CONVERTER METRICS
 * Generated by package: @autorest/openapi-to-typespec
 * Parameters used:
 *   isFullCompatible: false
 *   guessResourceKey: false
 * Version: 0.11.0
 * Date: 2025-05-13T20:15:12.229Z
 */
import "@typespec/rest";
import "@typespec/versioning";
import "@azure-tools/typespec-azure-core";
import "@azure-tools/typespec-azure-resource-manager";
import "./models.tsp";
import "./FlowProfile.tsp";
import "./Connection.tsp";
import "./Pipeline.tsp";
import "./Flow.tsp";
import "./ProviderActions.tsp";

using Azure.ResourceManager;
using TypeSpec.Versioning;
/**
 * Azure Data Transfer service resource provider
 */
@armProviderNamespace
@service(#{ title: "azuredatatransferrp" })
@versioned(Versions)
@armCommonTypesVersion(Azure.ResourceManager.CommonTypes.Versions.v5)
namespace Private.AzureDataTransfer;

/**
 * The available API versions.
 */
enum Versions {
  /**
   * The 2025-04-11-preview API version.
   */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @useDependency(Azure.Core.Versions.v1_0_Preview_1)
  v2025_04_11_preview: "2025-04-11-preview",

  /**
   * The 2025-05-21 API version.
   */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @useDependency(Azure.Core.Versions.v1_0_Preview_1)
  v2025_05_21: "2025-05-21",

  /**
   * The 2025-05-30-preview API version.
   */
  @useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @useDependency(Azure.Core.Versions.v1_0_Preview_1)
  v2025_05_30_preview: "2025-05-30-preview",
}

interface Operations extends Azure.ResourceManager.Operations {}
```
or if there is a recommended safe practice.
For the newest version I used the @added/removed decorators to add/remove certain properties/fields from the newest/olders versions. However, I'm not sure I want to always include the @added/removed decorators as we create new versions.
How do we manage this?
Also, does removing from the newest version count as a breaking change? I was under the impression that as long as we support it in prior versions and our backend supports it, we should be find to remove certain fields and properties.

## answer
It is generally recommended that you have only one active preview api, but this has to be balanced with ARM retention requirements for preview APIs.  For APIs that are not public to customers,  I don't see any need for them to remain once your tests no longer use those api versions

# "is referencing versioned type but is not versioned"

## question 
```
'Microsoft.Compute.{ statusCode: 202, location: string, retryAfter: int32, _: Microsoft.Compute.GalleryScriptVersion }._' is referencing versioned type 'Microsoft.Compute.GalleryScriptVersion' but is not versioned itself. TypeSpec(@typespec/versioning/incompatible-versioned-reference)
```
how do I resolve it? 
at the beginning of the file, it is defining GalleryScriptVersion which has added decorator. 
 
Where do I need to add 'added' decorator here? 
```
/**
 * Create or update a gallery Script Version. Script versions help save different states.
 */
#suppress "@azure-tools/typespec-azure-resource-manager/arm-put-operation-response-code"
#suppress "@azure-tools/typespec-azure-resource-manager/no-response-body" "For backward
@added(Versions.v2025_03_03)
createOrUpdate is ComputeResourceCreateOrReplaceAsync<
    GalleryScriptVersion,
    Response = ArmResourceUpdatedResponse<GalleryScriptVersion> | ArmResourceCreatedRespo
        GalleryScriptVersion,
        ArmLroLocationHeader & Azure.Core.Foundations.RetryAfterHeader
    > (ArmAcceptedLroResponse & {
    @bodyRoot
    _: GalleryScriptVersion;
    })
>;
```

## answer
was able to resolve it this way.
```
/**
 * Create or update a gallery Script Version. Script versions help save different states.
 */
#suppress "@azure-tools/typespec-azure-resource-manager/arm-put-operation-response-code"
#suppress "@azure-tools/typespec-azure-resource-manager/no-response-body" "For backward
@added(Versions.v2025_03_03)
createOrUpdate is ComputeResourceCreateOrReplaceAsync<
    GalleryScriptVersion,
    Response = ArmResourceUpdatedResponse<GalleryScriptVersion> | ArmResourceCreatedRespo
        GalleryScriptVersion,
        ArmLroLocationHeader & Azure.Core.Foundations.RetryAfterHeader
    > (ArmAcceptedLroResponse & {
    @bodyRoot
    @added(Versions.v2025_03_03)
    _: GalleryScriptVersion;
    })
>;
```

# relaxing a pattern across versions

## question 
We released a preview version of our API and found that the pattern being enforced on some of the model properties was too restrictive.  We wanted to relax them, but it doesn't seem I can do that with the added/removed decorators unless I change the name of the property.  Is there anything I'm missing or this is how it must be done?
```
@removed(Microsoft.IoTFirmwareDefense.Versions.v2025_06_01)
@doc("Firmware vendor.")
@pattern("^[a-zA-Z0-9][a-zA-Z0-9-_.,'\"~=(){}:]*$")
vendor?: string;

You, 1 second ago · Uncommitted changes

@added(Microsoft.IoTFirmwareDefense.Versions.v2025_06_01)
@doc("Firmware vendor.")
vendor?: string;    Model already has a property named vendor
```

## answer
The first question is:  are you relaxing this pattern across all api-versions in your service, or just the new api-version?
 
Generally speaking, patterns are not used by our client SDKs, and it is important for specs to be accurate to service behavior, so this kind of breaking change in the spec is generally allowed, if it really describes service behavior.
 
It is possible to do this, but it involves a remove/rename/replace pattern,  as in this playground.
 
The reason you have to do this is that in typespec, collections like Namespace or Model or Interface use the name of included types as their identifier, so you need two different names for these types.  In this case, you need two different model properties, and you cna use the renaming trick to remove and rename one and add the other in the version where the change is made, and ensure that the original property still has the correct name in previous versions.
 
However, if your service uses the new pattern across all api-versions, then it is best for your spec to reflect this.

it is generally better for review to isolate a change like this so it's easy to talk about it separately

# Handling multiple API versions using typespec

## question 
Hi Team,
 
I am releasing a new API version and looks there is a new validation to check existing swagger with new tsp file 
 
"appears to contain TypeSpec-generated swagger files, not generated from the current TypeSpec sources. Perhaps you deleted a version from your TypeSpec, but didn't delete the associated swaggers?"
 
We want to keep both old and new api versoins. What is the suggestion here? Do you suggest to create different TSP file for each version/use versioning in existing TSP? 
 
If second case, can you share some example where APIs/Properties are added/removed in a newer version

## answer
The idea behind TypeSpec versioning is to model the diffs in your API, encouraging best practices for API evolution. If you have questions about modeling particular changes, feel free to ask.

About your concern: fixing a new validation causing changes to older APIs—this usually happens when new changes are added without using the appropriate version decorators. For example:

If you added properties or made a property optional, but didn’t annotate them with @added or @madeOptional, then they would apply to all versions by default, including older ones.

That’s likely why your stable version (e.g., 2024-12-01) got unintentionally updated when you only meant to target 2025-07-01-preview.

To avoid this, always use decorators like @added and @madeOptional to indicate version-specific changes.

As for the location field and the read/create mutability warning:
That rule is a warning meant to be advisory. In your case, it doesn’t seem to apply—so this is a good example where you should not change the service behavior just to satisfy a linting rule. It's fine to ignore the warning here.

Regarding your question on renaming an API response (e.g., from MoveResponse to MoveResult):
If the new name is meant for a specific version, ensure it’s versioned properly in your TypeSpec. In your repro, the naming appears correct—MoveResult in 2021-10-01-preview, and MoveResponse in 2024-10-01-preview—so it’s working as expected.

Finally, for adding or removing a parameter in only the new version:
Unfortunately, you can’t directly decorate a spread parameter. The best way is to use operation-specific parameters, which you can decorate with versioning decorators. You could alternatively decorate the core parameter definition (e.g., with an augment decorator), but that would affect every usage—so for localized changes, use operation-specific ones. Since this is a method parameter, the APIs are equivalent and the change has no SDK impact.

# help with @added(multipleversion) vs @removed(oldversion)

## question 
Hi Team,  I am working on new api version and we have couple properties that are added in last api verison (viz., v2 version onwards).
 
we have 3 apiversions v1 the new property is not present.
v2 the property is added.
v3 also it need to be there.
 
so can i add @removed (v1) for this property? 
instead of 
@added (v2) 
@added(V3)
as decorators?
 
here is the actual change i was making :
@added(Versions.v2024_11_30_preview)
@added(Versions.v2025_06_15)
@doc("Number of cache node issues.")
@visibility(Lifecycle.Read)
 issuesCount?: int32;
 
can this be modified as below?
 
@removed(Versions.v2023_05_01_preview)
@doc("Number of cache node issues.")
@visibility(Lifecycle.Read)
  issuesCount?: int32;
 
any feedback in here is greatly appreciated.

## answer
if the version is added in v2 it will be available in all subsequent versions until removed, you shouldn't say you added it in multiple versions
 
the versioning is linear

# Can I have different Name Regex for ARM Resource for different api versions?

## question 
Hi Team,
 
In swagger our model is defined as
```
@doc("Dynamic Configuration Resource")
@parentResource(DynamicConfiguration)
@resource("versions")
model DynamicConfigurationVersion
  is ProxyResource<DynamicConfigurationVersionProperties> {
  ...ResourceNameParameter<
    DynamicConfigurationVersion,
    NamePattern = "^[a-zA-Z0-9.-]{3,24}$"
  >;
}
```
This namePattern is in current `2024-09-01-preview` version but in our new version i want to increase length and update regex from `^[a-zA-Z0-9.-]{3,24}$` to `^[a-zA-Z0-9.-]{3,24}$` in new `2025-06-01` version.
 
Is there any way to achieve this or I need to change this for all api versions?

## answer
does your service actually validate differently across version or you want to update to a new regex
This this issue for existing ask https://github.com/Azure/typespec-azure/issues/2355
issue content:
```
Document changing pattern

@johanste input

"
If bugfix, it goes into all. I suspect that is the most common.

If restricting what was previously valid input, it is a breaking change. If expanding on what is valid output for a previously valid input, it is a breaking change. Breaking changes go through breaking change board. I hope that is the lest common.

If it is a expanding outputs but only if the client sends something that was previously no possible to send (or, rather, the service would have rejected the call), it should be discussed in API review board. There are degrees of breakingness to this. I would make the case in review board to add to all, but I need a couple of concrete examples for the rest of the API review board to fully grok the implications I think. I have not a good picture of how common this is.'
"

Everytime this has come up it was a bug fix and so all specs should be fixed

an in more general document how to workaround the versioining of decorator
```
unless your service actually validates names differently between the versions, the recommendation is to make this change for all versions - client SDKs do not use name patterns, and this is just accurately describing what your service does.

# Need advice on how to change operation parameters on a breaking change

## question 
Hi all.  My team (Microsoft Translator) is trying to update our POST translate call with a breaking change.  The PR is: [Update translate call to 2025-05-01 by SG-MS · Pull Request #33410 · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/pull/33410)
 
The old version (3.0) is being updated to a new version (2025-05-01-preview).  The big change to our typespec definition is that we will be moving most of the request properties from query parameters to the request body.  In 3.0, the only property in the request body was an array of InputTextItem.  InputTextItem is just a string.  All the other relevant data was passed in as query parameters.
 
In 2025-05-01-preview, we want to add a number of additional properties to the request body, so rather than the body consisting of InputTextItem, we want the body to contain a TranslateBody definition, which in turn contains all of our properties.  I'm not sure how to represent this in the tsp files in a compliant way.  
 
Our translate operation in the routes.tsp file is
 
op translate is CustomOperation<
 InputTextItem[],
 TranslateParameters,
 TranslationResult,
 ErrorResponse
\>;
 
The first member of the template is the request body and I need it to be InputTextItem[] for 3.0 and I need it to be TranslateBody for 2025-05-01-preview.  Otherwise one of them will have the wrong set of arguments.  I considered simply updating InputTextItem to contain all the needed parameters in TranslateBody and then removing the unused ones with @added decorators, but the problem is we have a number of other operators in 2025-05-01-preview which use InputTextItem and require it to be just the text string.  
 
Is there anyway to have two different translate operations such that one is called by 3.0 and one is called by 2025-05-01-preview?  I think that's what would really get what I need.  

## answer
the post below might be helpful. It also involves adding a "adding" a newer API on the same route with different method signature. You'd need to add `@sharedRoute` on both methods and point it to the same route.
[Jocelyn Wei: Changing interface's `update` from ArmResourcePatchAsync to ArmCustom... | Azure SDK > TypeSpec Discussion | Microsoft Teams](https://teams.microsoft.com/l/message/19:906c1efbbec54dc8949ac736633e6bdf@thread.skype/1721233288788?tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47&groupId=3e17dcb0-4257-4a30-b843-77f47f1d4121&parentMessageId=1721233288788&teamName=Azure%20SDK&channelName=TypeSpec%20Discussion&createdTime=1721233288788):
```
title: Changing interface's `update` from ArmResourcePatchAsync to ArmCustomPatchAsync without generating breaking changes

question:
Hi TypeSpec Discussion,

I am attempting to follow the playground example here TypeSpec Azure to get past a LintDiff error after adding an MSI to my resource. However, adding the new update model as well as changing the interface from update is ArmResourcePatchAsync<... to update is ArmCustomPatchAsync<... is generating breaking changes for older api versions.

Is there a way to only change the update is.. for the newest api version? I tried using the @added and @removed decorators but the interface complained about duplicate update.

Summary of answer:
To change the update operation from ArmResourcePatchAsync to ArmCustomPatchAsync without generating breaking changes for older API versions:

You also need to use @renamedFrom("update", newVersion) on the old update operation and put @sharedRoute on both.

Make sure you don’t use a model that is not available in a certain version and carry over the @added/@removed if needed.

If a property like userAssignedIdentities was marked as added in 2023, but the model it depends on (ManagedServiceIdentity) was only added later, it doesn’t make sense — this can be the result of composing shared models.

The sharedRoute might be sharing the new property to the old versions and not honoring the @added and @removed decorators.

If you mark the whole model like:

@added(Microsoft.Mission.Versions.v2024_06_01_preview)  
model ManagedServiceIdentityUpdate  

…errors may occur if parts of the model were added in older versions.

One workaround was to use ManagedServiceIdentityProperty instead of Azure.ResourceManager.Foundations.ManagedServiceIdentity

LintDiff errors like AvoidAnonymousTypes seem to be coming from the MSI additionalProperties section. This error wasn’t there before setting use-read-only-status-schema: true, but it’s unclear why that config causes it.

Warnings like "Use the latest version v5 of types.json" for old API versions or EnumInsteadOfBoolean for new ones can be ignored — warnings are not required to change for check-in, only errors.

False positives (like Avocado errors) may also occur when using @removed and @added in the same API version — especially for preview versions.

If everything is tagged properly and using the latest compiler version, you should be able to get it approved. After that, just follow the normal PR process (e.g., sign-offs, label like WaitForARMFeedback).
```
There is some documentation on this here: [Versioning | TypeSpec Azure](https://azure.github.io/typespec-azure/docs/howtos/arm/versioning/#converting-an-operation-from-synchronous-to-asynchronous) (this is for ARM, but the same principle applies)

# Proper Service Versioning

## question 
Hi TypeSpec Discussion. I'm looking to understand how to properly add a new service version to my team's typespec. I've been looking at this doc here as a baseline, and I think I generally understand everything there. But I've got a couple of questions for my specific case.
1. What does the `added` decorator actually do? Just tell the swagger which version should/shouldn't contain a property? I'm assuming the question of whether it has an impact on any of the SDKs is a question for the individual generator teams?
2. My team's spec has a [client.tsp file](https://github.com/Azure/azure-rest-api-specs/blob/dargilco/ai-model-inference/specification/ai/ModelClient/client.tsp#L8) with a `customization` namespace, which has this decorator: `@useDependency(AI.Model.Versions.v2024_05_01_Preview)`. I get the general gist that this ties things to a specific version, but what does that mean from a practical standpoint? I maybe can understand client customizations being specific to individual versions, but what about modifications that work across versions? I don't seem to be able to provide a list or anything to that decorator. I've tried removing it, and I get an error saying that the `customization` namespace is referencing a versioned namespace and should add the decorator. I've also tried just changing the namespace to match, but then I get an error from my client interfaces saying that I have duplicate operations. So I'm trying to understand how to correctly handle this.

Any pointers would be appreciated. Thanks in advance!

## answer
On the first question, TypeSpec is allowing you to version based on differences,  starting with the base api-version, whenever you make changes to the api, you just need to tag those changes with the 'versioning' decorator to ensure that typespec can reconstruct the api at each version that is still active.
In the case of `@added` this is used whenever adding a new type to the spec - a new model, a new model property, a new interface, a new parameter, a new operation - you simply decorate the element with this decorator and pass in the version that this change occurred in.  `@removed` works similarly for removing types (which is always a breaking change).  decorators like `@renamedFrom` allow you to rename types, and take the version the change occurred at and the old name of the type (the type name is changed inline). `@typeChangedFrom` works similarly - describing the state of the type before the change occurred.
There are some limitations around versioning (for example, versioning decorators is impossible, so the decorated types themselves generally need to be versioned.
 
As far as the client.tsp goes, versioning is tightly tied to a namespace (but includes all the child namespaces).  If your client.tsp is a child namespace of the versioned namespace, then no explicit version coupling is required.  If not, then you will need to replicate the versions enum from the service namespace in client.tsp,  and explicitly tie each version to the corresponding version.  I have shown an example of what I mean [in this playground](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvaHR0cCI7CtIZcmVzdNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3JlIjsKCnVzaW5nIEh0dHA7xwxSZXN0yAxWyVfIEkHEPi5Db3Jl0hIuVHJhaXRzOwoKQOcAkGVkKENvbnRvc28uV2lkZ2V0TWFuYWdlci7HWXMpCm5hbWVzcGFjZSDVKiB7CiAgLyoqIFRoySIgxiIgyCNzZXJ2aWNlIMd1LiAqLwogIGVudW3oAMFzxUfGSccTIDIwMjItMDgtMzDGMCAgQHVzZURlcGVuZGVuY3ko6wDZyEcudjFfMF9QcmV2aWV3XzIpxTdgykpgLArUaDUtMDHfaN9oyGjHSmAsCiAgfcRsLy8gTW9kZWxzIC%2FTAcUi5wE9Y29sb3Igb2YgYSB35QFA5wEodW7kAKbmAVNDxSTmASxzdHJpbmfrANFCbGFja8cpIMYqxUEgxho6ICLFCCLLM1doaXRl1TPFGjogIsUIzDNSZWTVMVJlZDogIlJlZMwtR3JlZegAvM4vxRo6ICLFCMwzQmx19gCSxBnlAMR15ACQ6AFfKiogQe4BMEByZXNvdXJjZSgixhhzIuQBoG3kAYbIWeoCbuQBesYnIOQC6sdEICBAa2V5yEFOYW1lxUQgIEB2aXNpYmlsaXR5KExpZmVjeWNsZS5SZWFkxiDERDrnAZs76gDVy2blAefJZ8UOOuwB3s82SUTkAhN0yUAncyBtYW51ZmFjdHVyZcpJzBVJZO8Agy4uLkV0YWdQcm9wZXJ0eTvsAT7EZXJlcGFpciBzdGF0ZfMCgkBscm9TdGF0dXPvAo9SxTjEG2X4ApXGKcdicyBzdWNjZWVkZWTHWyAgU8gSOiAiyQztAqTOQmZhaWzLP0bFDzogIsYJ2zl3ZXJlIGNhbmNlzEBDxxE6ICLIC9NEd2FzIHNlbnQgdG%2FlAYz1AYNTZW50VG9NyxvkANPRFfACvXN1Ym1pdHRlZOgBh3JlcXVlc3QgZm9y8AGK7ALF5gF9Usct7gLS6QHK6gI5z17HUSDIEOUBxegCiesB2O8Cj2TEWGFuZCB0aW1lIHdoZW7FY8dcaXMgc2NoZWR1bGVk5AErb2NjdeoBIMkbRGF0ZVRp5AMpdXRjyA3faclp5gDv5AGRY3JlYXTrAcvHEN9e317JXnVwxCXKXscQ317fXsleY29tcGxlzGDJEtdi7wOzcGFyYW1ldGXkAxLrAijlAfp1c8ls9wI25gPOUMRE7Ad79ARxIGJl5AhG5gHi6wCwQHBhdGjFCsYo7AR18wWdJ3PkALPzBaTEFuYFokBwYXJlbnRSyBvmALHwBbxQYXLwAu7kBXXoALXLZe4FxcQ4%2FwXJ%2FwXJyG9JROQC3HVzZeUBenJlb3JkZcQn8gCAxA3xBY3yAWnEJ%2F8F2P8F2PcF2GRldGFpbHPEaGHoAKnvBF%2FqAVXzBGPEF1LGN%2FIEaElkZW50aWZpZXMgd2hvIHNpZ25lZCBvZuYA1M9ux10gxydPZmZCefICMC8gQW4gZXhh5AMK5gC15ArCbGV0b24g6AIy5wDfUHJvdmlkZXMgYW5hbHl0aWNzIGFib3V0xX3kAZzkA4BtYWludGVu5AXYxk%2FmANfyAoXJRf8CiewBHUHJee4CjmnoARhy5QFj5ACQyV8gb2JqZWN0LiDEKnJl5AUxb25seSBvbuYCxmQgJ2N1csR9J%2B8CxclDSWT%2FAsLoAsJp5AbUx07kDErtAzJ1bWJl5QptdXNl5QIg6gPxyXl1c2VDb3VudDogaW50NjTZQ%2BQE5nPLQeUE4fEEMMYRzlHtBCjsAurEV8ZNc%2FIBrcwp7Anc7Adl7wGUzDMnc%2BQIxHF15AOC7wFlzCf%2FAWjuAWj1A%2BHPde0KX%2FsEX883ZnVsbCBhZGRyZXPoAQwgyBD%2FBA%2FrA1hPcGVyYXTlDWr%2FDIjpDa505A4tZXPsBBZTxhvmDkfnAWMuLi5TdXDkDtFzUmVwZWF0YWJsZecELnPkAIPNI0NvbmRp5ACIYWzaJGxp5QN%2BxR9JZDzpDXXsDWE%2B6QDeYWxpYXPsAOE97A7z6APFyiA87QDGPjsKfQoK5A3yQXV0aCgKICBBcGlLZXnEDjzGC0xvY8U6LmhlYWRlciwgImFwaS1rZXkiPiB8IE%2FEKjLFL1vlAMrmARcgIOQP0TrHH0Zsb3dUeXBlLmltcGxpY2l05AoExAFhdXRob3JpesVhVXJsOiAi5BBdczovL2xvZ2luLmPnD3Vjb20vY29tbW9uL2%2FENTIvdjIuMC%2FIQOYNE8QBc2NvcGVzOiBbyUnnA9fMSi5kZWZhdWx0Il3GN30KICBdPgopCkDnAf4oI3sgdGl0bOQNcvYP1iIgfccvZXLkATcie2VuZHBvaW50fcdy5QCT0D5BUElzxRnqA6EK5wH56g5Z5wGicyDIU3MgKHByb3RvY29s5QXZaG9zdOQDQyzlBWPnBjI6CukA8mVzdHVzLmFwaS7yAP0pLgroA0fIX%2BgDSOYLminsER%2F7EQnrAnPkAM3mEW71ESXlA6DkBotyZsRAxhzGNu4DniDpDWfnD0DEJ%2BcJ0%2BsGwSDkA%2BHFMeoEwXNoYXJlZFJvdXTkByMgIGdldMYtyV3GXOQGdMkTcy5H5AoT7wMjxik8xj%2FlA2n%2BAI5kZeQKuf8Ale4AlUTFM%2F8Am%2FoAm8VNIOcAgOgC%2Bm5ldmVyxQw%2B6AVTL8cjy1fGGSoqIEPlDC9zIG9y5wvcc%2BoA0mFzeW5jaHJvbm91c2x56grzb2xsaW5nyU3nCBpzLvgBbeYGIuYMmU9yVcVnx2PuAOxMb25nUnVu5BPK6AD05gCgyDbqAY7sAY%2FqALLnAKPpAInPYMhV5Aa300vmAY%2F4AQDrC%2FT%2FAQH3AdnmAQfmAif%2FAP%2FpAP%2FGSfMArExpc3TqD9zGLecGpiAgbGlz5wCJ8AJOyFjEP%2FYCQMQaUXVlcnnlDRjlDVzlBiQ8U3RhbmRhcmTTISAmxz3kD%2F9sZWPPID73Ao%2FpCjz1Ac3JHusB1soU%2FwHZyyvJeP4C7tFvxiHfcvYCoNJ86QD05g569AOK5AD85Ao%2F8gRb7RDk6QKHI3N1cHDkCQD%2FF0NvcmUvdXNlLXPnAcAt6QSicyIgIlRoaeUA22EgY3VzdG9t6gTA6ACJ6AYfLiLFd0By5ATIKCLnBt9zL3vGCUlkfS%2FnEmUve8lFSWR95wpB7w92xHRGb3VuZOoE5%2F0E38w85g%2By6AKx8wEk7gJwKiogU%2BcRdeoQIuYOtcQ06gpQ%2FwPZ%2BQTa6BHT5gCE8AIq8wPWQWPEG%2FYDgPMAz9sbICYgxwpJZFJlc3BvbnNlSOUJoegBBOcKIPAHeOQO1ewNtPcG1sQn8wbT6QEQxCD%2FBsD%2FB1vFQ%2FQDpO0Ag%2B0S3csY%2FAOVV2l0aOcJROcPKGTkEPvXd%2BwEe8xz7QDr13DrBHfQV%2B8GVMxa7AYK213tBgPQX%2BsGB8VD%2BwYLxCD9Bg%2FWYucQ%2BiBhbGzlEaPmE2rzDyn4Az7FeOoDQvMCMuYDRucRLPQAp%2FQIGG9s5AZpaW9u9ANNxEXuAzbyEcX1CTHxAy7sD0%2F9AzDMKfYDMswi%2FwM0%2BgM0zEX3BtpyZXBsYeQCA%2B8Ale0DRE9yUsYnzSP%2BBuTHMvkAhOYDQ9R07wD%2B12jlA0fYXekDSc1J%2FwmjyWUozDblAo77AY7sCan7ALf6Ca%2F4AMTlA67NUvUDsMwi%2FQOyz2jvFPj%2FHoj%2FHoj%2FHoj%2FHiD6HiDPN%2F4feeUQ7cd4YP8eyP8eyP8AqP8AqP8AqO0RlfgfCOYV2yAiZ2xvYmFsIiBSUEPqBCnnAbjmB8RkcyB3aXRo6ARgaW5mb3JtxSfrFd9vdmVy5AWV5wHQ6RQi5gmfxxbkCfZ0deYUIG9w5AOI5wcS6gR4UnBj6QJg5gVGe33mBRzoEcTGRFPlEB3sFq8gIMcp7RJMCuUFSX0K&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fdata-plane%22%5D%7D%7D)
```
import "@typespec/http";
import "@typespec/rest";
import "@typespec/versioning";
import "@azure-tools/typespec-azure-core";

using Http;
using Rest;
using Versioning;
using Azure.Core;
using Azure.Core.Traits;

@versioned(Contoso.WidgetManager.Versions)
namespace Contoso.WidgetManager {
  /** The Contoso Widget Manager service version. */
  enum Versions {
    /** Version 2022-08-30 */
    @useDependency(Azure.Core.Versions.v1_0_Preview_2)
    `2022-08-30`,

    /** Version 2025-01-30 */
    @useDependency(Azure.Core.Versions.v1_0_Preview_2)
    `2025-01-30`,
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

  /** The repair state of a widget. */
  @lroStatus
  union WidgetRepairState {
    string,

    /** Widget repairs succeeded. */
    Succeeded: "Succeeded",

    /** Widget repairs failed. */
    Failed: "Failed",

    /** Widget repairs were canceled. */
    Canceled: "Canceled",

    /** Widget was sent to the manufacturer. */
    SentToManufacturer: "SentToManufacturer",
  }

  /** A submitted repair request for a widget. */
  model WidgetRepairRequest {
    /** The state of the widget repair request. */
    requestState: WidgetRepairState;

    /** The date and time when the repair is scheduled to occur. */
    scheduledDateTime: utcDateTime;

    /** The date and time when the request was created. */
    createdDateTime: utcDateTime;

    /** The date and time when the request was updated. */
    updatedDateTime: utcDateTime;

    /** The date and time when the request was completed. */
    completedDateTime: utcDateTime;
  }

  /** The parameters for a widget status request */
  model WidgetRepairStatusParams {
    /** The ID of the widget being repaired. */
    @path
    widgetId: string;
  }

  /** A widget's part. */
  @resource("parts")
  @parentResource(Widget)
  model WidgetPart {
    /** The name of the part. */
    @key("widgetPartName")
    @visibility(Lifecycle.Read)
    name: string;

    /** The ID to use for reordering the part. */
    partId: string;

    /** The ID of the part's manufacturer. */
    manufacturerId: string;

    ...EtagProperty;
  }

  /** The details of a reorder request for a WidgetPart. */
  model WidgetPartReorderRequest {
    /** Identifies who signed off the reorder request. */
    signedOffBy: string;
  }

  // An example of a singleton resource
  /** Provides analytics about the use and maintenance of a Widget. */
  @resource("analytics")
  @parentResource(Widget)
  model WidgetAnalytics {
    /** The identifier for the analytics object.  There is only one named 'current'. */
    @key("analyticsId")
    @visibility(Lifecycle.Read)
    id: "current";

    /** The number of uses of the widget. */
    useCount: int64;

    /** The number of times the widget was repaired. */
    repairCount: int64;
  }

  /** A manufacturer of widgets. */
  @resource("manufacturers")
  model Manufacturer {
    /** The manufacturer's unique ID. */
    @key("manufacturerId")
    @visibility(Lifecycle.Read)
    id: string;

    /** The manufacturer's name. */
    name: string;

    /** The manufacturer's full address. */
    address: string;

    ...EtagProperty;
  }

  // Operations ////////////////////

  /** The service traites */
  model ServiceTraits {
    ...SupportsRepeatableRequests;
    ...SupportsConditionalRequests;
    ...SupportsClientRequestId<Versions.`2025-01-30`>;
  }

  alias Operations = Azure.Core.ResourceOperations<ServiceTraits>;
}

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
@versioned(Versions)
namespace ContosoOperations {
  using Contoso.WidgetManager;

  interface Widgets {
    // Operation Status
    /** Gets status of a Widget operation. */
    @sharedRoute
    getWidgetOperationStatus is Operations.GetResourceOperationStatus<Widget>;
    /** Gets status of a Widget delete operation. */
    @sharedRoute
    getWidgetDeleteOperationStatus is Operations.GetResourceOperationStatus<
      Widget,
      never
    >;

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
      ListQueryParametersTrait<StandardListQueryParameters &
        SelectQueryParameter>
    >;

    // Widget Analytics
    /** Get a WidgetAnalytics */
    getAnalytics is Operations.ResourceRead<WidgetAnalytics>;

    /** Creates or updates a WidgetAnalytics */
    updateAnalytics is Operations.ResourceCreateOrUpdate<WidgetAnalytics>;

    // Widget Repair Operations
    /** Get the status of a WidgetRepairRequest. */
    #suppress "@azure-tools/typespec-azure-core/use-standard-operations" "This is a custom operation status endpoint."
    @route("/widgets/{widgetId}/repairs/{operationId}")
    getRepairStatus is Foundations.GetOperationStatus<
      WidgetRepairStatusParams,
      WidgetRepairRequest
    >;

    /** Schedule a widget for repairs. */
    @pollingOperation(Widgets.getWidgetOperationStatus)
    scheduleRepairs is Operations.LongRunningResourceAction<
      Widget,
      WidgetRepairRequest,
      WidgetRepairRequest & RequestIdResponseHeader
    >;
  }

  interface WidgetParts {
    /** Gets status of a WidgetPart operation. */
    getWidgetPartOperationStatus is Operations.GetResourceOperationStatus<WidgetPart>;

    /** Creates a WidgetPart */
    createWidgetPart is Operations.ResourceCreateWithServiceProvidedName<WidgetPart>;

    /** Get a WidgetPart */
    getWidgetPart is Operations.ResourceRead<WidgetPart>;

    /** Delete a WidgetPart */
    deleteWidgetPart is Operations.ResourceDelete<WidgetPart>;

    /** List WidgetPart resources */
    listWidgetParts is Operations.ResourceList<WidgetPart>;

    /** Reorder all parts for the widget. */
    @pollingOperation(WidgetParts.getWidgetPartOperationStatus)
    reorderParts is Operations.LongRunningResourceCollectionAction<
      WidgetPart,
      WidgetPartReorderRequest,
      never
    >;
  }

  interface Manufacturers {
    /** Gets status of a Manufacturer operation. */
    getManufacturerOperationStatus is Operations.GetResourceOperationStatus<Manufacturer>;

    /** Creates or replaces a Manufacturer */
    createOrReplaceManufacturer is Operations.ResourceCreateOrReplace<Manufacturer>;

    /** Get a Manufacturer */
    getManufacturer is Operations.ResourceRead<Manufacturer>;

    /** Delete a Manufacturer asynchronously. */
    @pollingOperation(Manufacturers.getManufacturerOperationStatus)
    deleteManufacturer is Operations.LongRunningResourceDelete<Manufacturer>;

    /** List Manufacturer resources */
    listManufacturers is Operations.ResourceList<Manufacturer>;
  }

  /** The Contoso Widget Manager service version. */
  enum Versions {
    /** Version 2022-08-30 */
    @useDependency(Azure.Core.Versions.v1_0_Preview_2)
    @useDependency(Contoso.WidgetManager.Versions.`2022-08-30`)
    `2022-08-30`,

    /** Version 2025-01-30 */
    @useDependency(Azure.Core.Versions.v1_0_Preview_2)
    @useDependency(Contoso.WidgetManager.Versions.`2025-01-30`)
    `2025-01-30`,
  }
  // A "global" RPC operation
  /** Responds with status information about the overall service. */
  @route("service-status")
  op getServiceStatus is RpcOperation<
    {},
    {
      statusString: string;
    },
    ServiceTraits
  >;
}
```
Feel free to reach out with any specific questions.
The linked playground is a little more complex than it needs to be, but, depending on what is in your client.tsp, is likely similar.  Note that this also gives you the option of making version-specific client.tsp changes, but simply having the linkage as shown using the `@useDependency` decorators will ensure that the client.tsp is used in both versions.

# First experience of TSP ApiVersion introduction - passed all CI checks, what's next?

## question 
Hi TypeSpec friends! 
 
We are bringing 2nd "private-preview" API version to exercise TSP-based wirings and learn the ecosystem:
https://github.com/Azure/azure-rest-api-specs-pr/pull/22321
 
First round was back in the swagger days.
Now that we are bringing TSP, it comes with a lot of learning.
We made lots of adjustments to satisfy the checks, that were not really changing the protocol, and we had to apply several suppressions otherwise since development of this version has completed - and it is heading to PowerShell CLI partners.
 
It is clear that specification work of the next API version need to be exercised through Azure REST repository tooling to flip the process around, and be able to make protocol affecting changes.
 
With that said - with all checks satisfied - except for "Automated merging requirements met" - what is the next step to bring this PR into the review loop?
 
Updated based on the review comments:

1) for comment [about metadata](https://github.com/Azure/azure-rest-api-specs-pr/pull/22321#discussion_r2072077880)

we have:
```
  @doc("The metadata")
  metadata?: Record<string>;
```
suggestion was: Consider using array of KVP
 
my follow up suggestion: our desired over-the-wire representation is { "mykey": "myvalue" }
can we achieve that via below, will that be supported:
```
model MetadataModel {  [key: string]: string;}
```
2) regarding a [question for the purpose of the PR](https://github.com/Azure/azure-rest-api-specs-pr/pull/22321#issuecomment-2847987559) - I provided brief [answer](https://github.com/Azure/azure-rest-api-specs-pr/pull/22321#issuecomment-2848108430), can you please share the "control plane template" form for me to fill?
 
3) for the initatorId property [question](https://github.com/Azure/azure-rest-api-specs-pr/pull/22321#discussion_r2072077812) - I answered it, not sure if I should put all of my answer in the @doc, since some of that doesn't have to be public facing. Would a regular comment be of help for reviewers, something that is otherwise invisible to the swagger?
 
4) there was a [recommendation](https://github.com/Azure/azure-rest-api-specs-pr/pull/22321#discussion_r2072053387) on how operation ids should look like - and my question was how do we control operation ids, since I am not seeing TSP code of ours being responsible for operation id strings that end up in the swagger?

## answer
Some thoughts:
1. That issue isn't how to get Typespec to construct that over-the-wire pattern. The issue is that the dictionary pattern is an ARM anti-pattern. It defeats important ARM features that customers expect to be able to use (ARG, and Azure Policy). There are other issues with this pattern: (how are the supported keys documented, how are the supported keys versioned, how do clients determine what keys are required vs. optional, etc.).
2. You can create a new dummy PR in github to get a template file and add it to your existing PR. If you use the link to create the PR, Github automatically adds the template file.
3. The type of that property was string. The suggestion is to use the name and @doc to help clients understand what that string represents and how to correctly populate it. It's up to you how much to share about internals.
4. This appears to be the only TypeSpec Discussion question here. I don't know the answer.
Follow-up on items 1-3 above should be in PR comments rather than here. Current on-call reviewer will follow up.

# How to restrict importing typespec files in main based off of versions.

## question 
Hi Team,
We have 2 imports in our main.tsp to include other resource type tsp files, however we do not want to include one of the resourcetype to the new api version we want to introduce.  Is there a way to do conditional imports based off of version in main?
 
Thank you in advance!
## answer
no, you have to mark the models/types and everything that you want to remove with the @removed decorator using the versioning library
 
if you are in preview version I think also the policy is to only have a single preview version in the spec repo at the timme now so you could also just delete it

# Does TypeSpec support example generation for new added versions?

## question 
Hi team, when adding a new api version in the TypeSpec, is there any way I can generate thoes example Json files from preview version and with the "api-version" property changed? Or I will need to manually cope the example Json files from preview version and update the "api-version" property inside all of them?

## answer
You will need to place version specific examples under `examples\[version]`. So if you are adding a new version, you can copy over the example files and make appropriate add/remove/update to them including the `api-version`
Note that you can also use swagger-based example generation, for the new version (which is less attractive if you have customized the examples)

# Description changes across versions?

## question 
Hi, as part of this PR: [Service Fabric Managed Clusters - API version 2025-03-01-preview · Azure/azure-rest-api-specs@599e269](https://github.com/Azure/azure-rest-api-specs/actions/runs/14090043123/job/39464153437?pr=33332)

My team wanted to add more details to a model description. This change results in a change in all spec versions generated with Typespec, and causes the Typespec validation to fail if I don't include the changes to the older specs. 
 
I wanted to know what the best course of action was for passing this check. Since we don't expect updates to our older specs, is it ok to just change the output path in our tspconfig.yaml to only point at the current version of the output spec? Or is there a better way to handle this?

## answer
Honestly, the best thing is to update your docs and take the update in previous versions (which are likely now more accurately described as well). Documentation updates are not breaking changes, and, if changes are limited to documentation, this should be passed easily by the breaking change board.
