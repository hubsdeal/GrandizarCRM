import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductFaqRoutingModule } from './productFaq-routing.module';
import { ProductFaqsComponent } from './productFaqs.component';
import { CreateOrEditProductFaqModalComponent } from './create-or-edit-productFaq-modal.component';
import { ViewProductFaqModalComponent } from './view-productFaq-modal.component';
import { ProductFaqProductLookupTableModalComponent } from './productFaq-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductFaqsComponent,
        CreateOrEditProductFaqModalComponent,
        ViewProductFaqModalComponent,

        ProductFaqProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductFaqRoutingModule, AdminSharedModule],
})
export class ProductFaqModule {}
