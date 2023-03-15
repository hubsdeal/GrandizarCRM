import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StateRoutingModule } from './state-routing.module';
import { StatesComponent } from './states.component';
import { CreateOrEditStateModalComponent } from './create-or-edit-state-modal.component';
import { ViewStateModalComponent } from './view-state-modal.component';

@NgModule({
    declarations: [StatesComponent, CreateOrEditStateModalComponent, ViewStateModalComponent],
    imports: [AppSharedModule, StateRoutingModule, AdminSharedModule],
})
export class StateModule {}
