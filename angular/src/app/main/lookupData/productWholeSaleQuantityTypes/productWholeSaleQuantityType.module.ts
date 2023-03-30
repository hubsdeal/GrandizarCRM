import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductWholeSaleQuantityTypeRoutingModule } from './productWholeSaleQuantityType-routing.module';
import { ProductWholeSaleQuantityTypesComponent } from './productWholeSaleQuantityTypes.component';
import { CreateOrEditProductWholeSaleQuantityTypeModalComponent } from './create-or-edit-productWholeSaleQuantityType-modal.component';
import { ViewProductWholeSaleQuantityTypeModalComponent } from './view-productWholeSaleQuantityType-modal.component';

@NgModule({
    declarations: [
        ProductWholeSaleQuantityTypesComponent,
        CreateOrEditProductWholeSaleQuantityTypeModalComponent,
        ViewProductWholeSaleQuantityTypeModalComponent,
    ],
    imports: [AppSharedModule, ProductWholeSaleQuantityTypeRoutingModule, AdminSharedModule],
})
export class ProductWholeSaleQuantityTypeModule {}
