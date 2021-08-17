![Build .NET Core](https://github.com/Azure-Samples/Commercial-Marketplace-SaaS-Manual-On-Boarding/workflows/Build%20.NET%20Core/badge.svg)

![Deploy to App Service](https://github.com/microsoft/Commercial-Marketplace-SaaS-Manual-On-Boarding/workflows/Deploy%20to%20App%20Service/badge.svg)

# Microsoft commercial marketplace SaaS offers sample - Manual on-boarding of customers

<!--
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

Some solutions require out-of-band or manual on-boarding steps, such as validating a customer, running scripts manually for deploying resources needed for a new customer etc. This sample uses notifications for a new customer, or any changes on the subscription status made outside of the solution code.

**Please note the sample uses a [personal NuGet package](https://www.nuget.org/packages/Ercenk.Microsoft.Marketplace) based on the preview version of [.NET client library for Commercial Marketplace](https://github.com/microsoft/commercial-marketplace-client-dotnet).**

You can also find a short
[video published on the Azure Friday channel](https://www.youtube.com/watch?v=2Oaq5dHczMY)
to see the experience and a brief explanation.

## Prerequisites

The sample requires .NET 5.\*.\*, and an Azure Storage account.

## Table of contents

In the sections below you will find:

- [Microsoft commercial marketplace SaaS offers sample - Manual on-boarding of customers](#microsoft-commercial-marketplace-saas-offers-sample---manual-on-boarding-of-customers)
  - [Prerequisites](#prerequisites)
  - [Table of contents](#table-of-contents)
- [Integrating a software as a service solution with commercial marketplace](#integrating-a-software-as-a-service-solution-with-commercial-marketplace)
  - [Landing page](#landing-page)
    - [Azure AD Requirement: Multi-Tenant Application Registration](#azure-ad-requirement-multi-tenant-application-registration)
  - [Webhook endpoint](#webhook-endpoint)
  - [Marketplace REST API interactions](#marketplace-rest-api-interactions)
    - [Azure AD Requirement: Single-Tenant Registration](#azure-ad-requirement-single-tenant-registration)
  - [Activating a Subscription](#activating-a-subscription)
- [Scenario for the Sample](#scenario-for-the-sample)
  - [Architecture Overview and Process Flow of the Solution](#architecture-overview-and-process-flow-of-the-solution)
  - [Walking-through the Scenario Subscription Process](#walking-through-the-scenario-subscription-process)
- [Running the Sample](#running-the-sample)
  - [Quick deployment](#quick-deployment)
    - [Option 1](#option-1)
    - [Option 2](#option-2)
  - [Detailed deployment steps](#detailed-deployment-steps)
    - [Creating a web application on Azure App Service and deploying the sample with Visual Studio](#creating-a-web-application-on-azure-app-service-and-deploying-the-sample-with-visual-studio)
    - [Creating a web application on Azure App Service and deploying the sample with Visual Studio Code](#creating-a-web-application-on-azure-app-service-and-deploying-the-sample-with-visual-studio-code)
    - [Registering Azure Active Directory Applications](#registering-azure-active-directory-applications)
      - [Creating a New Directory](#creating-a-new-directory)
      - [Registering the Apps](#registering-the-apps)
    - [Creating a Storage Account](#creating-a-storage-account)
    - [Change the Configuration Settings](#change-the-configuration-settings)
  - [Create an Offer on Commercial Marketplace Portal in Partner Center](#create-an-offer-on-commercial-marketplace-portal-in-partner-center)
    - [Example Offer Setup in Commercial Marketplace Portal](#example-offer-setup-in-commercial-marketplace-portal)
      - [Offer Setup](#offer-setup)
      - [Properties](#properties)
      - [Offer Listing](#offer-listing)
      - [Preview Audience](#preview-audience)
      - [Technical Configuration](#technical-configuration)
      - [Plan Overview](#plan-overview)
      - [Co-Sell with Microsoft](#co-sell-with-microsoft)
      - [Resell Through CSPs](#resell-through-csps)
      - [Review and Publish](#review-and-publish)
  - [Signing Up for Your Offer](#signing-up-for-your-offer)

  - [Quick deployment](#quick-deployment)
    - [Option 1](#option-1)
    - [Option 2](#option-2)
  - [Detailed deployment steps](#detailed-deployment-steps)

    - [Creating a web application on Azure App Service and deploying the sample with Visual Studio](#creating-a-web-application-on-azure-app-service-and-deploying-the-sample-with-visual-studio)
    - [Creating a web application on Azure App Service and deploying the sample with Visual Studio Code](#creating-a-web-application-on-azure-app-service-and-deploying-the-sample-with-visual-studio-code)
    - [Registering Azure Active Directory Applications](#registering-azure-active-directory-applications)
      - [Creating a New Directory](#creating-a-new-directory)
      - [Registering the Apps](#registering-the-apps)
    - [Creating a Storage Account](#creating-a-storage-account)
    - [Change the Configuration Settings](#change-the-configuration-settings)

  - [Create an Offer on Commercial Marketplace Portal in Partner Center](#create-an-offer-on-commercial-marketplace-portal-in-partner-center)
    - [Example Offer Setup in Commercial Marketplace Portal](#example-offer-setup-in-commercial-marketplace-portal)
      - [Offer Setup](#offer-setup)
      - [Properties](#properties)
      - [Offer Listing](#offer-listing)
      - [Preview Audience](#preview-audience)
      - [Technical Configuration](#technical-configuration)
      - [Plan Overview](#plan-overview)
      - [Co-Sell with Microsoft](#co-sell-with-microsoft)
      - [Resell Through CSPs](#resell-through-csps)
      - [Review and Publish](#review-and-publish)
  - [Signing Up for Your Offer](#signing-up-for-your-offer)

Let's first start with mentioning how to integrate a SaaS solution with Azure
Marketplace.

# Integrating a software as a service solution with commercial marketplace

Many different types of solution offers are available on Microsoft commercial marketplace for
the customers to subscribe. Those different types include options such as
virtual machines (VMs), solution templates, and containers, where a customer can
deploy the solution to their Azure subscription. Commercial marketplace also provides
the option to subscribe to a _Software as a Service (SaaS)_ solution, which runs
in an environment other than the customer's subscription.

A SaaS solution publisher needs to integrate with the marketplace commerce
capabilities for enabling the solution to be available for purchase.

Commercial marketplace talks to a SaaS solution on two channels:

- [Landing Page](#landing-page): The marketplace sends the subscriber to
  this page maintained by the publisher to capture the details for provisioning
  the solution for the subscriber. The subscriber is on this page for activating
  or modifying the subscription.
- [Webhook](#webhook-endpoint): This is an endpoint where the marketplace
  notifies the solution of events, such as subscription cancellation or
  modification, or a suspend request for the subscription, should the customer's
  payment method become unusable.

The SaaS solution in turn uses the REST API exposed on the marketplace
side to perform corresponding operations. Those can be activating, cancelling,
or updating a subscription.

To summarize, we can talk about three interaction areas between the Azure
Marketplace and the SaaS solution,

1. Landing page
2. Webhook endpoint
3. Marketplace REST API interactions

![overview](./ReadmeFiles/IntegrationOverview.png)

## Landing page

On this page, the subscriber provides additional details to the publisher so the
publisher can provision required resources for the new subscription. A publisher
provides the URL for this page when registering the offer for commercial marketplace.

The publisher can collect other information from the subscriber to onboard the
customer, and provision additional resources as needed. The publisher's solution
can also ask for consent to access other resources owned by the customer, and
protected by AAD, such as Microsoft Graph API, Azure Management API, etc.

> **:warning: IMPORTANT:** the subscriber can access this page after subscribing
> to an offer to make changes to his/her subscription, such as upgrading,
> downgrading, or any other changes to the subscription from Azure portal.

### Azure AD Requirement: Multi-Tenant Application Registration

This page should authenticate a subscriber through Azure Active Directory (AAD)
using the
[OpenID Connect](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc)
flow. The publisher should register a multi-tenant AAD application for the
landing page.

## Webhook endpoint

The commercial marketplace calls this endpoint to notify the solution for the events
happening on the marketplace side. Those events can be the cancellation or
modification of the subscription through commercial marketplace, or suspending it
because of the unavailability of a customer's payment method. A publisher
provides the URL for this webhook endpoint when registering the offer for Azure
Marketplace.

> **:warning: IMPORTANT:** This endpoint is not protected. The implementation
> should call the marketplace REST API to ensure the validity of the event. This endpoint also receives a JWT token. You can validate the token against AAD, and check the audience to make sure this call is addressed to you. Please see the startup.cs file to see the implementation.

## Marketplace REST API interactions

The _Fulfillment API_ is documented
[here](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/pc-saas-fulfillment-api-v2)
for subscription integration, and the usage based _Metered Billing API_
documentation is
[here](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/marketplace-metering-service-apis).

### Azure AD Requirement: Single-Tenant Registration

The publisher should register an AAD application and provide the `AppID`
(ClientId) and the `Tenant ID` (AAD directory where the app is registered)
during registering the offer for the marketplace.

The solution is put on an [access control list](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow#access-control-lists) so it can call the marketplace REST API with
those details. A client must use [client credentials grant to get a token](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow#get-a-token).

> **:warning: Important:**
> The marketplace fulfillment API V2.0's resource ID is `20e940b3-4c77-4b0b-9a53-9e16a1b010a7`.

If you are using the V1 end point for AAD, use the value `20e940b3-4c77-4b0b-9a53-9e16a1b010a7` for the resource parameter. If you are using AAD V2 endpoint (recommended), use `20e940b3-4c77-4b0b-9a53-9e16a1b010a7/.default` for value of the scope parameter.

Please note the different requirements for the Azure AD interaction for the
landing page and calling the APIs. I recommend two separate AAD applications,
one for the landing page, and one for the API interactions, so you can have
proper separation of concerns when authenticating against Azure AD.

This way, you can ask the subscriber for consent to access his/her Graph API,
Azure Management API, or any other API that is protected by Azure AD on the
landing page, and separate the security for accessing the marketplace API from
this interaction as good practice. The certification policy requires the use of "User.Read" for signing on the user, and [incremental consent](https://docs.microsoft.com/en-us/azure/active-directory/azuread-dev/azure-ad-endpoint-comparison#incremental-and-dynamic-consent) pattern should you need to request other permissions.

## Activating a Subscription

Let's go through the steps of activating a subscription to an offer.

![AuthandAPIFlow](./ReadmeFiles/Auth_and_API_flow.png)

1. Customer subscribes to an offer on commercial marketplace.
2. Commerce engine generates a marketplace token for the landing page. This is
   an opaque token, unlike a JSON Web Token (JWT) that is returned when
   authenticating against Azure AD, and does not contain any information. It is
   just an index to the subscription and used by the resolve API to retrieve the
   details of a subscription. This token is available when the user clicks
   "Configure Account" for an inactive subscription or "Manage Account" for an
   active subscription.
3. Customer clicks on "Configure Account" (new and not activated subscription)
   or "Manage Account" (activated subscription) and accesses the landing page.
4. Landing page asks the user to logon using Azure AD
   [OpenID Connect](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc)
   flow.
5. Azure AD returns the `id_token`. There needs to be additional steps for
   validating the `id_token` — just receiving an `id_token` is not enough for
   authentication. Also, the solution may need to ask for authorization to
   access other resources on behalf of the user. We are not covering them for
   brevity and ask you to refer to the related Azure AD documentation.
6. Solution asks for an access token using the
   [service-to-service access token request](https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-oauth2-client-creds-grant-flow#service-to-service-access-token-request)
   of the client credential workflow to be able to call the API.
7. Azure AD returns the access token.
8. Solution prepends `"Bearer "` (notice the space) to the access token, and
   adds it to the `Authorization` header of the outgoing request. We are using
   the marketplace token previously received on the landing page to get the
   details of the subscription using the resolve API.
9. The subscription details are returned.
10. Further API calls are made, again using the access token obtained from the
    Azure AD, in this case to activate the subscription.

# Scenario for the Sample

This sample can be a good starting point, assuming the solution does not have
requirements of providing a native experience for cancelling or updating a
subscription by a customer.

It exposes a landing page that can be customized for branding. It provides a
webhook endpoint for processing the incoming notifications from the Azure
Marketplace. It also provides a privacy policy and support page to meet the
partner center requirements. The rest of the integration is done via notifications.

The landing page can also used for adding new fields to gather more information
from the subscriber; for example: what is the favored region. When a subscriber
provides the details on the landing page, the solution generates a notification to the
configured operations contact. The sample implements a mechanism for Azure storage queue notifications. The operations team then provisions the required
resources, on-boards the customer using their internal processes, and then comes
back to the generated notification and accesses the URL to activate the
subscription.

Please see my overview for the integration points in
[Integrating a software as a service with Microsoft commercial marketplace](#integrating-a-software-as-a-solution-with-azure-marketplace).

## Architecture Overview and Process Flow of the Solution

![Architecture Overview and Process Flow of the Solution](./ReadmeFiles/Overview.png)

Remember, this scenario is useful when there is a human element in the mix, for
situations such as:

- A script needs to be run manually for provisioning resources for a new
  customer as part of the onboarding process.
- A team needs to qualify the purchase of the customer for reasons like ITAR
  certification, etc.

## Walking-through the Scenario Subscription Process

1. The prospective customer is on the Azure Portal going through the Azure
   Marketplace in-product experience. They find the solution and subscribe to it
   after deciding on a plan. A placeholder resource is deployed on the
   customer's (subscriber's) Azure subscription for the new offer subscription.
   _Note:_ Please notice the overloaded use of the "subscription", there are two
   subscriptions at this moment, the customer's Azure subscription and the
   subscription to the SaaS offer. I will use **subscription** only when I refer
   to the subscription to the offer from now on.
2. Subscriber clicks on the **Configure Account** button on the new
   subscription, and gets transferred to the landing page.
3. Landing page uses Azure Active Directory (with OpenID Connect flow) to log
   the user on.
4. Landing page uses the client library to resolve the subscription to get the details,
   using the marketplace token on the landing page URL token parameter.
5. Client library gets an access token from Azure Active Directory (AAD).
6. Client library calls **resolve** operation on the Fulfillment API, using the access
   token as a bearer token.
7. Subscriber fills in the other details on the landing page that will help the
   operations team to kick of the provisioning process. The landing page asks
   for a deployment region, as well as the email of the business unit contact.
   The solution may be using different data retention policies based on the
   region (e.g. GDPR in Europe), or the solution may be depending on a
   completely different identity provider (IP), such as something in-house
   developed, and may be sending an email to the business unit owner asking them
   to add the other end users to the solution's account management system.
   Please keep in mind that the person subscribing, that is the purchaser
   (having access to the Azure subscription) can be different than the end users
   of the solution.
8. Subscriber completes the process by submitting the form on the landing page.
   This sends notification using the configured notification handler.
9. Operations team takes the appropriate steps (qualifying, provisioning
   resources, etc.).
10. Once complete, operation team clicks on the activate link in the message.
11. The sample uses the client library to activate the subscription.
12. Client library gets an access token from Azure Active Directory (AAD).
13. Client library calls the `activate` operation on the Fulfillment API.
14. The subscriber may eventually unsubscribe from the subscription by deleting
    it, or may stop fulfilling their monetary commitment to Microsoft.
15. The commerce engine sends a notification on the webhook at this time, to
    notify the publisher know about the situation.
16. The sample sends a notification to the operations team, notifying the team about
    the status.
17. The operations team may de-provision the customer.

# Running the Sample

Although we recommend you to take the long way to deploy the sample for seeing the components in operation, we also provide easier options for deploying the solution to your Azure subscription.

The solution consists of the following resources

![Architecture Overview and Process Flow of the Solution](./ReadmeFiles/saas-samplesdk-architecture.png)

Please see the following sections for

- [Quick deployment](#quick-deployment)
- [Detailed deployment steps](#detailed-deployment)

## Quick deployment

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

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FCommercial-Marketplace-SaaS-Manual-On-Boarding%2Fmain%2F%2Fdeployment%2Fazuredeploy.json)

### Option 2

If you do not have an AAD app registrations yet, this option includes a script to create the app registrations, and deploys the resources using the template.

Open a PowerShell session, and login to your Azure subscription, then change the current directory to the "deployment" directory of this repo.

> **:warning: IMPORTANT:** The script uses cmdlets from AzureAD module. AzureAD module is not compatible with PowerShell 7+/Core, and supported in PowerShell 5.\*. If you have not have the module already, install it as shown below:

```powershell
Install-Module -Name AzureAD
```

```powershell
cd deployment

# You may need to import AzureAD module, and AzureAD module supports PowerShell 5.x only.

Import-Module AzureAD

#connect to the Azure AD, note if you want to add app registrations to a different tenant, you may specify it with TenantId parameter

Connect-AzureAD

# Login to your Azure subscription separately

Connect-AzAccount

# Optional: get a list of Azure locations, if you want to override the default deployment location, and pass it with the Location parameter of the script.
Get-AzLocation

# run the deploy file.

# **** Important!: WebAppName is the DNS subdomain of the WebApp and it needs to be unique.

.\deploy.ps1 -ResourceGroupName 'rgGroupName' -WebAppName 'mktplc1' -AppAdmin 'user@domain.com'
```

## Detailed deployment steps

We will go through the detailed deployment steps in the coming sections.

### Creating a web application on Azure App Service and deploying the sample with Visual Studio

I am assuming you have already cloned the code in this repo. Open the solution
in Visual Studio, and follow the steps for deploying the solution starting from
this
[step](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-get-started-dotnet#publish-your-web-app).

The following is how my Visual Studio Publish profile looks:

![publishprofile](./ReadmeFiles/PublishProfile.png)

### Creating a web application on Azure App Service and deploying the sample with Visual Studio Code

Make sure you have [Azure Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-node-azure-pack) or [Azure App Service](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice) extension.

> **:warning: Important:**
> Open Visual Studio Code at the src/CommandSenter folder, not at the repo root.

Create a new Web App with the Azure App Service extension.

Deploy to the new Web App with the extension.

Alternatively, follow the [tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-5.0), and modify the process as you see fit.

### Registering Azure Active Directory Applications

I usually maintain a separate Azure Active Directory tenant (directory) for my
application registrations. If you want to register the apps on your default
directory, you can skip the following steps and go directly to
[Registering the Apps](#registering-the-apps).

#### Creating a New Directory

1. Login to the [Azure Portal](https://portal.azure.com).
2. Click `Create a Resource` and type in `Azure Active Directory` in the search
   box:

   ![createdirectory](./ReadmeFiles/createdirectory.png)

   Select `Create Directory` then fill in the details as you see fit after
   clicking the `Create` button

3. Switch to the new directory.

   ![switchdirectory](./ReadmeFiles/switchdirectory.png)

4. Select the new directory, if it does not show under "Favorites" check "All
   directories":

   ![gotodirectory](./ReadmeFiles/gotodirectory.png)

5. Once you switch to the new directory (or if you have not created a new one,
   and decided to use the existing one instead), select the Active Directory
   service (**1** on the image below). If you do not see it, find it using "All
   services" (**2** on the image below).

   ![findactivedirectory](./ReadmeFiles/findactivedirectory.png)

6. Click on "App registrations", and select "New registration". You will need to
   create two apps.

   ![registerappstart](./ReadmeFiles/registerappstart.png)

#### Registering the Apps

As I mentioned in the landing page and webhook sections above, I recommend
registering two applications:

1. **For the Landing Page:** Commercial marketplace SaaS offers are required to have
   a landing page, authenticating through Azure Active Directory. Register it as
   described in the
   [documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-core-webapp#option-2-register-and-manually-configure-your-application-and-code-sample).
   **Make sure you register a multi-tenant application**, you can find the
   differences in the
   [documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/single-and-multi-tenant-apps).
   Select the "ID tokens" on the "Authentication" page. Also, add two Redirect
   URLs: the base `/` URL of the web app and another web app URL with
   `/signin-oidc` added.

2. **To authenticate marketplace fulfillment APIs,** you can register a
   **single tenant application**.

   ![A screenshot of a computer Description automatically generated](./ReadmeFiles/AdAppRegistration.png)

### Creating a Storage Account

Create an Azure Storage account following the steps
[here](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).
The solution uses the storage account to keep references to the operations
returned by actions done on the fulfillment API, as well as offering a place to
store Leads generated from the Marketplace offer (via Table Storage).

### Change the Configuration Settings

You will need to modify the settings with the values for the services you have
created above.

You will need to replace the values marked as `CHANGE`, either by editing the
`appconfig.json` file in the solution, or by using `dotnet user-secrets` if you are planning share your work publicly.

| Setting                                          | Change/Keep | Notes                                                                                                                                                                                                                                                                                                                                                                                                                           |
| ------------------------------------------------ | ----------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AzureAd:Instance                                 | Keep        | This is used by the library                                                                                                                                                                                                                                                                                                                                                                                                     |
| AzureAd:Domain                                   | Change      | You can find this value on the "Overview" page of the Active Directory you have registered your applications in. If you are not using a custom domain, it is in the format of \<tenant name\>.onmicrosoft.com                                                                                                                                                                                                                   |
| AzureAd:TenantId                                 | Keep        | Common authentication endpoint, since this is a multi-tenant app                                                                                                                                                                                                                                                                                                                                                                |
| AzureAd:ClientId                                 | Change      | Copy the clientId of the multi-tenant app from its "Overview" page                                                                                                                                                                                                                                                                                                                                                              |
| AzureAd:ClientSecret                             | Change      | Go to the "Certificates & secrets" page of the single-tenant app you have registered, create a new client secret, and copy the value to the clipboard, then set the value for this setting.                                                                                                                                                                                                                                     |
| AzureAd:CallbackPath                             | Keep        | Default oidc sign in path                                                                                                                                                                                                                                                                                                                                                                                                       |
| AzureAd:SignedOutCallbackPath                    | Keep        | Default sign out path                                                                                                                                                                                                                                                                                                                                                                                                           |
| MarketplaceClient:ClientId                       | Change      | Copy the clientId of the single-tenant app from its "Overview" page. This AD app is for calling the Fulfillment API                                                                                                                                                                                                                                                                                                             |
| MarketplaceClient:TenantId                       | Change      | Copy the tenantId of the single-tenant app from its "Overview" page.                                                                                                                                                                                                                                                                                                                                                            |
| MarketplaceClient:ClientSecret                   | Change      | Go to the "Certificates & secrets" page of the single-tenant app you have registered, create a new client secret, and copy the value to the clipboard, then set the value for this setting.                                                                                                                                                                                                                                     |
| WebHookTokenParameters:Instance                  | Keep        | This is used by the library                                                                                                                                                                                                                                                                                                                                                                                                     |
| WebHookTokenParameters:TenantId                  | Change      | Set the same value as MarketplaceClient:TenantId                                                                                                                                                                                                                                                                                                                                                                                |
| WebHookTokenParameters:ClientId                  | Change      | Set the same value as MarketplaceClient:ClientId                                                                                                                                                                                                                                                                                                                                                                                |
| CommandCenter:OperationsStoreConnectionString    | Change      | Copy the connection string of the storage account you have created in the previous step. Please see [Client library documentation for details](https://github.com/Ercenk/AzureMarketplaceSaaSApiClient#operations-store)                                                                                                                                                                                                        |
| CommandCenter:CommandCenterAdmin                 | Change      | Change it to the email address you are logging on to the dashboard. Only the users with the domain name of this email is authorized to use the dashboard to display the subscriptions.                                                                                                                                                                                                                                          |
| CommandCenter:ShowUnsubscribed                   | Change      | Change true or false, depending on if you want to see the subscriptions that are not active.                                                                                                                                                                                                                                                                                                                                    |
| CommandCenter:AzureQueue:StorageConnectionString | Change      | Add the storage account connection string for the queue.                                                                                                                                                                                                                                                                                                                                                                        |
| CommandCenter:AzureQueue:QueueName               | Change      | Name of the queue the messages will go to.                                                                                                                                                                                                                                                                                                                                                                                      |
| CommandCenter:EnableDimensionMeterReporting      | Change      | Use this section to enable manually sending usage events on a dimension for a customer subscription through this App. Default: false, change to true if there is at least one dimension enabled on an Offer-Plan and would like to trigger usage events manually. [More information on Marketplace Metering Service dimensions.](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/saas-metered-billing) |
| CommandCenter:Dimensions:DimensionId             | Change      | Use this section if the above EnableDimensionMeterReporting setting is true. Add DimensionId of the enabled custom meter.                                                                                                                                                                                                                                                                                                       |
| CommandCenter:Dimensions:PlanIds                 | Change      | Use this section if the above EnableDimensionMeterReporting setting is true. Add PlanId's of the plans for which the above DimensionId is enabled.                                                                                                                                                                                                                                                                              |
| CommandCenter:Dimensions:OfferIds                | Change      | Use this section if the above EnableDimensionMeterReporting setting is true. Add OfferId's of the plans for which the above DimensionId is enabled.                                                                                                                                                                                                                                                                             |

## Create an Offer on Commercial Marketplace Portal in Partner Center

Once your AAD directory, AAD applications, and web application are setup and
ready to use, an offer must be created in the
[Commercial Marketplace Portal in the Microsoft Partner Center](https://partner.microsoft.com/en-us/dashboard/home).

Documentation on creating an offer can be found on
[Microsoft Docs: Create a New Saas Offer in the Commercial Marketplace](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/create-new-saas-offer).
Documentation is also available for all
[fields and pages for the offer on Docs](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/offer-creation-checklist)
as well.

You will need the following information to complete the offer:

- AAD Application ID (also called Client ID) from the Single-Tenant Application
- AAD Tenant ID hosting the AAD Single-Tenant
- URLs from the Azure AppService web application:
  - The Base URL `/`
  - The Landing Page `/landingpage`
  - The Webhook Endpoint `api/webhook`
  - The Privacy Policy `/privacy`
  - The Support Page `/support`
- Storage Account Connection String

Additionally, you will need assets, such as logos and screenshots, to complete
the offer listing as well. They can be found in this code repository under
`/resources`.

### Example Offer Setup in Commercial Marketplace Portal

These are sample configuration values for the offer to pass certification of the
sample SaaS solution to help you get started with a sample offer.

> **:warning: IMPORTANT:** This is just meant to be a sample. You will need to
> make adjustments based on your specific offer, even for this sample (e.g.
> contact information). _Real_ information needs to be entered; using "Lorem
> ipsum" style information will _not_ pass the certification steps if you want
> to preview your offer in the marketplace. Again, reference the
> [Microsoft Docs](https://docs.microsoft.com/en-us/azure/marketplace/partner-center-portal/create-new-saas-offer)
> for a more thorough overview of each section.

#### Offer Setup

![Microsoft Partner Center - Offer Setup](./ReadmeFiles/MicrosoftPartnerCenter-OfferSetup.png)

1. **Selling Through Microsoft:** to simplify for this sample, choose "Yes".
2. **Customer Leads:** unless you already have a CRM or other system setup that
   you'd like to use, choose Azure Table storage and use the connection string
   for the storage account you created earlier:

   ![Microsoft Partner Center - Offer Setup - Customer Leads](./ReadmeFiles/MicrosoftPartnerCenter-OfferSetup-CustomerLeads.png)

   1. **Lead Destination:** Azure Table.
   2. **Contact Email:** your email address.
   3. **Storage Account Connection String:** the connection string for the
      storage account you created earlier.
   4. **Validate:** ensure you can actually connect to the storage account.
   5. **OK:** close the dialog and you're set.

3. **Save Draft:** Between each screen, be sure to save a draft.

#### Properties

![Microsoft Partner Center - Properties](./ReadmeFiles/MicrosoftPartnerCenter-Properties.png)

1. **Category:** choose any category. Web is appropriate for this sample.
2. **Industried:** again, choose any.
3. **Legal:** optional, but choosing the Standard Contract may help with the
   certification process.
4. **Save Draft:** as always, save a draft.

#### Offer Listing

This is where most of your configuration for the offer will be. Pay attention to
all the fields, taking care to fill them out as fully as you can.

![Microsoft Partner Center - Offer Listing](./ReadmeFiles/MicrosoftPartnerCenter-OfferListing.png)

1. **Name:** the name of your offer. This is what will be listed in the
   Marketplace. Since this is just a demo, something along the lines of
   _Marketplace Demo - {YOUR ORG NAME}_ could work here.
2. **Search Results Summary:** this is the text that shows when you search for
   your offer in the marketplace – a short sentence should suffice.
3. **Description:** this needs to be a real description (i.e. no _Lorem Ipsum_).
   Feel free to use something similar to what's in the screenshot above.
4. **Getting Started Instructions:** again, this needs to be real text. Also
   feel free to use something similar to what's in the screenshot above.
5. **Search Keywords:** add your organization name, "marketplace demo", or
   something similar.
6. **Privacy Policy Link:** This needs to be your web application's privacy
   policy URL that was called out earlier. It will be in the format of
   `https://{YOUR_APP_SERVICE_NAME}.azurewebsites.net/privacy`.
7. **Contact Information:** Use your details for _Name_, _Email_, and _Phone_
   for both the _Support Contact_ and _Engineering Contact_ sections. For
   _Support URL_, you'll need to use the support URL that was called out
   earlier. It will be in the format of
   `https://{YOUR_APP_SERVICE_NAME}.azurewebsites.net/support`.
8. **Supporting Documents:** upload the PDF
   `resources/OfferListing-SupportDocuments-SupportInformation.pdf` provided in
   this repository and name it "Support Information".
9. **Marketplace Media - Logos:** upload all logos using the PNG files provided
   under `resources/` in this repository. They are named according to their
   sizes.
10. **Marketplace Media - Screenshots:** a screenshot has been provided under
    the `resources/` directory – name it "All Offer Subscriptions".
11. **Save Draft:** as always, save a draft when finished editing the page.

#### Preview Audience

![Microsoft Partner Center - Preview Audience](./ReadmeFiles/MicrosoftPartnerCenter-PreviewAudience.png)

1. **Azure Active Directory or Microsoft Account Email Address:** add any AAD or
   Microsoft account email addresses that you would like to access this
   application. These will be the accounts that have access to viewing the offer
   in its preview phase. At a minimum, add your own account.
2. **Add Another Email:** if there are others you'd like to see the offer in the
   preview phase, add up to 10 total accounts.
3. **Save Draft:** save before moving forward.

#### Technical Configuration

This page has the landing page and webhook configuration for your offer that was
deployed in earlier steps.

![Microsoft Partner Center - Technical Configuration](./ReadmeFiles/MicrosoftPartnerCenter-TechnicalConfiguration.png)

1. **Landing Page URL:** this is the Landing Page URL that was called out
   earlier. It will be in the format of
   `https://{YOUR_APP_SERVICE_NAME}.azurewebsites.net/LandingPage`.
2. **Connection Webhook:** this is the Webhook Endpoint URL that was called out
   earlier. It will be in the format of
   `https://{YOUR_APP_SERVICE_NAME}.azurewebsites.net/api/webhook`.
3. **Azure Active Directory Tenant ID:** the Tenant ID hosting the Single-Tenant
   Application.
4. **Azure Active Directory Application ID:** the Application (Client) ID from
   the Single-Tenant Application that was created.
5. **Save Draft:** save the current page.

![How does it map?](./ReadmeFiles/Mapping_to_partner_center.png)

#### Plan Overview

![Microsoft Partner Center - Plan Overview](./ReadmeFiles/MicrosoftPartnerCenter-PlanOverview.png)

1. **Create New Plan:** add a new plan to the offer to allow for signing up.
   We'll add one that's \$0 to avoid billing.
2. **Selecting Previous Plan:** if you've already created a plan, you can edit
   them by selecting from the list.
3. **Stop Selling:** if you need to "remove" a plan, you can stop selling it.

_Creating a new plan:_

1. **Plan Listing:**

   ![Microsoft Partner Center - Plan Overview - Plan Listing](./ReadmeFiles/MicrosoftPartnerCenter-PlanOverview-PlanListing.png)

   1. **Plan Name:** choose a plan name that will be listed when selecting it in
      the subscription process.
   2. **Plan Description:** a basic, real description is required for
      certification.
   3. **Save Draft**

2. **Pricing and Availability:**

   ![Microsoft Partner Center - Plan Overview - Pricing and Availability](./ReadmeFiles/MicrosoftPartnerCenter-PlanOverview-PricingAndAvailability.png)

   1. **Markets:** Choose the markets in which this plan will be available:

      ![Microsoft Partner Center - Plan Overview - Pricing and Availability - Markets](./ReadmeFiles/MicrosoftPartnerCenter-PlanOverview-PricingAndAvailability-Markets.png)

      1. **Market Selection:** Search for your current market(s).
      2. **Save:** save to close the dialog with your selection.

   2. **Pricing - Pricing Model:** choose "Flat Rate".
   3. **Pricing - Billing Term:** choose "Monthly" and set the cost to \$0.
   4. **Plan Visibility:** set to "Private".
   5. **Restricted Audience:** use the Tenant ID that hosts the Single-Tenant
      and Multi-Tenant AAD applications (the same that was used in previous
      steps).
   6. **Save Draft**

#### Co-Sell with Microsoft

_Nothing needs to be configured here for the purpose of this solution._

#### Resell Through CSPs

_Nothing needs to be configured here for the purpose of this solution._

#### Review and Publish

Under _Offer Overview_, verify that all available information looks correct.
Then choose to `Review and Publish` the offer to start the certification
process. Correct any errors that come back and work to the _Publisher Signoff_
step; this is where you'll be able to sign up for your offer before going live
(with your real SaaS offer in the future).

![Microsoft Partner Center - Offer Overview - Review and Publish](./ReadmeFiles/MicrosoftPartnerCenter-OfferOverview-ReviewAndPublish.png)

## Signing Up for Your Offer

Customer searches for the offer on Azure Portal

1. Go to Azure Portal and add a resource

   ![purchaser1](./ReadmeFiles/Purchaser1.png)

2. Find the search text box

   ![purchaser2](./ReadmeFiles/Purchaser2.png)

3. Type in your offer name

   ![purchaser3](./ReadmeFiles/Purchaser3.png)

4. Select the plan

   ![purchaser4](./ReadmeFiles/Purchaser4.png)

5. Subscribe

   ![purchaser5](./ReadmeFiles/Purchaser5.png)

6. Find the subscription after the deployment is complete, and go the
   subscription

   ![purchaser6](./ReadmeFiles/Purchaser6.png)

7. Subscription details, notice it is not active yet

   ![purchaser7](./ReadmeFiles/Purchaser7.png)

8. Landing page

   ![purchaser8](./ReadmeFiles/Purchaser8.png)

9. Purchaser submits the form, and Contoso ops team receives a notification

   ![purchaser9](./ReadmeFiles/Purchaser9.png)

10. Contoso team takes the appropriate action to qualify and onboard the
    customer

    ![purchaser10](./ReadmeFiles/Purchaser10.png)
