import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubTypeRoutingModule } from './hubType-routing.module';
import { HubTypesComponent } from './hubTypes.component';
import { CreateOrEditHubTypeModalComponent } from './create-or-edit-hubType-modal.component';
import { ViewHubTypeModalComponent } from './view-hubType-modal.component';

@NgModule({
    declarations: [HubTypesComponent, CreateOrEditHubTypeModalComponent, ViewHubTypeModalComponent],
    imports: [AppSharedModule, HubTypeRoutingModule, AdminSharedModule],
})
export class HubTypeModule {}
