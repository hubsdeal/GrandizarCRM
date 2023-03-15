import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CountyRoutingModule } from './county-routing.module';
import { CountiesComponent } from './counties.component';
import { CreateOrEditCountyModalComponent } from './create-or-edit-county-modal.component';
import { ViewCountyModalComponent } from './view-county-modal.component';

@NgModule({
    declarations: [CountiesComponent, CreateOrEditCountyModalComponent, ViewCountyModalComponent],
    imports: [AppSharedModule, CountyRoutingModule, AdminSharedModule],
})
export class CountyModule {}
