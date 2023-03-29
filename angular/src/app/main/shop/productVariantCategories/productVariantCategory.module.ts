import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductVariantCategoryRoutingModule } from './productVariantCategory-routing.module';
import { ProductVariantCategoriesComponent } from './productVariantCategories.component';
import { CreateOrEditProductVariantCategoryModalComponent } from './create-or-edit-productVariantCategory-modal.component';
import { ViewProductVariantCategoryModalComponent } from './view-productVariantCategory-modal.component';
import { ProductVariantCategoryStoreLookupTableModalComponent } from './productVariantCategory-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductVariantCategoriesComponent,
        CreateOrEditProductVariantCategoryModalComponent,
        ViewProductVariantCategoryModalComponent,

        ProductVariantCategoryStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductVariantCategoryRoutingModule, AdminSharedModule],
})
export class ProductVariantCategoryModule {}
