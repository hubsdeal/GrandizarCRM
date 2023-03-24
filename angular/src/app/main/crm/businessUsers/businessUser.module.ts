import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessUserRoutingModule } from './businessUser-routing.module';
import { BusinessUsersComponent } from './businessUsers.component';
import { CreateOrEditBusinessUserModalComponent } from './create-or-edit-businessUser-modal.component';
import { ViewBusinessUserModalComponent } from './view-businessUser-modal.component';
import { BusinessUserBusinessLookupTableModalComponent } from './businessUser-business-lookup-table-modal.component';
import { BusinessUserUserLookupTableModalComponent } from './businessUser-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessUsersComponent,
        CreateOrEditBusinessUserModalComponent,
        ViewBusinessUserModalComponent,

        BusinessUserBusinessLookupTableModalComponent,
        BusinessUserUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessUserRoutingModule, AdminSharedModule],
})
export class BusinessUserModule {}
