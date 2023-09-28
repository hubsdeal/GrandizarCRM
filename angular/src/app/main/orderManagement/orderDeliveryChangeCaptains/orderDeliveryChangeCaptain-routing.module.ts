import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {OrderDeliveryChangeCaptainsComponent} from './orderDeliveryChangeCaptains.component';



const routes: Routes = [
    {
        path: '',
        component: OrderDeliveryChangeCaptainsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderDeliveryChangeCaptainRoutingModule {
}
