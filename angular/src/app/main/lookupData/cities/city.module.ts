import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CityRoutingModule } from './city-routing.module';
import { CitiesComponent } from './cities.component';
import { CreateOrEditCityModalComponent } from './create-or-edit-city-modal.component';
import { ViewCityModalComponent } from './view-city-modal.component';

@NgModule({
    declarations: [CitiesComponent, CreateOrEditCityModalComponent, ViewCityModalComponent],
    imports: [AppSharedModule, CityRoutingModule, AdminSharedModule],
})
export class CityModule {}
