import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DocumentTypeRoutingModule } from './documentType-routing.module';
import { DocumentTypesComponent } from './documentTypes.component';
import { CreateOrEditDocumentTypeModalComponent } from './create-or-edit-documentType-modal.component';
import { ViewDocumentTypeModalComponent } from './view-documentType-modal.component';

@NgModule({
    declarations: [DocumentTypesComponent, CreateOrEditDocumentTypeModalComponent, ViewDocumentTypeModalComponent],
    imports: [AppSharedModule, DocumentTypeRoutingModule, AdminSharedModule],
})
export class DocumentTypeModule {}
