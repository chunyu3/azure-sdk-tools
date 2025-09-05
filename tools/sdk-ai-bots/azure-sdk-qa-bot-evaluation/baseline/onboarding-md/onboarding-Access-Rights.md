# Have joined Microsoft and Azure organization on GitHub but Validate-AzsdkCodeOwner still fails

## question 
Joined the 2 organization using links below, is there a delay in reflection of permissions?
Join the [Microsoft](https://repos.opensource.microsoft.com/orgs/Microsoft) organization on GitHub.
Join the [Azure](https://repos.opensource.microsoft.com/orgs/Azure) organization
PS C:\Users\vikottur\Documents> .\validate.ps1 vikramkotturu
 
Required Orginizations:
	x Microsoft
	x Azure
 
Required Permissions:
	✓ write
 
 
Validation result for 'vikramkotturu':
	x Not a valid code owner

## answer
The required orgs might still be getting marked as invalid if your membership to those orgs is private. You need to be a public member of the Microsoft and Azure Orgs. 

# Request for public GitHub organization memberships and permissions required of an internal contributor

## question 
Hi Azure SDK Onboarding

I have created this PR for Azure recovery services [ASR April SDK release by vidyadharijami · Pull Request #49302 · Azure/azure-sdk-for-net](https://github.com/Azure/azure-sdk-for-net/pull/49302)

I got the comment as below, could you please guide me what to do for this as the documentation is not clear.

Your account lacks the public GitHub organization memberships and permissions required of an internal contributor.

## answer
Hi have you gone over the "Azure SDK get access" doc ?
[Request access to Azure REST API and SDK repositories](https://eng.ms/docs/products/azure-developer-experience/onboard/access?tabs=write-access)
 
You need to join the orgs and request to join the different teams (ex: azure-sdk-read team, Azure SDK Partners etc)

# Partner needs Release Planner access

## question 
Hi Azure SDK Onboarding, our partner Red Hat is not able to access Release Planner here - https://apps.powerapps.com/play/e/ed2ffefd-774d-40dd-ab23-7fff01aeec9f/a/821ab569-ae60-420d-8264-d7b5d5ef734c?tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47&source=sharebutton&sourcetime=1749159050561. How can they access it? They have b- accounts. They are the ones developing this service. 

## answer
Vendors must join the Azure SDK Power Apps Vendors (azuresdk-powerapps-v@microsoft.com) Security Group on ID Web to access Release Planner.
