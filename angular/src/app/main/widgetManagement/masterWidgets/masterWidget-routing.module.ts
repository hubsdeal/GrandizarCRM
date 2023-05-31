import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterWidgetsComponent } from './masterWidgets.component';

const routes: Routes = [
    {
        path: '',
        component: MasterWidgetsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MasterWidgetRoutingModule {}
