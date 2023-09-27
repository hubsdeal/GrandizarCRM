import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactEducationsComponent} from './contactEducations.component';



const routes: Routes = [
    {
        path: '',
        component: ContactEducationsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactEducationRoutingModule {
}
