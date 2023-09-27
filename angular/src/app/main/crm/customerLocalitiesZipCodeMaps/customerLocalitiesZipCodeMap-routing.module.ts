import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CustomerLocalitiesZipCodeMapsComponent} from './customerLocalitiesZipCodeMaps.component';



const routes: Routes = [
    {
        path: '',
        component: CustomerLocalitiesZipCodeMapsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomerLocalitiesZipCodeMapRoutingModule {
}
