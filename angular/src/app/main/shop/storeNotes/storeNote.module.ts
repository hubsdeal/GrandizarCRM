import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreNoteRoutingModule } from './storeNote-routing.module';
import { StoreNotesComponent } from './storeNotes.component';
import { CreateOrEditStoreNoteModalComponent } from './create-or-edit-storeNote-modal.component';
import { ViewStoreNoteModalComponent } from './view-storeNote-modal.component';
import { StoreNoteStoreLookupTableModalComponent } from './storeNote-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreNotesComponent,
        CreateOrEditStoreNoteModalComponent,
        ViewStoreNoteModalComponent,

        StoreNoteStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreNoteRoutingModule, AdminSharedModule],
})
export class StoreNoteModule {}
