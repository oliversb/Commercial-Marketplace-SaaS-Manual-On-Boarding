## Deploying the solution to an Azure subscription

You can use one of the options below to deploy the solution
1- If you already have Azure Active Directory (AAD) app registrations
2- Use a local script to create AAD app registrations and deploy the solution

### Option 1

Please use this option, if you already have app registrations created and would like to create an App Service, Storage account and Deploy the code to App Service.

The template uses the following parameters:

| Parameter                         | Note                                                                                                                                     |
| --------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| Web App Name                      | The DNS subdomain of the WebApp                                                                                                          |
| Web App Admin                     | The account name for the admin console, corresponding to CommandCenter:CommandCenterAdmin app setting                                    |
| App Service Plan Sku              | The SKU of App Service Plan                                                                                                              |
| Login App Reg Domain Name         | Domain name from the AAD app registration for the landing page AAD SSO, corresponding to AzureAd:Domain app setting                      |
| Login App Reg Client Id           | Client ID for the AAD app registration for the landing page AAD SSO, corresponding to AzureAd:AzureAd:ClientId app setting               |
| Login App Reg Client Secret       | Client secret for the AAD app registration for the landing page AAD SSO, corresponding to AzureAd:ClientSecret app setting               |
| Fulfillment App Reg Tenant Id     | Tenant ID for the AAD app registration for calling the marketplace APIs, corresponding to MarketplaceClient:TenantId app setting         |
| Fulfillment App Reg Client Id     | App ID for the AAD app registration for calling the marketplace APIs, corresponding to MarketplaceClient:ClientId app setting            |
| Fulfillment App Reg Client Secret | Client secret for the AAD app registration for calling the marketplace APIs, corresponding to MarketplaceClient:ClientSecret app setting |

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FCommercial-Marketplace-SaaS-Manual-On-Boarding%2Fdev%2Fresources%2Fdeploy%2FdeployWithExistingAADApps.json)

### Option 2

Please use this option, to Create App registrations using the Powershell script, App Service, Storage account with ARM template and Deploy the code to App Service.

```powershell
md landinpageappdeploy #create a staging directory

cd landinpageappdeploy #go to the new staging directory

curl -o deploy.ps1 https://raw.githubusercontent.com/santhoshbomma9/landingpage-deploy-automation/main/deploy.ps1  # pull deploy ps file

curl -o mainTemplate.json https://raw.githubusercontent.com/santhoshbomma9/landingpage-deploy-automation/main/mainTemplate.json  # pull template json file

Connect-AzureAD #connect to you azure account

.\deploy.ps1 #run the deploy file
```

### Option 3

Please use this option, if you like to Create App registrations, App and Storage account using ARM template and Deploy the code to App Service.

This option will require a User Managed Identity with the right AzureAD roles permissions.

[Creating a User Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/how-to-manage-ua-identity-portal)

Required permissions below:
![Create custom role with these permissions and assigned to the User Managed Identity](../../ReadmeFiles/azure-ad-role-permissions.png)
Pass this User Managed Identity Resource ID to the below ARM template deployment

[![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fsanthoshb-msft%2FCommercial-Marketplace-SaaS-Manual-On-Boarding%2Fmain%2Fresources%2Fdeploy%2Fazuredeploy.json)

---

### Sample Architecture

Below sample architecture could be one way of running the sample app in your Azure instance in a Cloud optimized way.

![Architecture Overview and Process Flow of the Solution](../../ReadmeFiles/saas-samplesdk-architecture.png)

</hr>
