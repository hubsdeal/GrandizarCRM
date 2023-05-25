import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductTaskMapRoutingModule } from './productTaskMap-routing.module';
import { ProductTaskMapsComponent } from './productTaskMaps.component';
import { CreateOrEditProductTaskMapModalComponent } from './create-or-edit-productTaskMap-modal.component';
import { ViewProductTaskMapModalComponent } from './view-productTaskMap-modal.component';
import { ProductTaskMapProductLookupTableModalComponent } from './productTaskMap-product-lookup-table-modal.component';
import { ProductTaskMapTaskEventLookupTableModalComponent } from './productTaskMap-taskEvent-lookup-table-modal.component';
import { ProductTaskMapProductCategoryLookupTableModalComponent } from './productTaskMap-productCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductTaskMapsComponent,
        CreateOrEditProductTaskMapModalComponent,
        ViewProductTaskMapModalComponent,

        ProductTaskMapProductLookupTableModalComponent,
        ProductTaskMapTaskEventLookupTableModalComponent,
        ProductTaskMapProductCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductTaskMapRoutingModule, AdminSharedModule],
})
export class ProductTaskMapModule {}
