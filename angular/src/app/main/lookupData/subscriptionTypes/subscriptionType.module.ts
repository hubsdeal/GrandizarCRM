import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SubscriptionTypeRoutingModule } from './subscriptionType-routing.module';
import { SubscriptionTypesComponent } from './subscriptionTypes.component';
import { CreateOrEditSubscriptionTypeModalComponent } from './create-or-edit-subscriptionType-modal.component';
import { ViewSubscriptionTypeModalComponent } from './view-subscriptionType-modal.component';

@NgModule({
    declarations: [
        SubscriptionTypesComponent,
        CreateOrEditSubscriptionTypeModalComponent,
        ViewSubscriptionTypeModalComponent,
    ],
    imports: [AppSharedModule, SubscriptionTypeRoutingModule, AdminSharedModule],
})
export class SubscriptionTypeModule {}
