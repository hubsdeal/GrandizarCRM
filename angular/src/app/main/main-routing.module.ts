import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
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
