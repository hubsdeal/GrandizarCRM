import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CountryRoutingModule } from './country-routing.module';
import { CountriesComponent } from './countries.component';
import { CreateOrEditCountryModalComponent } from './create-or-edit-country-modal.component';
import { ViewCountryModalComponent } from './view-country-modal.component';

@NgModule({
    declarations: [CountriesComponent, CreateOrEditCountryModalComponent, ViewCountryModalComponent],
    imports: [AppSharedModule, CountryRoutingModule, AdminSharedModule],
})
export class CountryModule {}
