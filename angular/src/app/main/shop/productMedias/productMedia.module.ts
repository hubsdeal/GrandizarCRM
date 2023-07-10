import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductMediaRoutingModule } from './productMedia-routing.module';
import { ProductMediasComponent } from './productMedias.component';
import { CreateOrEditProductMediaModalComponent } from './create-or-edit-productMedia-modal.component';
import { ViewProductMediaModalComponent } from './view-productMedia-modal.component';
import { ProductMediaProductLookupTableModalComponent } from './productMedia-product-lookup-table-modal.component';
import { ProductMediaMediaLibraryLookupTableModalComponent } from './productMedia-mediaLibrary-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductMediasComponent,
        CreateOrEditProductMediaModalComponent,
        ViewProductMediaModalComponent,

        ProductMediaProductLookupTableModalComponent,
        ProductMediaMediaLibraryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductMediaRoutingModule, AdminSharedModule],
    exports: [ProductMediasComponent, CreateOrEditProductMediaModalComponent, ViewProductMediaModalComponent],
})
export class ProductMediaModule {}
