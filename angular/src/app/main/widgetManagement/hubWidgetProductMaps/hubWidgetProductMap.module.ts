import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubWidgetProductMapRoutingModule } from './hubWidgetProductMap-routing.module';
import { HubWidgetProductMapsComponent } from './hubWidgetProductMaps.component';
import { CreateOrEditHubWidgetProductMapModalComponent } from './create-or-edit-hubWidgetProductMap-modal.component';
import { ViewHubWidgetProductMapModalComponent } from './view-hubWidgetProductMap-modal.component';
import { HubWidgetProductMapHubWidgetMapLookupTableModalComponent } from './hubWidgetProductMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetProductMapProductLookupTableModalComponent } from './hubWidgetProductMap-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubWidgetProductMapsComponent,
        CreateOrEditHubWidgetProductMapModalComponent,
        ViewHubWidgetProductMapModalComponent,

        HubWidgetProductMapHubWidgetMapLookupTableModalComponent,
        HubWidgetProductMapProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubWidgetProductMapRoutingModule, AdminSharedModule],
})
export class HubWidgetProductMapModule {}
