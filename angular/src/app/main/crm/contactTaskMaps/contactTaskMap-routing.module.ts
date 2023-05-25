import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactTaskMapsComponent } from './contactTaskMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ContactTaskMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactTaskMapRoutingModule {}
