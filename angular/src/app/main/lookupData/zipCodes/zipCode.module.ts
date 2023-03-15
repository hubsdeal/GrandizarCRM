import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ZipCodeRoutingModule } from './zipCode-routing.module';
import { ZipCodesComponent } from './zipCodes.component';
import { CreateOrEditZipCodeModalComponent } from './create-or-edit-zipCode-modal.component';
import { ViewZipCodeModalComponent } from './view-zipCode-modal.component';

@NgModule({
    declarations: [ZipCodesComponent, CreateOrEditZipCodeModalComponent, ViewZipCodeModalComponent],
    imports: [AppSharedModule, ZipCodeRoutingModule, AdminSharedModule],
})
export class ZipCodeModule {}
