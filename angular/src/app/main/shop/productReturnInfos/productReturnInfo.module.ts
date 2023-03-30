import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductReturnInfoRoutingModule } from './productReturnInfo-routing.module';
import { ProductReturnInfosComponent } from './productReturnInfos.component';
import { CreateOrEditProductReturnInfoModalComponent } from './create-or-edit-productReturnInfo-modal.component';
import { ViewProductReturnInfoModalComponent } from './view-productReturnInfo-modal.component';
import { ProductReturnInfoProductLookupTableModalComponent } from './productReturnInfo-product-lookup-table-modal.component';
import { ProductReturnInfoReturnTypeLookupTableModalComponent } from './productReturnInfo-returnType-lookup-table-modal.component';
import { ProductReturnInfoReturnStatusLookupTableModalComponent } from './productReturnInfo-returnStatus-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductReturnInfosComponent,
        CreateOrEditProductReturnInfoModalComponent,
        ViewProductReturnInfoModalComponent,

        ProductReturnInfoProductLookupTableModalComponent,
        ProductReturnInfoReturnTypeLookupTableModalComponent,
        ProductReturnInfoReturnStatusLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductReturnInfoRoutingModule, AdminSharedModule],
})
export class ProductReturnInfoModule {}
