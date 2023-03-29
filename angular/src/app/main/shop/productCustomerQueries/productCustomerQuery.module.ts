import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCustomerQueryRoutingModule } from './productCustomerQuery-routing.module';
import { ProductCustomerQueriesComponent } from './productCustomerQueries.component';
import { CreateOrEditProductCustomerQueryModalComponent } from './create-or-edit-productCustomerQuery-modal.component';
import { ViewProductCustomerQueryModalComponent } from './view-productCustomerQuery-modal.component';
import { ProductCustomerQueryProductLookupTableModalComponent } from './productCustomerQuery-product-lookup-table-modal.component';
import { ProductCustomerQueryContactLookupTableModalComponent } from './productCustomerQuery-contact-lookup-table-modal.component';
import { ProductCustomerQueryEmployeeLookupTableModalComponent } from './productCustomerQuery-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCustomerQueriesComponent,
        CreateOrEditProductCustomerQueryModalComponent,
        ViewProductCustomerQueryModalComponent,

        ProductCustomerQueryProductLookupTableModalComponent,
        ProductCustomerQueryContactLookupTableModalComponent,
        ProductCustomerQueryEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductCustomerQueryRoutingModule, AdminSharedModule],
})
export class ProductCustomerQueryModule {}
