import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactMembershipHistoriesComponent} from './contactMembershipHistories.component';



const routes: Routes = [
    {
        path: '',
        component: ContactMembershipHistoriesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactMembershipHistoryRoutingModule {
}
