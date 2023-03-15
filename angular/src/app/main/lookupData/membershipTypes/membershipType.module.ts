import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MembershipTypeRoutingModule } from './membershipType-routing.module';
import { MembershipTypesComponent } from './membershipTypes.component';
import { CreateOrEditMembershipTypeModalComponent } from './create-or-edit-membershipType-modal.component';
import { ViewMembershipTypeModalComponent } from './view-membershipType-modal.component';

@NgModule({
    declarations: [
        MembershipTypesComponent,
        CreateOrEditMembershipTypeModalComponent,
        ViewMembershipTypeModalComponent,
    ],
    imports: [AppSharedModule, MembershipTypeRoutingModule, AdminSharedModule],
})
export class MembershipTypeModule {}
