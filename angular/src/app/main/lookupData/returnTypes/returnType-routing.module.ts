import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReturnTypesComponent } from './returnTypes.component';

const routes: Routes = [
    {
        path: '',
        component: ReturnTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ReturnTypeRoutingModule {}
