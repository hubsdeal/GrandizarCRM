import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductOwnerPublicContactInfoRoutingModule } from './productOwnerPublicContactInfo-routing.module';
import { ProductOwnerPublicContactInfosComponent } from './productOwnerPublicContactInfos.component';
import { CreateOrEditProductOwnerPublicContactInfoModalComponent } from './create-or-edit-productOwnerPublicContactInfo-modal.component';
import { ViewProductOwnerPublicContactInfoModalComponent } from './view-productOwnerPublicContactInfo-modal.component';
import { ProductOwnerPublicContactInfoContactLookupTableModalComponent } from './productOwnerPublicContactInfo-contact-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoProductLookupTableModalComponent } from './productOwnerPublicContactInfo-product-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoStoreLookupTableModalComponent } from './productOwnerPublicContactInfo-store-lookup-table-modal.component';
import { ProductOwnerPublicContactInfoUserLookupTableModalComponent } from './productOwnerPublicContactInfo-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductOwnerPublicContactInfosComponent,
        CreateOrEditProductOwnerPublicContactInfoModalComponent,
        ViewProductOwnerPublicContactInfoModalComponent,

        ProductOwnerPublicContactInfoContactLookupTableModalComponent,
        ProductOwnerPublicContactInfoProductLookupTableModalComponent,
        ProductOwnerPublicContactInfoStoreLookupTableModalComponent,
        ProductOwnerPublicContactInfoUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductOwnerPublicContactInfoRoutingModule, AdminSharedModule],
})
export class ProductOwnerPublicContactInfoModule {}
