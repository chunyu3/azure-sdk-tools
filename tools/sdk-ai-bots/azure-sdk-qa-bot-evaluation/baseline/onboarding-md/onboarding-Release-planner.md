# Edit API Specs in Release Planner

## question 
Azure SDK Onboarding I need to edit my API Specs PR Link in my [release planner](https://aka.ms/sdk-release-planner?release-plan-id=0731ed29-db37-f011-8c4e-6045bd06bb0b). I had to make some small changes because of the namespace board review. Is this possible, or do I need to create another release plan?
 
Additionally, I will be releasing underneath the AKS product, but my service has it's own typespec and openapi specs. Will this be an issue?

## answer
You can update the link in your existing release plan, but it will override subsequent steps if you’ve progressed beyond that step already

# Release planner 'Service Contacts'

## question 
I created a [release plan](https://aka.ms/sdk-release-planner?release-plan-id=87e2b5c5-7d15-f011-9989-000d3a34671f) for our Managment API & SDK, however it auto-populated the Service Contacts "Product*" fields with my contact information instead of information from service tree.  
Is this the expected behavior?  I was not able to edit it when creating the release plan (from what I remember)

## answer
```
Service contacts
APEX PM
dfulcer@microsoft.com
Product primary PM
[pencil icon] Carl Ochs
Product Engineering Lead
[pencil icon] Carl Ochs
```
It will fill the contact info with whoever is creating it. Feel free to edit the contacts on the summary page if this is not the desired contact by clicking the pencil icon.

# Got error when trying to create ARM SDK release plan

## question 
Hi, I'm from Azure Confidential Ledger team and I am working on releasing SDKs for our RP version 2024-09-01.
I think this is our first time using "Azure SDK Release Planner" to release ARM SDKs.
 
Started with "Create a release plan" and then I selected "Azure Confidential Ledger -Offering" and "GA" for Product lifecycle
No issue, but when I further select target API Scope "Management plan (ARM)", I got error:
"Onboarding records do not target management plane for this product
Contact SDK Onboarding PM to confirm the product requires a management plane release plan."

```
Azure SDK Release Planner

REST API details

Specify the REST API spec scope to be released.

API scope
Management plane (ARM)
API spec release type

API version target publishing date
[Select a date...]

Onboarding records do not target management plane for this product
Contact SDK Onboarding PM to confirm the product requires a management plane release plan.
```
I couldn't figure out the contact of "SDK Onboarding PM" but seems this channel is about SDK Onboarding. Can you please give any instructions what should be done?

## answer
From our records, `Musabbir Khan` onboarded the product `Azure confidential ledger - Offering` in Oct - 24 and selected `No` for management plane scope.
that is why you can't create a release plan.
 
If this is not the case, you can go ahead to `Onboard your product` and update your records by onboarding again. 
 
Once you have updated the product, then you will be able to create the release plan

# Management plane release plan needing updates

## question 
I have a release plan [Azure SDK Release Planner - Power Apps](https://apps.powerapps.com/play/e/ed2ffefd-774d-40dd-ab23-7fff01aeec9f/a/821ab569-ae60-420d-8264-d7b5d5ef734c?release-plan-id=b1e8b8ef-7eb4-ee11-a569-000d3a3411c3&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47) and I am trying to go through the prerequisite checks for SDK release, however the 'API spec' under 'API specs' is incorrect (private repo when it should be public repo). How do I update that value? Or do I need to create a new release plan?

## answer
You need to update your release plan to use the public repo PR. To do so, go to the `Management plane API readiness` -> `Associate a pull request`

# Release planner - SDK Release summary shows as pending with all Prs completed

## question 
Hi Azure SDK Onboarding team,

We have a release plan ready but I am not able to complete it since the Plan shows the release status as pending. Will the status be marked as released once the packages are released ? 
If yes, can you please share the release dates for the remaining packages ?
We need to complete the release plan to acquire attestation for this KPI in CLC.  
 
Release plan: https://aka.ms/sdk-release-planner?release-plan-id=ea22ea4f-c751-f011-877a-000d3a5b0147

```
Release summary
Plan: Release #1795
Target Date: July 2025
After your packages are released, got to the public feed and verify that the links, change logs, and release notes are correct and appear as expected. Download the packages and verify that they match what you built and published. You can find Azure SDK releases here.

Language    Package name    Release status
.NET    Azure.ResourceManager.HardwareSecurityModules    Released
Go    Azure.ResourceManager.HardwareSecurityModules    Released
Java    Azure.ResourceManager.HardwareSecurityModules    Pending
Python    Azure.ResourceManager.HardwareSecurityModules    Pending
JavaScript    Azure.ResourceManager.HardwareSecurityModules    Pending
```

## answer
You are in charge of releasing the SDKs. and it looks like everything is ready for u to it. In the Release SDKs Task -> Release SDK has the information to release
