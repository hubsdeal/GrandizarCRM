import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactReferralContactsComponent} from './contactReferralContacts.component';



const routes: Routes = [
    {
        path: '',
        component: ContactReferralContactsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactReferralContactRoutingModule {
}
