import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubRoutingModule } from './hub-routing.module';
import { HubsComponent } from './hubs.component';
import { CreateOrEditHubModalComponent } from './create-or-edit-hub-modal.component';
import { ViewHubModalComponent } from './view-hub-modal.component';

@NgModule({
    declarations: [HubsComponent, CreateOrEditHubModalComponent, ViewHubModalComponent],
    imports: [AppSharedModule, HubRoutingModule, AdminSharedModule],
})
export class HubModule {}
