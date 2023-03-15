import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeasurementUnitsComponent } from './measurementUnits.component';

const routes: Routes = [
    {
        path: '',
        component: MeasurementUnitsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MeasurementUnitRoutingModule {}
