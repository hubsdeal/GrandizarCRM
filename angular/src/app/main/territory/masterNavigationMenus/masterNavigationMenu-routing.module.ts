import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterNavigationMenusComponent } from './masterNavigationMenus.component';

const routes: Routes = [
    {
        path: '',
        component: MasterNavigationMenusComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MasterNavigationMenuRoutingModule {}
