import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCustomerStatRoutingModule } from './productCustomerStat-routing.module';
import { ProductCustomerStatsComponent } from './productCustomerStats.component';
import { CreateOrEditProductCustomerStatModalComponent } from './create-or-edit-productCustomerStat-modal.component';
import { ViewProductCustomerStatModalComponent } from './view-productCustomerStat-modal.component';
import { ProductCustomerStatProductLookupTableModalComponent } from './productCustomerStat-product-lookup-table-modal.component';
import { ProductCustomerStatContactLookupTableModalComponent } from './productCustomerStat-contact-lookup-table-modal.component';
import { ProductCustomerStatStoreLookupTableModalComponent } from './productCustomerStat-store-lookup-table-modal.component';
import { ProductCustomerStatHubLookupTableModalComponent } from './productCustomerStat-hub-lookup-table-modal.component';
import { ProductCustomerStatSocialMediaLookupTableModalComponent } from './productCustomerStat-socialMedia-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCustomerStatsComponent,
        CreateOrEditProductCustomerStatModalComponent,
        ViewProductCustomerStatModalComponent,

        ProductCustomerStatProductLookupTableModalComponent,
        ProductCustomerStatContactLookupTableModalComponent,
        ProductCustomerStatStoreLookupTableModalComponent,
        ProductCustomerStatHubLookupTableModalComponent,
        ProductCustomerStatSocialMediaLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductCustomerStatRoutingModule, AdminSharedModule],
    exports: [ProductCustomerStatsComponent,
        CreateOrEditProductCustomerStatModalComponent,
        ViewProductCustomerStatModalComponent,

        ProductCustomerStatProductLookupTableModalComponent,
        ProductCustomerStatContactLookupTableModalComponent,
        ProductCustomerStatStoreLookupTableModalComponent,
        ProductCustomerStatHubLookupTableModalComponent,
        ProductCustomerStatSocialMediaLookupTableModalComponent,]
})
export class ProductCustomerStatModule { }
