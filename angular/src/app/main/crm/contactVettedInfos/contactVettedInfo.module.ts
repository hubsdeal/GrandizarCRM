import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactVettedInfoRoutingModule} from './contactVettedInfo-routing.module';
import {ContactVettedInfosComponent} from './contactVettedInfos.component';
import {CreateOrEditContactVettedInfoModalComponent} from './create-or-edit-contactVettedInfo-modal.component';
import {ViewContactVettedInfoModalComponent} from './view-contactVettedInfo-modal.component';
import {ContactVettedInfoContactLookupTableModalComponent} from './contactVettedInfo-contact-lookup-table-modal.component';
    					import {ContactVettedInfoContactEducationLookupTableModalComponent} from './contactVettedInfo-contactEducation-lookup-table-modal.component';
    					import {ContactVettedInfoContactExperienceLookupTableModalComponent} from './contactVettedInfo-contactExperience-lookup-table-modal.component';
    					import {ContactVettedInfoContactCertificationLicenseLookupTableModalComponent} from './contactVettedInfo-contactCertificationLicense-lookup-table-modal.component';
    					import {ContactVettedInfoEmployeeLookupTableModalComponent} from './contactVettedInfo-employee-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactVettedInfosComponent,
        CreateOrEditContactVettedInfoModalComponent,
        ViewContactVettedInfoModalComponent,
        
    					ContactVettedInfoContactLookupTableModalComponent,
    					ContactVettedInfoContactEducationLookupTableModalComponent,
    					ContactVettedInfoContactExperienceLookupTableModalComponent,
    					ContactVettedInfoContactCertificationLicenseLookupTableModalComponent,
    					ContactVettedInfoEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactVettedInfoRoutingModule , AdminSharedModule ],
    
})
export class ContactVettedInfoModule {
}
