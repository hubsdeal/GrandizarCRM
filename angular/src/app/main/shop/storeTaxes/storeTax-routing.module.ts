import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreTaxesComponent } from './storeTaxes.component';

const routes: Routes = [
    {
        path: '',
        component: StoreTaxesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreTaxRoutingModule {}
