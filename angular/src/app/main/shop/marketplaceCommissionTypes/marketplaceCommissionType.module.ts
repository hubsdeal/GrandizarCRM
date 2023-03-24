import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MarketplaceCommissionTypeRoutingModule } from './marketplaceCommissionType-routing.module';
import { MarketplaceCommissionTypesComponent } from './marketplaceCommissionTypes.component';
import { CreateOrEditMarketplaceCommissionTypeModalComponent } from './create-or-edit-marketplaceCommissionType-modal.component';
import { ViewMarketplaceCommissionTypeModalComponent } from './view-marketplaceCommissionType-modal.component';

@NgModule({
    declarations: [
        MarketplaceCommissionTypesComponent,
        CreateOrEditMarketplaceCommissionTypeModalComponent,
        ViewMarketplaceCommissionTypeModalComponent,
    ],
    imports: [AppSharedModule, MarketplaceCommissionTypeRoutingModule, AdminSharedModule],
})
export class MarketplaceCommissionTypeModule {}
