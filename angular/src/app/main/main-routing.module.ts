﻿import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'salesLeadManagement/leadReferralRewards',
                        loadChildren: () => import('./salesLeadManagement/leadReferralRewards/leadReferralReward.module').then(m => m.LeadReferralRewardModule),
                        data: { permission: 'Pages.LeadReferralRewards' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadContacts',
                        loadChildren: () => import('./salesLeadManagement/leadContacts/leadContact.module').then(m => m.LeadContactModule),
                        data: { permission: 'Pages.LeadContacts' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadNotes',
                        loadChildren: () => import('./salesLeadManagement/leadNotes/leadNote.module').then(m => m.LeadNoteModule),
                        data: { permission: 'Pages.LeadNotes' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadTasks',
                        loadChildren: () => import('./salesLeadManagement/leadTasks/leadTask.module').then(m => m.LeadTaskModule),
                        data: { permission: 'Pages.LeadTasks' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadTags',
                        loadChildren: () => import('./salesLeadManagement/leadTags/leadTag.module').then(m => m.LeadTagModule),
                        data: { permission: 'Pages.LeadTags' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadSalesTeams',
                        loadChildren: () => import('./salesLeadManagement/leadSalesTeams/leadSalesTeam.module').then(m => m.LeadSalesTeamModule),
                        data: { permission: 'Pages.LeadSalesTeams' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadPipelineStatuses',
                        loadChildren: () => import('./salesLeadManagement/leadPipelineStatuses/leadPipelineStatus.module').then(m => m.LeadPipelineStatusModule),
                        data: { permission: 'Pages.LeadPipelineStatuses' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leads',
                        loadChildren: () => import('./salesLeadManagement/leads/lead.module').then(m => m.LeadModule),
                        data: { permission: 'Pages.Leads' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadPipelineStages',
                        loadChildren: () => import('./salesLeadManagement/leadPipelineStages/leadPipelineStage.module').then(m => m.LeadPipelineStageModule),
                        data: { permission: 'Pages.LeadPipelineStages' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadSources',
                        loadChildren: () => import('./salesLeadManagement/leadSources/leadSource.module').then(m => m.LeadSourceModule),
                        data: { permission: 'Pages.LeadSources' }
                    },
                
                    
                    {
                        path: 'shop/storeZipCodeMaps',
                        loadChildren: () => import('./shop/storeZipCodeMaps/storeZipCodeMap.module').then(m => m.StoreZipCodeMapModule),
                        data: { permission: 'Pages.StoreZipCodeMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeReviewFeedbacks',
                        loadChildren: () => import('./shop/storeReviewFeedbacks/storeReviewFeedback.module').then(m => m.StoreReviewFeedbackModule),
                        data: { permission: 'Pages.StoreReviewFeedbacks' }
                    },
                
                    
                    {
                        path: 'shop/storeReviews',
                        loadChildren: () => import('./shop/storeReviews/storeReview.module').then(m => m.StoreReviewModule),
                        data: { permission: 'Pages.StoreReviews' }
                    },
                
                    
                    {
                        path: 'shop/storeRelevantStores',
                        loadChildren: () => import('./shop/storeRelevantStores/storeRelevantStore.module').then(m => m.StoreRelevantStoreModule),
                        data: { permission: 'Pages.StoreRelevantStores' }
                    },
                
                    
                    {
                        path: 'shop/storeMarketplaceCommissionSettings',
                        loadChildren: () => import('./shop/storeMarketplaceCommissionSettings/storeMarketplaceCommissionSetting.module').then(m => m.StoreMarketplaceCommissionSettingModule),
                        data: { permission: 'Pages.StoreMarketplaceCommissionSettings' }
                    },
                
                    
                    {
                        path: 'shop/marketplaceCommissionTypes',
                        loadChildren: () => import('./shop/marketplaceCommissionTypes/marketplaceCommissionType.module').then(m => m.MarketplaceCommissionTypeModule),
                        data: { permission: 'Pages.MarketplaceCommissionTypes' }
                    },
                
                    
                    {
                        path: 'shop/storeSalesAlerts',
                        loadChildren: () => import('./shop/storeSalesAlerts/storeSalesAlert.module').then(m => m.StoreSalesAlertModule),
                        data: { permission: 'Pages.StoreSalesAlerts' }
                    },
                
                    
                    {
                        path: 'shop/storeTaxes',
                        loadChildren: () => import('./shop/storeTaxes/storeTax.module').then(m => m.StoreTaxModule),
                        data: { permission: 'Pages.StoreTaxes' }
                    },
                
                    
                    {
                        path: 'shop/storeBankAccounts',
                        loadChildren: () => import('./shop/storeBankAccounts/storeBankAccount.module').then(m => m.StoreBankAccountModule),
                        data: { permission: 'Pages.StoreBankAccounts' }
                    },
                
                    
                    {
                        path: 'shop/storeNotes',
                        loadChildren: () => import('./shop/storeNotes/storeNote.module').then(m => m.StoreNoteModule),
                        data: { permission: 'Pages.StoreNotes' }
                    },
                
                    
                    {
                        path: 'shop/storeMedias',
                        loadChildren: () => import('./shop/storeMedias/storeMedia.module').then(m => m.StoreMediaModule),
                        data: { permission: 'Pages.StoreMedias' }
                    },
                
                    
                    {
                        path: 'shop/storeLocations',
                        loadChildren: () => import('./shop/storeLocations/storeLocation.module').then(m => m.StoreLocationModule),
                        data: { permission: 'Pages.StoreLocations' }
                    },
                
                    
                    {
                        path: 'shop/storeBusinessHours',
                        loadChildren: () => import('./shop/storeBusinessHours/storeBusinessHour.module').then(m => m.StoreBusinessHourModule),
                        data: { permission: 'Pages.StoreBusinessHours' }
                    },
                
                    
                    {
                        path: 'shop/storeContactMaps',
                        loadChildren: () => import('./shop/storeContactMaps/storeContactMap.module').then(m => m.StoreContactMapModule),
                        data: { permission: 'Pages.StoreContactMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeBusinessCustomerMaps',
                        loadChildren: () => import('./shop/storeBusinessCustomerMaps/storeBusinessCustomerMap.module').then(m => m.StoreBusinessCustomerMapModule),
                        data: { permission: 'Pages.StoreBusinessCustomerMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeOwnerTeams',
                        loadChildren: () => import('./shop/storeOwnerTeams/storeOwnerTeam.module').then(m => m.StoreOwnerTeamModule),
                        data: { permission: 'Pages.StoreOwnerTeams' }
                    },
                
                    
                    {
                        path: 'shop/storeAccountTeams',
                        loadChildren: () => import('./shop/storeAccountTeams/storeAccountTeam.module').then(m => m.StoreAccountTeamModule),
                        data: { permission: 'Pages.StoreAccountTeams' }
                    },
                
                    
                    {
                        path: 'shop/storeProductMaps',
                        loadChildren: () => import('./shop/storeProductMaps/storeProductMap.module').then(m => m.StoreProductMapModule),
                        data: { permission: 'Pages.StoreProductMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeProductCategoryMaps',
                        loadChildren: () => import('./shop/storeProductCategoryMaps/storeProductCategoryMap.module').then(m => m.StoreProductCategoryMapModule),
                        data: { permission: 'Pages.StoreProductCategoryMaps' }
                    },
                
                    
                    {
                        path: 'crm/employeeTags',
                        loadChildren: () => import('./crm/employeeTags/employeeTag.module').then(m => m.EmployeeTagModule),
                        data: { permission: 'Pages.EmployeeTags' }
                    },
                
                    
                    {
                        path: 'crm/businessTaskMaps',
                        loadChildren: () => import('./crm/businessTaskMaps/businessTaskMap.module').then(m => m.BusinessTaskMapModule),
                        data: { permission: 'Pages.BusinessTaskMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessProductMaps',
                        loadChildren: () => import('./crm/businessProductMaps/businessProductMap.module').then(m => m.BusinessProductMapModule),
                        data: { permission: 'Pages.BusinessProductMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessStoreMaps',
                        loadChildren: () => import('./crm/businessStoreMaps/businessStoreMap.module').then(m => m.BusinessStoreMapModule),
                        data: { permission: 'Pages.BusinessStoreMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessJobMaps',
                        loadChildren: () => import('./crm/businessJobMaps/businessJobMap.module').then(m => m.BusinessJobMapModule),
                        data: { permission: 'Pages.BusinessJobMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessNotes',
                        loadChildren: () => import('./crm/businessNotes/businessNote.module').then(m => m.BusinessNoteModule),
                        data: { permission: 'Pages.BusinessNotes' }
                    },
                
                    
                    {
                        path: 'crm/businessUsers',
                        loadChildren: () => import('./crm/businessUsers/businessUser.module').then(m => m.BusinessUserModule),
                        data: { permission: 'Pages.BusinessUsers' }
                    },
                
                    
                    {
                        path: 'crm/businessAccountTeams',
                        loadChildren: () => import('./crm/businessAccountTeams/businessAccountTeam.module').then(m => m.BusinessAccountTeamModule),
                        data: { permission: 'Pages.BusinessAccountTeams' }
                    },
                
                    
                    {
                        path: 'crm/businessContactMaps',
                        loadChildren: () => import('./crm/businessContactMaps/businessContactMap.module').then(m => m.BusinessContactMapModule),
                        data: { permission: 'Pages.BusinessContactMaps' }
                    },
                
                    
                    {
                        path: 'taskManagement/taskTags',
                        loadChildren: () => import('./taskManagement/taskTags/taskTag.module').then(m => m.TaskTagModule),
                        data: { permission: 'Pages.TaskTags' }
                    },
                
                    
                    {
                        path: 'taskManagement/taskEvents',
                        loadChildren: () => import('./taskManagement/taskEvents/taskEvent.module').then(m => m.TaskEventModule),
                        data: { permission: 'Pages.TaskEvents' }
                    },
                
                    
                    {
                        path: 'taskManagement/taskStatuses',
                        loadChildren: () => import('./taskManagement/taskStatuses/taskStatus.module').then(m => m.TaskStatusModule),
                        data: { permission: 'Pages.TaskStatuses' }
                    },
                
                    
                    {
                        path: 'crm/employees',
                        loadChildren: () => import('./crm/employees/employee.module').then(m => m.EmployeeModule),
                        data: { permission: 'Pages.Employees' }
                    },
                
                    
                    {
                        path: 'jobManagement/jobTags',
                        loadChildren: () => import('./jobManagement/jobTags/jobTag.module').then(m => m.JobTagModule),
                        data: { permission: 'Pages.JobTags' }
                    },
                
                    
                    {
                        path: 'jobManagement/jobs',
                        loadChildren: () => import('./jobManagement/jobs/job.module').then(m => m.JobModule),
                        data: { permission: 'Pages.Jobs' }
                    },
                
                    
                    {
                        path: 'jobManagement/jobStatusTypes',
                        loadChildren: () => import('./jobManagement/jobStatusTypes/jobStatusType.module').then(m => m.JobStatusTypeModule),
                        data: { permission: 'Pages.JobStatusTypes' }
                    },
                
                    
                    {
                        path: 'shop/storeTags',
                        loadChildren: () => import('./shop/storeTags/storeTag.module').then(m => m.StoreTagModule),
                        data: { permission: 'Pages.StoreTags' }
                    },
                
                    
                    {
                        path: 'shop/productTags',
                        loadChildren: () => import('./shop/productTags/productTag.module').then(m => m.ProductTagModule),
                        data: { permission: 'Pages.ProductTags' }
                    },
                
                    
                    {
                        path: 'crm/businessTags',
                        loadChildren: () => import('./crm/businessTags/businessTag.module').then(m => m.BusinessTagModule),
                        data: { permission: 'Pages.BusinessTags' }
                    },
                
                    
                    {
                        path: 'crm/contactTags',
                        loadChildren: () => import('./crm/contactTags/contactTag.module').then(m => m.ContactTagModule),
                        data: { permission: 'Pages.ContactTags' }
                    },
                
                    
                    {
                        path: 'shop/stores',
                        loadChildren: () => import('./shop/stores/store.module').then(m => m.StoreModule),
                        data: { permission: 'Pages.Stores' }
                    },
                
                    
                    {
                        path: 'shop/products',
                        loadChildren: () => import('./shop/products/product.module').then(m => m.ProductModule),
                        data: { permission: 'Pages.Products' }
                    },
                
                    
                    {
                        path: 'shop/productCategories',
                        loadChildren: () => import('./shop/productCategories/productCategory.module').then(m => m.ProductCategoryModule),
                        data: { permission: 'Pages.ProductCategories' }
                    },
                
                    
                    {
                        path: 'lookupData/mediaLibraries',
                        loadChildren: () => import('./lookupData/mediaLibraries/mediaLibrary.module').then(m => m.MediaLibraryModule),
                        data: { permission: 'Pages.MediaLibraries' }
                    },
                
                    
                    {
                        path: 'crm/businesses',
                        loadChildren: () => import('./crm/businesses/business.module').then(m => m.BusinessModule),
                        data: { permission: 'Pages.Businesses' }
                    },
                
                    
                    {
                        path: 'crm/contacts',
                        loadChildren: () => import('./crm/contacts/contact.module').then(m => m.ContactModule),
                        data: { permission: 'Pages.Contacts' }
                    },
                
                    
                    {
                        path: 'territory/hubs',
                        loadChildren: () => import('./territory/hubs/hub.module').then(m => m.HubModule),
                        data: { permission: 'Pages.Hubs' }
                    },
                
                    
                    {
                        path: 'lookupData/hubTypes',
                        loadChildren: () => import('./lookupData/hubTypes/hubType.module').then(m => m.HubTypeModule),
                        data: { permission: 'Pages.HubTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/membershipTypes',
                        loadChildren: () => import('./lookupData/membershipTypes/membershipType.module').then(m => m.MembershipTypeModule),
                        data: { permission: 'Pages.MembershipTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/contractTypes',
                        loadChildren: () => import('./lookupData/contractTypes/contractType.module').then(m => m.ContractTypeModule),
                        data: { permission: 'Pages.ContractTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/documentTypes',
                        loadChildren: () => import('./lookupData/documentTypes/documentType.module').then(m => m.DocumentTypeModule),
                        data: { permission: 'Pages.DocumentTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/smsTemplates',
                        loadChildren: () => import('./lookupData/smsTemplates/smsTemplate.module').then(m => m.SmsTemplateModule),
                        data: { permission: 'Pages.SmsTemplates' }
                    },
                
                    
                    {
                        path: 'lookupData/emailTemplates',
                        loadChildren: () => import('./lookupData/emailTemplates/emailTemplate.module').then(m => m.EmailTemplateModule),
                        data: { permission: 'Pages.EmailTemplates' }
                    },
                
                    
                    {
                        path: 'lookupData/connectChannels',
                        loadChildren: () => import('./lookupData/connectChannels/connectChannel.module').then(m => m.ConnectChannelModule),
                        data: { permission: 'Pages.ConnectChannels' }
                    },
                
                    
                    {
                        path: 'lookupData/zipCodes',
                        loadChildren: () => import('./lookupData/zipCodes/zipCode.module').then(m => m.ZipCodeModule),
                        data: { permission: 'Pages.ZipCodes' }
                    },
                
                    
                    {
                        path: 'lookupData/ratingLikes',
                        loadChildren: () => import('./lookupData/ratingLikes/ratingLike.module').then(m => m.RatingLikeModule),
                        data: { permission: 'Pages.RatingLikes' }
                    },
                
                    
                    {
                        path: 'lookupData/measurementUnits',
                        loadChildren: () => import('./lookupData/measurementUnits/measurementUnit.module').then(m => m.MeasurementUnitModule),
                        data: { permission: 'Pages.MeasurementUnits' }
                    },
                
                    
                    {
                        path: 'lookupData/masterTags',
                        loadChildren: () => import('./lookupData/masterTags/masterTag.module').then(m => m.MasterTagModule),
                        data: { permission: 'Pages.MasterTags' }
                    },
                
                    
                    {
                        path: 'lookupData/masterTagCategories',
                        loadChildren: () => import('./lookupData/masterTagCategories/masterTagCategory.module').then(m => m.MasterTagCategoryModule),
                        data: { permission: 'Pages.MasterTagCategories' }
                    },
                
                    
                    {
                        path: 'lookupData/cities',
                        loadChildren: () => import('./lookupData/cities/city.module').then(m => m.CityModule),
                        data: { permission: 'Pages.Cities' }
                    },
                
                    
                    {
                        path: 'lookupData/counties',
                        loadChildren: () => import('./lookupData/counties/county.module').then(m => m.CountyModule),
                        data: { permission: 'Pages.Counties' }
                    },
                
                    
                    {
                        path: 'lookupData/states',
                        loadChildren: () => import('./lookupData/states/state.module').then(m => m.StateModule),
                        data: { permission: 'Pages.States' }
                    },
                
                    
                    {
                        path: 'lookupData/countries',
                        loadChildren: () => import('./lookupData/countries/country.module').then(m => m.CountryModule),
                        data: { permission: 'Pages.Countries' }
                    },
                
                    
                    {
                        path: 'lookupData/currencies',
                        loadChildren: () => import('./lookupData/currencies/currency.module').then(m => m.CurrencyModule),
                        data: { permission: 'Pages.Currencies' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
