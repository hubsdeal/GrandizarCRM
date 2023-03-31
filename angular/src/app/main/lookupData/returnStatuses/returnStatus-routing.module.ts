import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReturnStatusesComponent } from './returnStatuses.component';

const routes: Routes = [
    {
        path: '',
        component: ReturnStatusesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ReturnStatusRoutingModule {}
