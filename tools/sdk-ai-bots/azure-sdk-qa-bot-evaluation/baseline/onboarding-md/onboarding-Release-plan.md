# Untitled

## question 
Hi All,
 
We are trying to work through the sign off process for Management Plane SDK readiness and are a little stumped here. It says we are required to link to a public API spec, but our public preview launch is on 5/19, and we feel it is still a bit early to have a public API spec. Any guidance on how to proceed? We are only a few weeks from launch and it's critical that we get this CPEX requirement over the finish line to complete overall sign off for launch.  Thanks in advance!

## answer
The only way to get your SDKs generated is to have your spec in the public repo. I see two options:
1. Release your API spec “early” by putting it in the public repo and get your SDKs generated on time
2. Request an extension for the management plane API and SDK release readiness KPI, see these docs for how to request one properly  https://eng.ms/docs/products/azure-developer-experience/onboard/request-exception

# Question about concrete release date

## question 
Hi Azure SDK Onboarding, when I create the release plan, about the concrete release date, I only can see the target release month. I can not find the concrete day.  My question is for the July release month, what is the concrete date? Is it the beginning of July or the end of July? so want to know the concrete date?  The same question for June release target month

## answer
It depends in multiple factors so it is better to look at: [Release SDK](https://eng.ms/docs/products/azure-developer-experience/develop/sdk-release/sdk-release)
Look at the `When to release` section

# Release plan for SQL ARM API version 2024-08-01-preview into Azure SDK

## question 
Hi Azure SDK Onboarding,

I'm not sure if this is the correct channel, but I'm working on an addition to the ARM API for SQL Managed Instance product and the API has been released, but I'm interested about the info regarding the downstream of that API version to the Azure SDK and Powershell API. I found this PR: [[SQL] Release api version 2024-05-01-preview by HarveyLink · Pull Request #49836 · Azure/azure-sdk-for-net](https://github.com/Azure/azure-sdk-for-net/pull/49836) in the Azure-SDK repo, and I was interested if the same process will occur for 2024-08-01-preview version, and is anything required of me as the engineer?

A follow-up question is, can I introduce the changes from te API to the PowerShell client, even though the Azure SDK doesn't yet have the 2024-08-01-preview version released?

Thanks,
Uros

## answer
Did you create a release plan? You need to create a release plan and it will guide you step by step to generate your SDKs
[What is a release plan?](https://eng.ms/docs/products/azure-developer-experience/plan/release-plan):
```
What is a release plan?
A release plan is a guided workflow that you can create to track an upcoming REST API and SDK release.

How it works
Let's say your product team is working towards a new feature or REST API version. With Release Planner, you can build a guided workflow for the REST API and SDK tasks that your team must complete to obtain Cloud Product Excellence (CPEX) sign-off.

For example, consider the following scenario:

1.As an Azure service engineer, you want to know what tasks are required to release a new REST API version to customers. You log in to Release Planner and create new plan.
2.Release Planner connects to Service Tree to get details about your service, and creates a workflow that's specific to your scenario.
3.You link the pull request that contains your latest REST API spec updates. Release Planner lets you know when it's time to schedule a review, fix validation issues, request sign-offs, and more.
4.You then get information on how to generate, test, release, and get approval of your SDKs.

Milestones and tasks
A release plan consists of milestones. A milestone is a bucket of related tasks to complete. Depending on your product's specific scenario, the milestones and tasks will vary. The major two milestones are API Readiness and SDK release

Next steps
Create a release plan
```

# Update required for Release Plan Mgmt SDK

## question 
Created the release plan a while ago, milestones and API PR has changed in the mean time - https://aka.ms/sdk-release-planner?release-plan-id=6ee1c60e-24b6-ee11-a569-000d3a3418aa
 
What is the process to change changes to a release plan?

## answer
You can change the API spec multiple times. It only becomes tricky if you've already requested work from the SDK team. Looking at your release plans, it seems you did, but something may have happened with that request. If there are a lot of changes, I recommend following James' advice: abandon your current release plan and create a new one. It shouldn't take long, and you can include your edited API spec in the new plan.
