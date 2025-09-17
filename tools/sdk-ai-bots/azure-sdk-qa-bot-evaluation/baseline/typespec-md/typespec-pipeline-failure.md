# TypeSpec Avocado failing even with labels tag

## question 
I have the following PR that is currently failing: [[CTSRP] Update models for old version to be typespec compatible by joschung · Pull Request #23680 · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/pull/23680)
Here is a picture of the avocado failure:
```
Run Avocado

Run AVOCADO_OUTPUT_FILE=$RUNNER_TEMP/avocado.ndjson
avocadoDir: /home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager
avocadoDir: /home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager

{"level":"Error","code":"MISSING_APIS_IN_DEFAULT_TAG","message":"The default tag should contain all APIs. The API path `/providers/private.azuredatatransfer/validateschema` is not in the default tag. Please make sure the missing API swaggers are in the default tag.","tag":"default","readMeUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/readme.md","jsonUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.azuredatatransfer.json","path":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.AzureDataTransfer/preview/2025-04-11-preview/azuredatatransfer.json","apiPath":"/providers/private.azuredatatransfer/validateschema"}

{"level":"Error","code":"MISSING_APIS_IN_DEFAULT_TAG","message":"The default tag should contain all APIs. The API path `/providers/private.azuredatatransfer/listapprovedschemas` is not in the default tag. Please make sure the missing API swaggers are in the default tag.","tag":"default","readMeUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/readme.md","jsonUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.AzureDataTransfer/preview/2025-04-11-preview/azuredatatransfer.json","path":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.AzureDataTransfer/preview/2025-04-11-preview/azuredatatransfer.json","apiPath":"/providers/private.azuredatatransfer/listapprovedschemas"}

{"level":"Error","code":"MISSING_APIS_IN_DEFAULT_TAG","message":"The default tag should contain all APIs. The API path `/subscriptions/{}/resourceGroups/{}/providers/private.azuredatatransfer/pipelines/{}/listschemas` is not in the default tag. Please make sure the missing API swaggers are in the default tag.","tag":"default","readMeUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/readme.md","jsonUrl":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.AzureDataTransfer/preview/2025-04-11-preview/azuredatatransfer.json","path":"/home/runner/work/azure-rest-api-specs-pr/c93b354fd9c14905bb574a8834c46d9b/specification/azuredatatransfer/resource-manager/private.AzureDataTransfer/preview/2025-04-11-preview/azuredatatransfer.json","apiPath":"/subscriptions/{}/resourceGroups/{}/providers/private.azuredatatransfer/pipelines/{}/listschemas"}

errors: 3
##vso[task.setVariable variable=validationResult]failure

Error: Process completed with exit code 1.
```
However, I have the Approved-Avocado label on my PR so I'm unsure as to why this is still running?

## answer
My guess is you confused the intermediate check "Swagger Avocado - Analyze Code" with the final required check "Swagger Avocado".

# SDK Generation failure

## question 
Hi Team,
 
I am seeing an SDK generation failure in my PR. Can you help what we are doing wrong? [Support multidownload support by aneesh-ponneth · Pull Request #22690 · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/pull/22690/checks?check_run_id=43636536312)

```
Build log #L251

Errors occurred while generating SDK from specification/edge. Follow the steps at https://aka.ms/azsdk/sdk-automation-faq#how-to-view-the-detailed-sdk-generation-errors to view detailed errors.
2025-06-06 18:23:40 [ERROR] Error occurred when call tsp-client:
2025-06-06 18:23:40 [ERROR] fail to generate sdk for specification/edge/Microsoft.Edge.DisconnectedOperations.Management: Command 'tsp-client init --tsp-config /mnt/vss/_work/1/s/azure-rest-api-specs-pr/specification/edge/Microsoft.Edge.DisconnectedOperations.Management --local-spec-repo /mnt/vss/_work/1/s/azure-rest-api-specs-pr/specification/edge/Microsoft.Edge.DisconnectedOperations.Management --commit 788b0c469a26449b7c083547ca2ab1635865423b --repo Azure/azure-rest-api-specs-pr --skip-install --debug' returned non-zero exit status 1.
2025-06-06 18:23:40 [ERROR] ======================================= Whant Can I do (begin) ========================================================================
2025-06-06 18:23:40 [ERROR] Fail to generate sdk for specification/edge/Microsoft.Edge.DisconnectedOperations.Management. If you are from service team, please first check if the failure happens only to Python automation, or for all SDK automations. 
2025-06-06 18:23:40 [ERROR] If it happens for all SDK automations, please double check your Swagger / Typespec, and check whether there is error in ModelValidation and LintDiff. 
2025-06-06 18:23:40 [ERROR] If it happens to Python alone, you can open an issue to https://github.com/Azure/autorest.python/issues. Please include the link of this Pull Request in the issue.
2025-06-06 18:23:40 [ERROR] ======================================= Whant Can I do (end) =========================================================================
Refer to the inner logs for details or report this issue through https://aka.ms/azsdk/support/specreview-channel.
ErrorStack: Error: [EXT-ERR] Failed to read generateOutput.json. Please check if the generate script is configured correctly.
```
 
and below is the error details [Pipelines - Run 20250606.27 logs](https://dev.azure.com/azure-sdk/internal/_build/results?buildId=4949592&view=logs&j=83516c17-6666-5250-abde-63983ce72a49&t=00be4b52-4a63-5865-8e02-c61723ad0692). 
 
Error log
[dev.azure.com/azure-sdk/590cfd2a-581c-4dcb-a12e-6568ce786175/_apis/build/builds/4949592/logs/15](https://dev.azure.com/azure-sdk/590cfd2a-581c-4dcb-a12e-6568ce786175/_apis/build/builds/4949592/logs/15)

## answer
From the GO messages,  it looks like there are a couple of unresolved imports:
```
/models.tsp:4:1 - error import-not-found: Couldn't resolve import "@typespec/openapi3"
/images.tsp:7:1 - error import-not-found: Couldn't resolve import "../Microsoft.Edge.Shared/common.tsp"
```

To be clear,  you should not import @typespec/openapi3 as part of your spec.  Unless you are using openapi3-specific decorators (which you should not) this import is most likely unnecessary

# Swagger LintDiff - Analyze Code failing in PR

## question 
I have the following PR that is currently failing: [[CTSRP] Update models for old version to be typespec compatible by joschung · Pull Request #23680 · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/pull/23680)
The error message isn't clear as to why the LintDiff is failing for my PR. Here is a picture of the error:
```
Run echo "summary=$GITHUB_STEP_SUMMARY" >> $GITHUB_OUTPUT

echo "summary=$GITHUB_STEP_SUMMARY" >> $GITHUB_OUTPUT

npm exec --no -- lint-diff \
--before before \ 
--after after \ 
--changed-files-path changed-files.txt \ 
--base-branch RP5aa5Dev \ 
--compare-sha 4c38db95d583774352f75251aaf02408935856a8 \
--out-file $GITHUB_STEP_SUMMARY

shell: /usr/bin/bash -e {0}
env:
NODE_OPTIONS: --max-old-space-size=8192

Using @microsoft.azure/openapi-validator version: 2.2.4

file:///home/runner/work/azure-rest-api-specs-pr/azure-rest-api-specs-pr/.github/shared/src/spec-model.js:150
throw new Error(`No affected swaggers found in specModel for ${swaggerPath}`);
^

Error: No affected swaggers found in specModel for /home/runner/work/azure-rest-api-specs-pr/azure-rest-api-specs-pr/before/specification/azuredatatransfer/resource-manager/Private.AzureDataTransfer/stable/2025-05-21/azuredatatransfer.json
    at SpecModel.getAffectedSwaggers (file:///home/runner/work/azure-rest-api-specs-pr/.github/shared/src/spec-model.js:150:13)
    at async buildState (file:///home/runner/work/azure-rest-api-specs-pr/tools/lint-diff/dist/src/processChanges.js:117:28)
    at async getRunList (file:///home/runner/work/tools/lint-diff/dist/src/processChanges.js:29:27)
    at async runLintDiff (file:///home/tools/lint-diff/dist/src/lint-diff.js:78:53)
    at async main (file:///home/tools/lint-diff/dist/src/lint-diff.js::73.5)
    at async file://tools/cmd/lint-diff/js::1

Node.js v22.17.0

Error Process completed with exit code 1.
```
How do I fix this?

## answer
The error message points to the problem:
```
No affected swaggers found in specModel for /home/runner/work/azure-rest-api-specs-pr/azure-rest-api-specs-pr/before/specification/azuredatatransfer/resource-manager/Private.AzureDataTransfer/stable/2025-05-21/azuredatatransfer.json
```
This means, the file in the error message was not reachable, either directly or via a reference, from your readme.md.  Either add the file to your readme.md, or delete the file.

# having trouble with examples not matching @pattern rules in github actions

## question 
I think I'm doing something wrong when I generate my examples... what should i be doing?
 
for example this tsp:
```
model AcStorConfiguration is ExtensionResource<AcStorConfigurationProperties> {
  ...ResourceNameParameter<
    AcStorConfiguration,
    NamePattern = "^acstor-configuration$"
  >;
}
```
then:
```
oav generate-examples ./specification/arccontainerstorage/resource-manager/Microsoft.ArcContainerStorage/preview/2024-10-01-preview/arccontainerstorage.json
```
and i run 
```
npx prettier --write specification/arccontainerstorage/resource-manager/Microsoft.ArcContainerStorage/preview/2024-10-01-preview/examples/*  
 npx tsv specification/arccontainerstorage/ArcContainerStorage.Management    
```
example github action failure:https://github.com/Azure/azure-rest-api-specs-pr/pull/23604/checks?check_run_id=46129519720

## answer
I think you need to manually update the auto-generated strings to satisfy the regexes. 
 
most of the errors, the example itself tells you to update the string:
 
```
GitHub Actions / [TEST-IGNORE] Swagger ModelValidation

PATTERN: String does not match pattern ^acstor-configuration$: Replace this value with a string matching RegExp ^acstor-configuration$
```
This one is a little unusual, but I think just update the string to something that works, unless your spec is doing something unusual with this id property.
```
GitHub Actions / [TEST-IGNORE] Swagger ModelValidation

INVALID_FORMAT: Object didn't pass validation for format arm-id: gkbwmag
```
Examples generation was written to produce a structure that you can update, but there is more work necessary to fully generate the placholders. It's just not been prioritized work unfortunately.

# Swagger LintDiff failed

## question 
I have many lintDiff failures in this [PR](https://github.com/Azure/azure-rest-api-specs/pull/34318). The interesting thing is, the place I'm violating the rules, its swagger is the same as before. Why these violations are not reported before?

## answer
The LintDiff errors in your PR are not necessarily new violations. Many of them already existed in the main branch, but previously, CI failures weren’t blocking, so these issues were effectively allowed to persist unnoticed.

Now, because your PR is part of a TypeSpec (TSP) migration, the generated Swagger includes many differences—over 3,000 diffs in total—due to normalization by the AutoRest emitter. These changes are functionally equivalent but not textually identical, which makes it hard for the LintDiff auto-baselining algorithm to recognize unchanged areas. As a result, you’re seeing errors flagged as new, even if the same issues existed in main.

Given the unreliability of the auto-baselining mechanism, we recommend not depending on it. Instead, all LintDiff errors—regardless of whether they are newly introduced or pre-existing—should be explicitly suppressed in the `readme.md` file. This is the mitigation approach we advise for all PRs, especially TSP migrations.

# Duplicate model definition errors on compilation with TypeSpec Compiler v1.0.0 - Microsoft.Mission

## question 
I am working on this PR [Add Microsoft.Mission version 2025-05-01-preview by tgoodyear · Pull Request #22581 · Azure/azure-rest-api-specs-pr](https://github.com/Azure/azure-rest-api-specs-pr/pull/22581) and after the TypeSpec compiler and dependency bumps a couple weeks ago, I am getting compilation errors regarding `duplicate-type-name` for multiple models and unions. I also receive an error about a UnionVariant requiring `@doc`, but it is clearly in the source. None of these errors or warnings occur on 1.0.0-rc.1.
 
The last commit involving package.json and package-lock.json that works is [[shared\] Add SpecModel (#33362) · Azure/azure-rest-api-specs-pr@3fcf376](https://github.com/Azure/azure-rest-api-specs-pr/commit/3fcf37699e1c8687141725d630f027c2623087e1), this includes `"@typespec/compiler": "1.0.0-rc.1"` 
 
I've checked the breaking changes for TypeSpec v1.0.0 as compared to 1.0.0-rc.1 and don't see anything relevant for my spec.
 
Anyone else experiencing similar issues? Full errors below and available at the [TypeSpec Validation check for the PR](https://github.com/Azure/azure-rest-api-specs-pr/actions/runs/15098397916/job/42436108953?pr=22581).
```
Executing rule: Compile
  execFile("npm", ["exec","--no","--","tsp","compile","--list-files","--warn-as-error","/home/runner/work/azure-rest-api-specs-pr/azure-rest-api-specs-pr/specification/mission/Mission.Management"])
  TypeSpec compiler v1.0.0
  
  Diagnostics were reported during compilation:
  
  specification/mission/Mission.Management/resourcetypes/catalog/catalog.tsp:31:3 - error @azure-tools/typespec-azure-core/documentation-required: The UnionVariant named 'blob' should have a documentation or description, use doc comment /** */ to provide it.
  > 31 |   blob: BlobCatalog,
       |   ^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:55:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'GovernedServiceItem'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 55 | model GovernedServiceItem {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:10:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitHubState'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 10 | union TransitHubState {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:53:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOption'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 53 | model TransitOption {
       |       ^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:36:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 36 | model EnclaveEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:59:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 59 | model CommunityEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/virtualEnclave/virtualenclave.tsp:32:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'SubnetConfiguration'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 32 | model SubnetConfiguration {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/principal.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'Principal'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | model Principal {
      |       ^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ServiceIdentifier'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | union ServiceIdentifier {
      |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:30:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 30 | union TransitOptionType {
       |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:44:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionParams'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 44 | model TransitOptionParams {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union EnclaveEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:42:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'DestinationType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 42 | union DestinationType {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union CommunityEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/roleassignmentitem.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'RoleAssignmentItem'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | model RoleAssignmentItem {
      |       ^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:55:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'GovernedServiceItem'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 55 | model GovernedServiceItem {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:10:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitHubState'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 10 | union TransitHubState {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:53:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOption'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 53 | model TransitOption {
       |       ^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:36:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 36 | model EnclaveEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:59:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 59 | model CommunityEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/approvals/approvals.tsp:65:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'Approver'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 65 | model Approver {
       |       ^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/principal.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'Principal'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | model Principal {
      |       ^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/virtualEnclave/virtualenclave.tsp:32:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'SubnetConfiguration'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 32 | model SubnetConfiguration {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ServiceIdentifier'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | union ServiceIdentifier {
      |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/community/community.tsp:31:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ApprovalPolicy'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 31 | union ApprovalPolicy {
       |       ^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/community/community.tsp:43:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'MandatoryApprover'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 43 | model MandatoryApprover {
       |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:30:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 30 | union TransitOptionType {
       |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:44:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionParams'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 44 | model TransitOptionParams {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union EnclaveEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:42:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'DestinationType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 42 | union DestinationType {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union CommunityEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/approvals/approvals.tsp:32:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ActionPerformedEnum'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 32 | union ActionPerformedEnum {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/roleassignmentitem.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'RoleAssignmentItem'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | model RoleAssignmentItem {
      |       ^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:55:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'GovernedServiceItem'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 55 | model GovernedServiceItem {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:10:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitHubState'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 10 | union TransitHubState {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:53:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOption'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 53 | model TransitOption {
       |       ^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:36:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 36 | model EnclaveEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:59:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointDestinationRule'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 59 | model CommunityEndpointDestinationRule {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/approvals/approvals.tsp:65:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'Approver'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 65 | model Approver {
       |       ^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/principal.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'Principal'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | model Principal {
      |       ^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/shared/governedserviceitem.tsp:5:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ServiceIdentifier'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 5 | union ServiceIdentifier {
      |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/virtualEnclave/virtualenclave.tsp:32:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'SubnetConfiguration'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 32 | model SubnetConfiguration {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/community/community.tsp:31:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ApprovalPolicy'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 31 | union ApprovalPolicy {
       |       ^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/community/community.tsp:43:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'MandatoryApprover'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 43 | model MandatoryApprover {
       |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:30:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 30 | union TransitOptionType {
       |       ^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/transitHub/transitHub.tsp:44:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'TransitOptionParams'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 44 | model TransitOptionParams {
       |       ^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/enclaveEndpoint/enclaveEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'EnclaveEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union EnclaveEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:42:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'DestinationType'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 42 | union DestinationType {
       |       ^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/communityEndpoint/communityEndpoint.tsp:13:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'CommunityEndpointProtocol'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 13 | union CommunityEndpointProtocol {
       |       ^^^^^^^^^^^^^^^^^^^^^^^^^
  specification/mission/Mission.Management/resourcetypes/approvals/approvals.tsp:32:7 - error @typespec/openapi/duplicate-type-name: Duplicate type name: 'ActionPerformedEnum'. Check @friendlyName decorators and overlap with types in TypeSpec or service namespace.
  > 32 | union ActionPerformedEnum {
       |       ^^^^^^^^^^^^^^^^^^^
  
  Found 50 errors.
```

## answer
This is a bug, but there are a couple of things you are doing here you probably want to avoid as well:
Don't use the transformation decorators (like `@withoutDefaultProperties`) directly 
Don't use the transformation templates without giving the transformed object a name (as in `properties?: Update<CommunityPatchProperties>;`)
Here is a playground showing how this can be worked around (and you can end up with slightly better names)
[Here is a playground](https://azure.github.io/typespec-azure/playground/?c=aW1wb3J0ICJAdHlwZXNwZWMvcmVzdCI7CtIZaHR0cNUZdmVyc2lvbmluZ8wfYXp1cmUtdG9vbHMvyCstxhVjb3Jl3yvIK3Jlc291cmNlLW1hbmFnZXIiOwp1c2luZyBBxB8uQ29yZc4SUscxTcYwyB1UeXBlU3BlYy5IdHRw0RVSZXN00RVW6QDXOwpA5wDkZWQoxxdzKQpAZG9jKCJNaWNyb3NvZnQgTWlzxBog6ACDIFByb3ZpZGVyIOYAvW1lbnQgQVBJLiIpCkBhcm3IH05hbWVzcGFjZctNLsdNxCtzZXJ2aWNlKCN7IHRpdGxlOiDTKCB9KQpuyEkg0SA75wCuU3Vw5AF1ZWTkAIkg5wDWcyBmb3IgdGjTOCDoAYEgcOcAruQAvmVudW0g6AEJIHsKICDGXlRoZSAyMDI0LTA2LTAxLXByZXZpZXfIZ8Q6ICDkAPpDb21tb27kAWpzx0YoInY1xh%2FHEWluZy51c2VEZXBlbmRlbmN5KPUB4OgBpnMudjFfMF9QxnJfMd9L6QI910AyxEB25ADLXzA2XzAxX%2BcAyzogIvIA4CIsCn3nAV9Hb3Zlcm5lZFPmAbNJdGVt5AIGcGVydGllcyIpCm1vZGVsINQn6gFGxxYgSUTlARJpZDogc3Ry5QKIyGjkAUF1bml0ee0CbdBnySZCYXNlTcUTymblAn7nAR8gc3RhdGXnAaDEFmJpbGl0eShMaWZlY3ljbOQBd2FkxB7lAgbHNlPENT866QCDzFLFHDsKyW5MaXN0IG9mIOcCzHMgZ%2BcBAyBieSBhIGPoAKrmAITIHecBCsQ9OvQBNVtdID0gI1sje%2BUBJSIxMjMiIH1d%2FwEn6AEnIHdpdGhvdXQgZGVmYXVsdCB2YWx19AE%2BUGF0Y2jKOOQBk3MgVXBkYXRlYWJs5AD3xxo8T21pdETGTnM88gF%2BPj7oA4fJHCDFXekAn9d98AG80D9w7AH85QGlxw8%2FOvkAx%2BUBnS4uLvYDV0ZvdW5kYXTlAxpBcm1UYWdzxzt59AFf5gCh%2BgDGyBrkATpUcmFja2VkyBP0ASjmAOtrZXkoIukCGuQFJuYCoXNlZ%2BQFS8oc6AD4QHBhdHRlcm4oIl5bYS16QS1aXccIMC05LV0qyg1dJMkxaO0EvuQFQ%2BQCqOQFDcl76QC%2ByAnFPcQr%2FAOaSW50ZXJmYcUu5AT1yD1PcGVy5gFaCmnII%2BoBKuUA%2B2dldOQBKkHKNeQDgOoBKsgWPjvkAL11c2VGaW5hbOUDdFZpYSgi5gc8YXN5bmMt5AG1xW%2FlALVjcmVhdGVPcuYCv89lQ8cdUmVwbGFjZUHEQDwKICAg8gHJLMUXTHJvSGVhZGVycyA96weS7QI%2BUmV0cnlBZnRlcsYrICbFOyDEecVi6QEQxiAKICDlANd17ACiQ3VzdG9t5QLC%2FwCWICDmAxTmApI99ANJyGJkZWxl8QEERMUVV%2BYEG09rxm7RacVAbGlzdELJE0dyb3Vwz03kBLlCeVBhcmVudN1FU3Vic2NyaXDkAQfHRMY8zBnVQn3rAkXqAlogZXh0ZW5kc%2FYJFS7LKXt9Cg%3D%3D&e=%40azure-tools%2Ftypespec-autorest&options=%7B%22linterRuleSet%22%3A%7B%22extends%22%3A%5B%22%40azure-tools%2Ftypespec-azure-rulesets%2Fresource-manager%22%5D%7D%7D) showing how this can be worked around (and you can end up with slightly better names)
```
import "@typespec/rest";
import "@typespec/http";
import "@typespec/versioning";
import "@azure-tools/typespec-azure-core";
import "@azure-tools/typespec-azure-resource-manager";
using Azure.Core;
using Azure.ResourceManager;
using TypeSpec.Http;
using TypeSpec.Rest;
using TypeSpec.Versioning;
@versioned(Versions)
@doc("Microsoft Mission Resource Provider management API.")
@armProviderNamespace("Microsoft.Mission")
@service(#{ title: "Microsoft.Mission" })
namespace Microsoft.Mission;
@doc("Supported API versions for the Microsoft.Mission resource provider.")
enum Versions {
  @doc("The 2024-06-01-preview version.")
  @armCommonTypesVersion("v5")
  @Versioning.useDependency(Azure.ResourceManager.Versions.v1_0_Preview_1)
  @Versioning.useDependency(Azure.Core.Versions.v1_0_Preview_2)
  v2024_06_01_preview: "2024-06-01-preview",
}
@doc("GovernedServiceItem Properties")
model GovernedServiceItem {
  @doc("Service ID")
  id: string;
}
@doc("Community Resource Properties")
model CommunityBaseModel {
  @doc("Provisioning state.")
  @visibility(Lifecycle.Read)
  provisioningState?: ResourceProvisioningState;

  @doc("List of services governed by a community.")
  governedServiceList: GovernedServiceItem[] = #[#{ id: "123" }];
}
@doc("Community Resource Properties without default values")
model CommunityPatchProperties
  is UpdateableProperties<OmitDefaults<CommunityBaseModel>>;
@doc("Community Patch Resource")
model CommunityPatchModel {
  @doc("Community Patch properties")
  properties?: CommunityPatchProperties;

  ...Azure.ResourceManager.Foundations.ArmTagsProperty;
}
@doc("Community Model Resource")
model CommunityResource is TrackedResource<CommunityBaseModel> {
  @key("communityName")
  @segment("communities")
  @pattern("^[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z0-9]$")
  @path
  @doc("The name of the communityResource Resource")
  name: string;
}
@doc("Community Interface")
@armResourceOperations
interface Community {
  get is ArmResourceRead<CommunityResource>;
  @useFinalStateVia("azure-async-operation")
  createOrUpdate is ArmResourceCreateOrReplaceAsync<
    CommunityResource,
    LroHeaders = Azure.Core.Foundations.RetryAfterHeader &
      ArmAsyncOperationHeader
  >;
  update is ArmCustomPatchAsync<
    CommunityResource,
    PatchModel = CommunityPatchModel
  >;
  delete is ArmResourceDeleteWithoutOkAsync<CommunityResource>;
  listByResourceGroup is ArmResourceListByParent<CommunityResource>;
  listBySubscription is ArmListBySubscription<CommunityResource>;
}
interface Operations extends Azure.ResourceManager.Operations {}
```

# Swagger breaking change

## question 
PR check run: https://github.com/Azure/azure-rest-api-specs/pull/35346/checks?check_run_id=44501918225
My typespec conversion PR is failing in swagger breaking change check with the following error:
 
"new":"https://github.com/Azure/azure-rest-api-specs/blob/a1ac3f6f98bb1ea3583b765d00dfceab6d85654f/specification/devtestlabs/resource-manager/Microsoft.DevTestLab/stable/2018-09-15/DTL.json",
"old":"https://github.com/Azure/azure-rest-api-specs/blob/main/specification/devtestlabs/resource-manager/Microsoft.DevTestLab/stable/2018-09-15/DTL.json",
"details":"incompatible properties : tags\n definitions/TrackedResource/properties/tags\n at file:///mnt/vss/_work/1/azure-rest-api-specs/specification/common-types/resource-management/v3/types.json#L489:8\n definitions/Resource/properties/tags\n at file:///mnt/vss/_work/1/azure-rest-api-specs/specification/devtestlabs/resource-manager/Microsoft.DevTestLab/stable/2018-09-15/DTL.json#L12397:8"
 
This PR is just having typespec conversion change and I am avoiding any change to the swagger file that is generated as part of typespec conversion. In the old swagger there is a Resource definitionwhich is similar Azure resource definition. Should I suppress this check and how to do it? 

## answer
This occurs because in your original swagger, you did not use swagger common-types.
 
The 'tags' definitions are compatible, so there is no change in the actual api from this.  This could result in changes in some management sdks, although this change to using the common resource types has generally been accepted.
 
docs on suppressions for false positives are here: [Suppress validation failures on a PR](https://eng.ms/docs/products/azure-developer-experience/design/specs-pr-guides/pr-suppressions)
 
Note that it is important to go through these violations, as the conversion is not guaranteed to be 100% accurate.

# Missing APIs in default tag error for typespec conversion PR

## question 
PR: https://github.com/Azure/azure-rest-api-specs/pull/35346
Avocado check: https://github.com/Azure/azure-rest-api-specs/pull/35346/checks?check_run_id=44368323389
 
Is there a way to suppress the missing APIs check only for this PR since it only has typspec conversion but no update to the open API spec?
The check is trying to compare the APIs with previous stable release with currently published stable release.

## answer
Part of the typespec conversion is replacing the existing swagger with a generated swagger.  The generated swagger is still used for some purposes, and Avocado protects the ability to process it. The generated swagger needs to be equivalent, but not necessarily identical.  Generally these issues reflected in the swagger will also show up in breaking change checks, which will have to be resolved (or suppressed if they are false positives)

# Avocado Failing on PR

## question 
Hi TypeSpec Discussion
 
I have a PR here to add a new set of Azure AI APIs: https://github.com/Azure/azure-rest-api-specs/pull/33130
 
I'm a bit confused on why the avocado step is failing? As this is a brand new API, we started from scratch with tsp itself, and these swaggers are the output of `npx tsv` .... Do I really need to include a README.md in the swagger directories, or is this just failing incorrectly? I don't see README.md files in other service directories (i.e. keyvault) for example.

## answer
you need a readme.md somewhere to generate SDKs.  location and factoring of the readme.md(s) can vary by spec.

# Why does the PR bot add the `WaitForARMFeedback` label when TypeSpec validation pipeline fails?

## question 
Hi, not a typespec question per se, but I am curious why the PR bot now adds the WaitForARMFeedback label even when required checks fail? If I'm remembering correctly, this didn't used to be the case.
 
And now that it does add the label, it seems like the reviewer will typically manually say "Fix X pipeline check." and then switch it to "ARMChangesRequested".
 
Would it be possible for that to happen automatically? I feel bad for wasting the reviewer's time if I don't sit around and wait for the pipeline to fail and then manually remove the label myself. 
 
Or is the expectation that a commit shouldn't be pushed if I think the required checks may fail?

## answer
The WaitForARMFeedback label being added even when required checks fail is intended behavior and has always worked this way, according to the ARM review team.

Ideally, contributors should open a draft PR first and only mark it "ready for review" after all required checks pass. This avoids wasting reviewer time.

The suggestion to automatically change the label to ARMChangesRequested when checks fail is a good idea and is already on the backlog.

The engineering team is currently migrating the labeling system to GitHub Actions, which should make improvements like this easier in the future.
