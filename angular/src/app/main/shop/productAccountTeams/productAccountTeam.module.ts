import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductAccountTeamRoutingModule } from './productAccountTeam-routing.module';
import { ProductAccountTeamsComponent } from './productAccountTeams.component';
import { CreateOrEditProductAccountTeamModalComponent } from './create-or-edit-productAccountTeam-modal.component';
import { ViewProductAccountTeamModalComponent } from './view-productAccountTeam-modal.component';
import { ProductAccountTeamEmployeeLookupTableModalComponent } from './productAccountTeam-employee-lookup-table-modal.component';
import { ProductAccountTeamProductLookupTableModalComponent } from './productAccountTeam-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductAccountTeamsComponent,
        CreateOrEditProductAccountTeamModalComponent,
        ViewProductAccountTeamModalComponent,

        ProductAccountTeamEmployeeLookupTableModalComponent,
        ProductAccountTeamProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductAccountTeamRoutingModule, AdminSharedModule],
    exports:[ProductAccountTeamsComponent,
        CreateOrEditProductAccountTeamModalComponent,
        ViewProductAccountTeamModalComponent,

        ProductAccountTeamEmployeeLookupTableModalComponent,
        ProductAccountTeamProductLookupTableModalComponent,]
})
export class ProductAccountTeamModule {}
