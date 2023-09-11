import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreAccountTeamRoutingModule } from './storeAccountTeam-routing.module';
import { StoreAccountTeamsComponent } from './storeAccountTeams.component';
import { CreateOrEditStoreAccountTeamModalComponent } from './create-or-edit-storeAccountTeam-modal.component';
import { ViewStoreAccountTeamModalComponent } from './view-storeAccountTeam-modal.component';
import { StoreAccountTeamStoreLookupTableModalComponent } from './storeAccountTeam-store-lookup-table-modal.component';
import { StoreAccountTeamEmployeeLookupTableModalComponent } from './storeAccountTeam-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreAccountTeamsComponent,
        CreateOrEditStoreAccountTeamModalComponent,
        ViewStoreAccountTeamModalComponent,

        StoreAccountTeamStoreLookupTableModalComponent,
        StoreAccountTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreAccountTeamRoutingModule, AdminSharedModule],
    exports:[
        StoreAccountTeamsComponent,
        CreateOrEditStoreAccountTeamModalComponent,
        ViewStoreAccountTeamModalComponent,

        StoreAccountTeamStoreLookupTableModalComponent,
        StoreAccountTeamEmployeeLookupTableModalComponent,
    ]
})
export class StoreAccountTeamModule {}
