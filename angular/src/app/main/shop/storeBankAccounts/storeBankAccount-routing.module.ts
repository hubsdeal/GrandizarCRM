import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreBankAccountsComponent } from './storeBankAccounts.component';

const routes: Routes = [
    {
        path: '',
        component: StoreBankAccountsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreBankAccountRoutingModule {}
