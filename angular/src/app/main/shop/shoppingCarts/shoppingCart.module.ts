import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ShoppingCartRoutingModule } from './shoppingCart-routing.module';
import { ShoppingCartsComponent } from './shoppingCarts.component';
import { CreateOrEditShoppingCartModalComponent } from './create-or-edit-shoppingCart-modal.component';
import { ViewShoppingCartModalComponent } from './view-shoppingCart-modal.component';
import { ShoppingCartContactLookupTableModalComponent } from './shoppingCart-contact-lookup-table-modal.component';
import { ShoppingCartOrderLookupTableModalComponent } from './shoppingCart-order-lookup-table-modal.component';
import { ShoppingCartStoreLookupTableModalComponent } from './shoppingCart-store-lookup-table-modal.component';
import { ShoppingCartProductLookupTableModalComponent } from './shoppingCart-product-lookup-table-modal.component';
import { ShoppingCartCurrencyLookupTableModalComponent } from './shoppingCart-currency-lookup-table-modal.component';

@NgModule({
    declarations: [
        ShoppingCartsComponent,
        CreateOrEditShoppingCartModalComponent,
        ViewShoppingCartModalComponent,

        ShoppingCartContactLookupTableModalComponent,
        ShoppingCartOrderLookupTableModalComponent,
        ShoppingCartStoreLookupTableModalComponent,
        ShoppingCartProductLookupTableModalComponent,
        ShoppingCartCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ShoppingCartRoutingModule, AdminSharedModule],
})
export class ShoppingCartModule {}
