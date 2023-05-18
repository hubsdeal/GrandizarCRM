import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessDocumentRoutingModule } from './businessDocument-routing.module';
import { BusinessDocumentsComponent } from './businessDocuments.component';
import { CreateOrEditBusinessDocumentModalComponent } from './create-or-edit-businessDocument-modal.component';
import { ViewBusinessDocumentModalComponent } from './view-businessDocument-modal.component';
import { BusinessDocumentBusinessLookupTableModalComponent } from './businessDocument-business-lookup-table-modal.component';
import { BusinessDocumentDocumentTypeLookupTableModalComponent } from './businessDocument-documentType-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessDocumentsComponent,
        CreateOrEditBusinessDocumentModalComponent,
        ViewBusinessDocumentModalComponent,

        BusinessDocumentBusinessLookupTableModalComponent,
        BusinessDocumentDocumentTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessDocumentRoutingModule, AdminSharedModule],
})
export class BusinessDocumentModule {}
