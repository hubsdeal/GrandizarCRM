import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {OrderDeliveryByCaptainsComponent} from './orderDeliveryByCaptains.component';



const routes: Routes = [
    {
        path: '',
        component: OrderDeliveryByCaptainsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderDeliveryByCaptainRoutingModule {
}
