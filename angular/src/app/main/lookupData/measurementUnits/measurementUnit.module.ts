import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MeasurementUnitRoutingModule } from './measurementUnit-routing.module';
import { MeasurementUnitsComponent } from './measurementUnits.component';
import { CreateOrEditMeasurementUnitModalComponent } from './create-or-edit-measurementUnit-modal.component';
import { ViewMeasurementUnitModalComponent } from './view-measurementUnit-modal.component';

@NgModule({
    declarations: [
        MeasurementUnitsComponent,
        CreateOrEditMeasurementUnitModalComponent,
        ViewMeasurementUnitModalComponent,
    ],
    imports: [AppSharedModule, MeasurementUnitRoutingModule, AdminSharedModule],
})
export class MeasurementUnitModule {}
