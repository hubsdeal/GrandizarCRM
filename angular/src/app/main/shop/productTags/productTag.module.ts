import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductTagRoutingModule } from './productTag-routing.module';
import { ProductTagsComponent } from './productTags.component';
import { CreateOrEditProductTagModalComponent } from './create-or-edit-productTag-modal.component';
import { ViewProductTagModalComponent } from './view-productTag-modal.component';
import { ProductTagProductLookupTableModalComponent } from './productTag-product-lookup-table-modal.component';
import { ProductTagMasterTagCategoryLookupTableModalComponent } from './productTag-masterTagCategory-lookup-table-modal.component';
import { ProductTagMasterTagLookupTableModalComponent } from './productTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductTagsComponent,
        CreateOrEditProductTagModalComponent,
        ViewProductTagModalComponent,

        ProductTagProductLookupTableModalComponent,
        ProductTagMasterTagCategoryLookupTableModalComponent,
        ProductTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductTagRoutingModule, AdminSharedModule],
    exports: [ProductTagsComponent, CreateOrEditProductTagModalComponent, ViewProductTagModalComponent],
})
export class ProductTagModule {}
