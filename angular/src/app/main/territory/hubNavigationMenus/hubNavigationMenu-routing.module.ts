import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubNavigationMenusComponent } from './hubNavigationMenus.component';

const routes: Routes = [
    {
        path: '',
        component: HubNavigationMenusComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubNavigationMenuRoutingModule {}
