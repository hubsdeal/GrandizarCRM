import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductAndGiftCardMapRoutingModule } from './productAndGiftCardMap-routing.module';
import { ProductAndGiftCardMapsComponent } from './productAndGiftCardMaps.component';
import { CreateOrEditProductAndGiftCardMapModalComponent } from './create-or-edit-productAndGiftCardMap-modal.component';
import { ViewProductAndGiftCardMapModalComponent } from './view-productAndGiftCardMap-modal.component';
import { ProductAndGiftCardMapProductLookupTableModalComponent } from './productAndGiftCardMap-product-lookup-table-modal.component';
import { ProductAndGiftCardMapCurrencyLookupTableModalComponent } from './productAndGiftCardMap-currency-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductAndGiftCardMapsComponent,
        CreateOrEditProductAndGiftCardMapModalComponent,
        ViewProductAndGiftCardMapModalComponent,

        ProductAndGiftCardMapProductLookupTableModalComponent,
        ProductAndGiftCardMapCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductAndGiftCardMapRoutingModule, AdminSharedModule],
})
export class ProductAndGiftCardMapModule {}
