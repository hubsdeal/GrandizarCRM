import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreThemeMapRoutingModule } from './storeThemeMap-routing.module';
import { StoreThemeMapsComponent } from './storeThemeMaps.component';
import { CreateOrEditStoreThemeMapModalComponent } from './create-or-edit-storeThemeMap-modal.component';
import { ViewStoreThemeMapModalComponent } from './view-storeThemeMap-modal.component';
import { StoreThemeMapStoreMasterThemeLookupTableModalComponent } from './storeThemeMap-storeMasterTheme-lookup-table-modal.component';
import { StoreThemeMapStoreLookupTableModalComponent } from './storeThemeMap-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreThemeMapsComponent,
        CreateOrEditStoreThemeMapModalComponent,
        ViewStoreThemeMapModalComponent,

        StoreThemeMapStoreMasterThemeLookupTableModalComponent,
        StoreThemeMapStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreThemeMapRoutingModule, AdminSharedModule],
})
export class StoreThemeMapModule {}
