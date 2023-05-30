import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreMasterThemeRoutingModule } from './storeMasterTheme-routing.module';
import { StoreMasterThemesComponent } from './storeMasterThemes.component';
import { CreateOrEditStoreMasterThemeModalComponent } from './create-or-edit-storeMasterTheme-modal.component';
import { ViewStoreMasterThemeModalComponent } from './view-storeMasterTheme-modal.component';

@NgModule({
    declarations: [
        StoreMasterThemesComponent,
        CreateOrEditStoreMasterThemeModalComponent,
        ViewStoreMasterThemeModalComponent,
    ],
    imports: [AppSharedModule, StoreMasterThemeRoutingModule, AdminSharedModule],
})
export class StoreMasterThemeModule {}
