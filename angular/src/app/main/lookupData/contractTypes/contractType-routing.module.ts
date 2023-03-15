import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContractTypesComponent } from './contractTypes.component';

const routes: Routes = [
    {
        path: '',
        component: ContractTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContractTypeRoutingModule {}
