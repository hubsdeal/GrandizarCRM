import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterNavigationMenuRoutingModule } from './masterNavigationMenu-routing.module';
import { MasterNavigationMenusComponent } from './masterNavigationMenus.component';
import { CreateOrEditMasterNavigationMenuModalComponent } from './create-or-edit-masterNavigationMenu-modal.component';
import { ViewMasterNavigationMenuModalComponent } from './view-masterNavigationMenu-modal.component';

@NgModule({
    declarations: [
        MasterNavigationMenusComponent,
        CreateOrEditMasterNavigationMenuModalComponent,
        ViewMasterNavigationMenuModalComponent,
    ],
    imports: [AppSharedModule, MasterNavigationMenuRoutingModule, AdminSharedModule],
})
export class MasterNavigationMenuModule {}
