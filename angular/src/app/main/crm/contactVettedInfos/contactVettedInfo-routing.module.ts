import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactVettedInfosComponent} from './contactVettedInfos.component';



const routes: Routes = [
    {
        path: '',
        component: ContactVettedInfosComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactVettedInfoRoutingModule {
}
