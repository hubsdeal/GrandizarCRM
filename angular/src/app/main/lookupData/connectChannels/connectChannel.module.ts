import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ConnectChannelRoutingModule } from './connectChannel-routing.module';
import { ConnectChannelsComponent } from './connectChannels.component';
import { CreateOrEditConnectChannelModalComponent } from './create-or-edit-connectChannel-modal.component';
import { ViewConnectChannelModalComponent } from './view-connectChannel-modal.component';

@NgModule({
    declarations: [
        ConnectChannelsComponent,
        CreateOrEditConnectChannelModalComponent,
        ViewConnectChannelModalComponent,
    ],
    imports: [AppSharedModule, ConnectChannelRoutingModule, AdminSharedModule],
})
export class ConnectChannelModule {}
