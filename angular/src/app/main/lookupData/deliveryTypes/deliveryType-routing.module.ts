import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DeliveryTypesComponent} from './deliveryTypes.component';



const routes: Routes = [
    {
        path: '',
        component: DeliveryTypesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DeliveryTypeRoutingModule {
}
