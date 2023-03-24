import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreOwnerTeamRoutingModule } from './storeOwnerTeam-routing.module';
import { StoreOwnerTeamsComponent } from './storeOwnerTeams.component';
import { CreateOrEditStoreOwnerTeamModalComponent } from './create-or-edit-storeOwnerTeam-modal.component';
import { ViewStoreOwnerTeamModalComponent } from './view-storeOwnerTeam-modal.component';
import { StoreOwnerTeamStoreLookupTableModalComponent } from './storeOwnerTeam-store-lookup-table-modal.component';
import { StoreOwnerTeamUserLookupTableModalComponent } from './storeOwnerTeam-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreOwnerTeamsComponent,
        CreateOrEditStoreOwnerTeamModalComponent,
        ViewStoreOwnerTeamModalComponent,

        StoreOwnerTeamStoreLookupTableModalComponent,
        StoreOwnerTeamUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreOwnerTeamRoutingModule, AdminSharedModule],
})
export class StoreOwnerTeamModule {}
