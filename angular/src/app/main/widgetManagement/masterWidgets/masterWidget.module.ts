import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterWidgetRoutingModule } from './masterWidget-routing.module';
import { MasterWidgetsComponent } from './masterWidgets.component';
import { CreateOrEditMasterWidgetModalComponent } from './create-or-edit-masterWidget-modal.component';
import { ViewMasterWidgetModalComponent } from './view-masterWidget-modal.component';

@NgModule({
    declarations: [MasterWidgetsComponent, CreateOrEditMasterWidgetModalComponent, ViewMasterWidgetModalComponent],
    imports: [AppSharedModule, MasterWidgetRoutingModule, AdminSharedModule],
})
export class MasterWidgetModule {}
