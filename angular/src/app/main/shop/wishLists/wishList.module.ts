import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { WishListRoutingModule } from './wishList-routing.module';
import { WishListsComponent } from './wishLists.component';
import { CreateOrEditWishListModalComponent } from './create-or-edit-wishList-modal.component';
import { ViewWishListModalComponent } from './view-wishList-modal.component';
import { WishListContactLookupTableModalComponent } from './wishList-contact-lookup-table-modal.component';
import { WishListProductLookupTableModalComponent } from './wishList-product-lookup-table-modal.component';
import { WishListStoreLookupTableModalComponent } from './wishList-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        WishListsComponent,
        CreateOrEditWishListModalComponent,
        ViewWishListModalComponent,

        WishListContactLookupTableModalComponent,
        WishListProductLookupTableModalComponent,
        WishListStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, WishListRoutingModule, AdminSharedModule],
})
export class WishListModule {}
