import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {StoreProductServiceLocalityMapsComponent} from './storeProductServiceLocalityMaps.component';



const routes: Routes = [
    {
        path: '',
        component: StoreProductServiceLocalityMapsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreProductServiceLocalityMapRoutingModule {
}
