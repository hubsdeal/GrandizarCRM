import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ConnectMessagesComponent} from './connectMessages.component';



const routes: Routes = [
    {
        path: '',
        component: ConnectMessagesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConnectMessageRoutingModule {
}
