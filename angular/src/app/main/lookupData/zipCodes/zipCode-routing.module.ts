import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ZipCodesComponent } from './zipCodes.component';

const routes: Routes = [
    {
        path: '',
        component: ZipCodesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ZipCodeRoutingModule {}
