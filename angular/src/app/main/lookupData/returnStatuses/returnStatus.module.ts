import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ReturnStatusRoutingModule } from './returnStatus-routing.module';
import { ReturnStatusesComponent } from './returnStatuses.component';
import { CreateOrEditReturnStatusModalComponent } from './create-or-edit-returnStatus-modal.component';
import { ViewReturnStatusModalComponent } from './view-returnStatus-modal.component';

@NgModule({
    declarations: [ReturnStatusesComponent, CreateOrEditReturnStatusModalComponent, ViewReturnStatusModalComponent],
    imports: [AppSharedModule, ReturnStatusRoutingModule, AdminSharedModule],
})
export class ReturnStatusModule {}
