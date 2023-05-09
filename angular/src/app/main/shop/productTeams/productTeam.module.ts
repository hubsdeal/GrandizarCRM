import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductTeamRoutingModule } from './productTeam-routing.module';
import { ProductTeamsComponent } from './productTeams.component';
import { CreateOrEditProductTeamModalComponent } from './create-or-edit-productTeam-modal.component';
import { ViewProductTeamModalComponent } from './view-productTeam-modal.component';
import { ProductTeamEmployeeLookupTableModalComponent } from './productTeam-employee-lookup-table-modal.component';
import { ProductTeamProductLookupTableModalComponent } from './productTeam-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductTeamsComponent,
        CreateOrEditProductTeamModalComponent,
        ViewProductTeamModalComponent,

        ProductTeamEmployeeLookupTableModalComponent,
        ProductTeamProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductTeamRoutingModule, AdminSharedModule],
})
export class ProductTeamModule {}
