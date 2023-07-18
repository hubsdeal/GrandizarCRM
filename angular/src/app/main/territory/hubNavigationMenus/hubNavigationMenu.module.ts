import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubNavigationMenuRoutingModule } from './hubNavigationMenu-routing.module';
import { HubNavigationMenusComponent } from './hubNavigationMenus.component';
import { CreateOrEditHubNavigationMenuModalComponent } from './create-or-edit-hubNavigationMenu-modal.component';
import { ViewHubNavigationMenuModalComponent } from './view-hubNavigationMenu-modal.component';
import { HubNavigationMenuHubLookupTableModalComponent } from './hubNavigationMenu-hub-lookup-table-modal.component';
import { HubNavigationMenuMasterNavigationMenuLookupTableModalComponent } from './hubNavigationMenu-masterNavigationMenu-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubNavigationMenusComponent,
        CreateOrEditHubNavigationMenuModalComponent,
        ViewHubNavigationMenuModalComponent,

        HubNavigationMenuHubLookupTableModalComponent,
        HubNavigationMenuMasterNavigationMenuLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubNavigationMenuRoutingModule, AdminSharedModule],
    exports: [HubNavigationMenusComponent, CreateOrEditHubNavigationMenuModalComponent, ViewHubNavigationMenuModalComponent],
})
export class HubNavigationMenuModule {}
