import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreBusinessHoursComponent } from './storeBusinessHours.component';

const routes: Routes = [
    {
        path: '',
        component: StoreBusinessHoursComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreBusinessHourRoutingModule {}
