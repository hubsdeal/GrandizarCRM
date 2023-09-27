import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactExperienceRoutingModule} from './contactExperience-routing.module';
import {ContactExperiencesComponent} from './contactExperiences.component';
import {CreateOrEditContactExperienceModalComponent} from './create-or-edit-contactExperience-modal.component';
import {ViewContactExperienceModalComponent} from './view-contactExperience-modal.component';
import {ContactExperienceContactLookupTableModalComponent} from './contactExperience-contact-lookup-table-modal.component';
    					import {ContactExperienceEmployeeLookupTableModalComponent} from './contactExperience-employee-lookup-table-modal.component';
    					import {ContactExperienceBusinessLookupTableModalComponent} from './contactExperience-business-lookup-table-modal.component';
    					import {ContactExperienceCurrencyLookupTableModalComponent} from './contactExperience-currency-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactExperiencesComponent,
        CreateOrEditContactExperienceModalComponent,
        ViewContactExperienceModalComponent,
        
    					ContactExperienceContactLookupTableModalComponent,
    					ContactExperienceEmployeeLookupTableModalComponent,
    					ContactExperienceBusinessLookupTableModalComponent,
    					ContactExperienceCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactExperienceRoutingModule , AdminSharedModule ],
    
})
export class ContactExperienceModule {
}
