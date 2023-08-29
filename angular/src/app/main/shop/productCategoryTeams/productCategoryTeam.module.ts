import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCategoryTeamRoutingModule } from './productCategoryTeam-routing.module';
import { ProductCategoryTeamsComponent } from './productCategoryTeams.component';
import { CreateOrEditProductCategoryTeamModalComponent } from './create-or-edit-productCategoryTeam-modal.component';
import { ViewProductCategoryTeamModalComponent } from './view-productCategoryTeam-modal.component';
import { ProductCategoryTeamProductCategoryLookupTableModalComponent } from './productCategoryTeam-productCategory-lookup-table-modal.component';
import { ProductCategoryTeamEmployeeLookupTableModalComponent } from './productCategoryTeam-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCategoryTeamsComponent,
        CreateOrEditProductCategoryTeamModalComponent,
        ViewProductCategoryTeamModalComponent,

        ProductCategoryTeamProductCategoryLookupTableModalComponent,
        ProductCategoryTeamEmployeeLookupTableModalComponent,
    ],
    exports:[ProductCategoryTeamsComponent],
    imports: [AppSharedModule, ProductCategoryTeamRoutingModule, AdminSharedModule],
})
export class ProductCategoryTeamModule {}
