import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CurrencyRoutingModule } from './currency-routing.module';
import { CurrenciesComponent } from './currencies.component';
import { CreateOrEditCurrencyModalComponent } from './create-or-edit-currency-modal.component';
import { ViewCurrencyModalComponent } from './view-currency-modal.component';

@NgModule({
    declarations: [CurrenciesComponent, CreateOrEditCurrencyModalComponent, ViewCurrencyModalComponent],
    imports: [AppSharedModule, CurrencyRoutingModule, AdminSharedModule],
})
export class CurrencyModule {}
