# is sdk generation required for private preview?

## question 
we are trying to get our api approved for a private preview, but our preview customers do not require an SDK. We were hoping to onboard to SDK later.
 
I'm a bit confused. Does api approval always include SDK? If not, how do we setup tspconfig to validate.
 
we are creating an arm api / not dataplane. I've seen some tspconfig.yaml without sdk configs, but when i try removing them I encounter tsv failures.

## answer
As of today how the system is designed, you need to suppress those warning if you do not want to include SDK at this time. See an example:
https://github.com/Azure/azure-rest-api-specs/blob/main/specification/monitor/Monitor.Ingestion/suppressions.yaml:
```
- tool: TypeSpecValidation
  paths:
    - tspconfig.yaml
  rules:
    - SdkTspConfigValidation
  sub-rules:
    # Suppress JS package & dir names related to RLC, which require "rest" in the name. We do not use RLC.
    - options.@azure-tools/typespec-ts.package-dir
    - options.@azure-tools/typespec-ts.package-details.name
  reason: 'See above comments for details'
```
When you will need SDK, you will be required to remove the file and follow the config validation process.

# What is `x-ms-long-running-operation-options` for LRO operation of data-plane when `emit-lro-options: none` in `@azure-tools/typespec-autorest`?

## question 
Many `tspconfig.yaml` files for data-plane have an option `emit-lro-options: none` for emitter `@azure-tools/typespec-autorest`, which means only emit `x-ms-long-running-operation` but does not emit`x-ms-long-running-operation-option` for resource providers, like the following loadtestservice `tspconfig.yaml`: [azure-rest-api-specs/specification/loadtestservice/LoadTestService/tspconfig.yaml at main · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/blob/main/specification/loadtestservice/LoadTestService/tspconfig.yaml#L24)
```
parameters:
  service-dir:
    default: "sdk/loadtesting"
  "service-name":
    default: "loadtesting"
emit:
  - "@azure-tools/typespec-autorest"
  # Uncomment this line and add "@azure-tools/typespec-python" to your package.json to generate Python code
  #- "@azure-tools/typespec-python"
  #- "@azure-tools/typespec-ts"
  #- "@azure-tools/typespec-csharp"
  # Uncomment this line and add "@azure-tools/typespec-java" to your package.json to generate Java code
  # "@azure-tools/typespec-java": true
  # Uncomment this line and add "@azure-tools/typespec-csharp" to your package.json to generate C# code
  # "@azure-tools/typespec-csharp": true
  # Uncomment this line and add "@azure-tools/typespec-ts" to your package.json to generate Typescript code
  # "@azure-tools/typespec-ts": true
linter:
  extends:
    - "@azure-tools/typespec-azure-rulesets/data-plane"
options:
  "@azure-typespec/http-client-csharp":
    namespace: Azure.Developer.LoadTesting
    model-namespace: false
  "@azure-tools/typespec-autorest":
    emitter-output-dir: "{project-root}/../"
    output-file: "{azure-resource-provider-folder}/{service-name}/{version-status}/{version}/loadtestservice.json"
    azure-resource-provider-folder: "data-plane"
    emit-lro-options: "none"
    omit-unreachable-types: true
  "@azure-tools/typespec-python":
    package-dir: "azure-developer-loadtesting"
    namespace: "azure.developer.loadtesting"
    generate-test: true
    generate-sample: true
    package-mode: azure-dataplane
    flavor: azure
  "@azure-tools/typespec-ts":
    package-dir: "load-testing-rest"
    title: Azure Load Testing
    description: Azure Load Testing Client
    generate-metadata: true
    generate-test: false
    package-details:
      name: "@azure-rest/load-testing"
      description: "This package contains Microsoft Azure LoadTestingClient client library."
      version: 1.0.1
    flavor: azure
  "@azure-tools/typespec-csharp":
    package-dir: "Azure.Developer.LoadTesting"
    clear-output-folder: true
    namespace: "{package-dir}"
    generate-sample-project: false
    model-namespace: false
    flavor: azure
  "@azure-tools/typespec-java":
    package-dir: "azure-developer-loadtesting"
    namespace: com.azure.developer.loadtesting
    enable-sync-stack: true
    partial-update: true
    generate-tests: false
    generate-samples: false
    service-name: Load Test
    flavor: azure
```

So what exactly does this data-plane operation use when polling LRO request then? Can someone explain a little more about this situation of `emit-lro-options: none`? thanks

## answer
This is just an emitter option for emission of OpenAPI from the spec.  It doesn't impact how other emitters view the LRO - the LRO is resolved based on the encoding of the operation.
 
Because it's nice to have a visual indicator that the lro is encoded correctly, it is highly encouraged that spec authors use no emit-lro-options seting, , or use `emit-lro-options: "all"` to check .  But this is not required for check-in, because the lro-options are a microsoft-specific extension with little or no documentary value to customers.

We don't generate data plane clients from OpenAPI if there is a corresponding TypeSpec, they are generated form TypeSpec directly.

# Need help in Adding final-state-schema for a single post action

## question 
Hi TypeSpec Discussion,
PR: https://github.com/Azure/azure-rest-api-specs-pr/pull/22427
 
We would like to add final-state-schema for a single post action LRO operation for now. Is it possible to do it for a single post action or not ?
 
when we add emit-lro-options: "all" in tspconfig.yml. It is reflected in all the long-running options..

## answer
So, what you need to do is the following:  Change the response type to match the final result type you want  Here is a playground showing that the response parameter in ArmResourceActionAsync shows up both in the 200 response and in the final-state-schema, which means that typespec-based emitters  will get the right final response value and so will swagger-based emitters.  
 
To be clear the final-state-schema in this case is just for debugging purposes is it not necessary in the actual swagger.  I added a comment to the PR showing the change you should make here.

# how to set readOnly for union

## question 
I have these lintDiff [errors](https://github.com/Azure/azure-rest-api-specs/actions/runs/16303501902): 
```
| ProvisioningStateMustBeReadOnly | provisioningState property must be set to readOnly. Location: GalleryRP/stable/2025-03-03/GalleryRP.json#L3483 | RPC-Async-V1-16 |
| ProvisioningStateMustBeReadOnly | provisioningState property must be set to readOnly. Location: GalleryRP/stable/2025-03-03/GalleryRP.json#L3545 | RPC-Async-V1-16 |
| ProvisioningStateMustBeReadOnly | provisioningState property must be set to readOnly. Location: GalleryRP/stable/2025-03-03/GalleryRP.json#L3551 | RPC-Async-V1-16 |
```
Pretty much I have set union [GalleryProvisioningState](https://github.com/Azure/azure-rest-api-specs/blob/2acbb85d39640fc0b45a053023d79bc16ecc0a7d/specification/compute/Gallery.Management/models.tsp#L19) to readOnly
I tried adding `use-read-only-status-schema: true` to tspconfig.yaml but it changed some other unions and adding breaking changes 
 
is there another way to resolve this ? 

## answer
use-read-only-status-schema will change the status schema of any unions that are effectively read-only (that is, all the properties that use the union are marked as readOnly).  So, unless you are thinking that a user can actually set the value for any of the changed unions,  that is the recommendation.
 
`readOnly` requirements in lintdiff are bit over-used,  there is no difference between a response-only property that is marked as 'readOnly'  and a response-only property that is unmarked.  

# Namespace when not specified?

## question 
[azure-rest-api-specs/specification/ai/Face/tspconfig.yaml at main · Azure/azure-rest-api-specs](https://github.com/Azure/azure-rest-api-specs/blob/main/specification/ai/Face/tspconfig.yaml)
```
parameters:
  "service-dir":
    default: "sdk/face"
  "dependencies":
    default: ""
emit:
  - "@azure-tools/typespec-autorest"
options:
  "@azure-tools/typespec-autorest":
    azure-resource-provider-folder: "data-plane"
    emit-lro-options: "none"
    emitter-output-dir: "{project-root}/.."
    omit-unreachable-types: true
    output-file: "{azure-resource-provider-folder}/{service-name}/{version-status}/{version}/Face.json"
  "@azure-tools/typespec-python":
    package-dir: "azure-ai-vision-face"
    namespace: "azure.ai.vision.face"
    package-version: 1.0.0b2
    package-mode: dataplane
    flavor: azure
    generate-test: true
    generate-sample: true
  "@azure-tools/typespec-csharp":
    package-dir: "Azure.AI.Vision.Face"
    namespace: "{package-dir}"
    clear-output-folder: true
    model-namespace: false
    flavor: azure
  "@azure-typespec/http-client-csharp":
    namespace: Azure.AI.Vision.Face
    model-namespace: false
  "@azure-tools/typespec-ts":
    package-dir: "ai-vision-face-rest"
    generate-metadata: true
    flavor: azure
    package-details:
      name: "@azure-rest/ai-vision-face"
      description: "Face API REST Client"
  "@azure-tools/typespec-java":
    package-dir: "azure-ai-vision-face"
    namespace: com.azure.ai.vision.face
    partial-update: true
    use-eclipse-language-server: false
    enable-subclient: true
    generate-samples: false
    generate-tests: false
    flavor: azure
linter:
  extends:
    - "@azure-tools/typespec-azure-rulesets/data-plane"
  disable:
    "@azure-tools/typespec-azure-core/operation-missing-api-version": "API version located in the host template"
    "@azure-tools/typespec-azure-core/use-standard-operations": "Most of our operation doesn't fit standard ops"
    "@azure-tools/typespec-azure-core/use-standard-names": "Most of our operation doesn't fit standard ops"
```
I'm working on adding Tier 1 language namespace names to TypeSpec APIView. I can get this from the compiler options when namespace is specified in the tspconfig.yaml. However, what should the behavior be when, as in the above config, the namespace isn't specified (for typespec-ts)?  What heuristic is applied to determine the namespace? Is it language specific?

## answer
When the `namespace` isn't explicitly specified in `tspconfig.yaml`, the behavior falls back to what's defined in the TypeSpec file itself—i.e., whatever `namespace` is declared there. If no namespace is declared in the TypeSpec, then the emitter determines the default, which may vary by language.

In the management plane, it's fairly standardized: we derive the namespace based on the resource provider name, stripping prefixes like `Azure` or `Microsoft`, flattening separators, and applying language-specific naming conventions. For example:

* .NET: `Azure.ResourceManager.[ProviderName]`
* Python: `azure-mgmt-[providername]`
* Java: `com.azure.resourcemanager.[providername]`
* JS: `@azure/arm-[providername]`

For **data plane**, it's similar but instead of "ResourceManager", you use the service group (like `AI`, `Data`, etc.). By default, it uses the namespace from the TypeSpec unless you override it via:

```yaml
namespace: Azure.LoadTesting
```

in the `tspconfig.yaml`. That flag overrides the namespace across all language emitters.

There was some confusion with how this gets surfaced through TCGC. Although `clients[0].namespace` is supposed to reflect the effective namespace, it wasn't showing the expected value (`azure.ai.vision.face`). Isabella helped identify the issue and confirm that it was due to a workaround or emitter configuration not being applied as expected.
