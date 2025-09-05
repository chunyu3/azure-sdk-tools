# Publish .Net SDK to support new API version for Microsoft.OperationalInsights RP

## question 
Hi team, 
I'm Roi, team member of the RP owners of Microsoft.OperationalInsights.
 
Lately we've released a new swagger version of our APIs, 2025-02-01. As part of our support of a new feature that becoming GA in this version, I required some assistance in publish a new .NET SDK version. 
 
Couldn't find exactly the process that required from us to deploy a new version. If anyone could give me some guidance or TSG to follow, this will be superb. 
 
Thanks,
Roi

## answer
have you followed the process of creating a release plan yet? It will walk you step by step through the process. Here's our documents on how to do this, [Create a new release plan](https://eng.ms/docs/products/azure-developer-experience/plan/release-plan-create)
You should work on each milestone. The release plan is meant to be a guide for you, so you know the order and the steps you need to follow to get SDKs released

# Private Preview PLR: Requesting an earlier slot for informational meeting on the SDK release plan

## question 
Hi Azure SDK Onboarding team,
Our service is marching towards the Private Preview and our launch date is planned for July 30th. Onboarding using release planner has been completed for our service. As part of Private Preview PLR checklist, we have the below 2 pending items related to SDK onboarding:
 
Data plane Private Preview API review & approval
Management plane Private Preview API review & approval
 
Our service do not have any Data plane APIs and we have management plane APIs for which, we are not planning to release any SDKs for Private Preview (We plan to release the public SDKs as part of GA PLR). However, to mark the above 2 PLR items for N/A & completion respectively, we need to first create a release plan which is dependent on completing the informational meeting on the SDK release plan. The earliest slot I could find for the informational meet is on Aug 4th. Given that our service's Private Preview announcement date is July 30th, wanted to check if it's possible to get an earlier slot for the informational meeting (or do it offline?) or if it's possible to create a release plan first and then complete the informational meeting. Please suggest if there is a way to get unblocked on this for our Private Preview.
 
Service Tree Name: Azure Workloads Hub
Service Tree Oid: 33bc1e69-7c8c-4576-a492-eb735e0e7510
 
Thanks!

## answer
I just unblocked you for the informational meeting. Please let me know if you have specific questions and we can handle them async. For Private Preview you can submit your data plane KPI as N/A. You can do the same for Public Preview and GA. For mgmt plane private preview, you need to complete a release plan for API Spec Readiness. SDK Generation is not required for Public Preview.

# Will .Net SDK team work on this request? https://github.com/Azure/sdk-release-request/issues/6326

## question 
This release request is created from Release Plan.
[[resource manager] .NET: Release for Azure Carbon Optimization - 2025-04-01 · Issue #6326 · Azure/sdk-release-request](https://github.com/Azure/sdk-release-request/issues/6326)
 
Will .Net SDK team  work on this request? Or we will need to update it by ourself?

## answer
The GH issue is for our team's tracking purposes. 
 
You still need to generate the .NET SDK. We have added DevOps automation pipelines that generate the SDK automatically. You should try with it first as if it succeeds it will create a PR in the .NET repo with the changes. Here is the documentation: [Using the SDK generation pipelines (management plane)](https://eng.ms/docs/products/azure-developer-experience/develop/sdk-generation-pipelines)

# Can we release SDK in a private preview ?

## question 
Hello team, I'm trying to release SDK for our Disconnected Operations Service, but currently the API specs is in private repo so I'm unable to move forward as it says PR must be on public repo. Is there any way to release SDK in private preview? 
And If not how I can edit the PR link without creating new release plan after moving the API specs to public repo ?

## answer
Release planner does not support generating and releasing SDK from spec in private repo. You can update the API spec pull request link in API readiness milestone once you move your API spec to public repo and use the same release plan.

# GA request for Microsoft.Fabric .NET SDK

## question 
Hi Team,
 
I’m with the Fabric MWC team, which owns Microsoft.Fabric. We’re planning to move the .NET SDK to GA and would like to confirm what’s needed from the RP side.
 
Here is the NuGet link: [NuGet Gallery | Azure.ResourceManager.Fabric 1.0.0-beta.2](https://www.nuget.org/packages/Azure.ResourceManager.Fabric/1.0.0-beta.2#versions-body-tab)
 
Thanks!

## answer
To move the Microsoft.Fabric .NET SDK to GA, you need to start and complete a GA release plan. The release plan you previously linked is for Public Preview and includes .NET, but it appears that a GA release plan was never started or completed for Microsoft.Fabric. Each release stage—Private Preview, Public Preview, and GA—should have its own release plan covering all tier 1 languages (.NET, Java, JavaScript, Python, and Go). Since the other SDKs are already released as GA, you can justify that in the new GA release plan for .NET.
You’ve now created a GA release plan for .NET SDK and are working through the milestones. At the step where the screen indicates APIView approval is needed, you should update the SDK version and changelog, then submit a new PR. Once the PR is merged, you can kick off the release pipeline for the SDK release to the third-party package management platform. Be sure to attach the release pipeline so the release plan status will be refreshed upon execution.

# Is a release plan required for a new SDK release if the service is already in GA?

## question 
Hello! Our current service, Azure Device Registry, is already generally available and we have completed the GA release plan for our SDKs. We are now developing a new version of the SDK that will include some Resource Types in Public Preview, while other types remain in GA. Do we need to create a release plan for this SDK release?
 
Additionally, CPEX informed us that since the new features (the new RTs) do not have a separate deployment pipeline and will not be available in a subset of the regions where ADR is available, CPEX is not tracking the completion of their criteria for this release. Since it is not being tracked by CPEX, do we still need the release plan for the SDK?

## answer
Yes, a release plan is required for any SDK release—even if the service is already in GA.
The overhead of a release plan is very minimal. Alternatively, you could use our new SDK Agent, which will create and update the release plan on your behalf.
Since your intent is to release a Beta SDK for Public Preview that introduces new resource types, you should create a Public Preview/Beta SDK release plan. Even though the existing service is GA, the new resource types are in Public Preview, so the Product's lifecycle phase in the release plan should be Public Preview.
