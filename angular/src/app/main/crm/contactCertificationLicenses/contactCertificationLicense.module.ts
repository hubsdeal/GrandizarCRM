import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactCertificationLicenseRoutingModule} from './contactCertificationLicense-routing.module';
import {ContactCertificationLicensesComponent} from './contactCertificationLicenses.component';
import {CreateOrEditContactCertificationLicenseModalComponent} from './create-or-edit-contactCertificationLicense-modal.component';
import {ViewContactCertificationLicenseModalComponent} from './view-contactCertificationLicense-modal.component';
import {ContactCertificationLicenseContactLookupTableModalComponent} from './contactCertificationLicense-contact-lookup-table-modal.component';
    					import {ContactCertificationLicenseContactDocumentLookupTableModalComponent} from './contactCertificationLicense-contactDocument-lookup-table-modal.component';
    					import {ContactCertificationLicenseEmployeeLookupTableModalComponent} from './contactCertificationLicense-employee-lookup-table-modal.component';
    					import {ContactCertificationLicenseBusinessLookupTableModalComponent} from './contactCertificationLicense-business-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactCertificationLicensesComponent,
        CreateOrEditContactCertificationLicenseModalComponent,
        ViewContactCertificationLicenseModalComponent,
        
    					ContactCertificationLicenseContactLookupTableModalComponent,
    					ContactCertificationLicenseContactDocumentLookupTableModalComponent,
    					ContactCertificationLicenseEmployeeLookupTableModalComponent,
    					ContactCertificationLicenseBusinessLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactCertificationLicenseRoutingModule , AdminSharedModule ],
    
})
export class ContactCertificationLicenseModule {
}
