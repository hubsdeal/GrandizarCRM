import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BookingTimeslotSettingsComponent} from './bookingTimeslotSettings.component';



const routes: Routes = [
    {
        path: '',
        component: BookingTimeslotSettingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BookingTimeslotSettingRoutingModule {
}
