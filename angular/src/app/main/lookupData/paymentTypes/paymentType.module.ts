import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { PaymentTypeRoutingModule } from './paymentType-routing.module';
import { PaymentTypesComponent } from './paymentTypes.component';
import { CreateOrEditPaymentTypeModalComponent } from './create-or-edit-paymentType-modal.component';
import { ViewPaymentTypeModalComponent } from './view-paymentType-modal.component';

@NgModule({
    declarations: [PaymentTypesComponent, CreateOrEditPaymentTypeModalComponent, ViewPaymentTypeModalComponent],
    imports: [AppSharedModule, PaymentTypeRoutingModule, AdminSharedModule],
})
export class PaymentTypeModule {}
