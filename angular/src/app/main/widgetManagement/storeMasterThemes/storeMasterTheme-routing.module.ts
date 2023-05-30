import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreMasterThemesComponent } from './storeMasterThemes.component';

const routes: Routes = [
    {
        path: '',
        component: StoreMasterThemesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreMasterThemeRoutingModule {}
