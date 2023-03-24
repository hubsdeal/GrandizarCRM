import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreBusinessHourRoutingModule } from './storeBusinessHour-routing.module';
import { StoreBusinessHoursComponent } from './storeBusinessHours.component';
import { CreateOrEditStoreBusinessHourModalComponent } from './create-or-edit-storeBusinessHour-modal.component';
import { ViewStoreBusinessHourModalComponent } from './view-storeBusinessHour-modal.component';
import { StoreBusinessHourStoreLookupTableModalComponent } from './storeBusinessHour-store-lookup-table-modal.component';
import { StoreBusinessHourMasterTagCategoryLookupTableModalComponent } from './storeBusinessHour-masterTagCategory-lookup-table-modal.component';
import { StoreBusinessHourMasterTagLookupTableModalComponent } from './storeBusinessHour-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreBusinessHoursComponent,
        CreateOrEditStoreBusinessHourModalComponent,
        ViewStoreBusinessHourModalComponent,

        StoreBusinessHourStoreLookupTableModalComponent,
        StoreBusinessHourMasterTagCategoryLookupTableModalComponent,
        StoreBusinessHourMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreBusinessHourRoutingModule, AdminSharedModule],
})
export class StoreBusinessHourModule {}
