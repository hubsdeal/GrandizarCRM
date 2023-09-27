import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactAddressesComponent} from './contactAddresses.component';



const routes: Routes = [
    {
        path: '',
        component: ContactAddressesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactAddressRoutingModule {
}
