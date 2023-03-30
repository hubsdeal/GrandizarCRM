import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductPackageRoutingModule } from './productPackage-routing.module';
import { ProductPackagesComponent } from './productPackages.component';
import { CreateOrEditProductPackageModalComponent } from './create-or-edit-productPackage-modal.component';
import { ViewProductPackageModalComponent } from './view-productPackage-modal.component';
import { ProductPackageProductLookupTableModalComponent } from './productPackage-product-lookup-table-modal.component';
import { ProductPackageMediaLibraryLookupTableModalComponent } from './productPackage-mediaLibrary-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductPackagesComponent,
        CreateOrEditProductPackageModalComponent,
        ViewProductPackageModalComponent,

        ProductPackageProductLookupTableModalComponent,
        ProductPackageMediaLibraryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductPackageRoutingModule, AdminSharedModule],
})
export class ProductPackageModule {}
