import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactEducationRoutingModule} from './contactEducation-routing.module';
import {ContactEducationsComponent} from './contactEducations.component';
import {CreateOrEditContactEducationModalComponent} from './create-or-edit-contactEducation-modal.component';
import {ViewContactEducationModalComponent} from './view-contactEducation-modal.component';
import {ContactEducationContactLookupTableModalComponent} from './contactEducation-contact-lookup-table-modal.component';
    					import {ContactEducationEmployeeLookupTableModalComponent} from './contactEducation-employee-lookup-table-modal.component';
    					import {ContactEducationBusinessLookupTableModalComponent} from './contactEducation-business-lookup-table-modal.component';
    					import {ContactEducationContactDocumentLookupTableModalComponent} from './contactEducation-contactDocument-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactEducationsComponent,
        CreateOrEditContactEducationModalComponent,
        ViewContactEducationModalComponent,
        
    					ContactEducationContactLookupTableModalComponent,
    					ContactEducationEmployeeLookupTableModalComponent,
    					ContactEducationBusinessLookupTableModalComponent,
    					ContactEducationContactDocumentLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactEducationRoutingModule , AdminSharedModule ],
    
})
export class ContactEducationModule {
}
