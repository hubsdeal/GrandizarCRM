import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductNoteRoutingModule } from './productNote-routing.module';
import { ProductNotesComponent } from './productNotes.component';
import { CreateOrEditProductNoteModalComponent } from './create-or-edit-productNote-modal.component';
import { ViewProductNoteModalComponent } from './view-productNote-modal.component';
import { ProductNoteProductLookupTableModalComponent } from './productNote-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductNotesComponent,
        CreateOrEditProductNoteModalComponent,
        ViewProductNoteModalComponent,

        ProductNoteProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductNoteRoutingModule, AdminSharedModule],
})
export class ProductNoteModule {}
