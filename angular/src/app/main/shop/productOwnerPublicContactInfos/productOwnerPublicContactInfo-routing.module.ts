import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductOwnerPublicContactInfosComponent } from './productOwnerPublicContactInfos.component';

const routes: Routes = [
    {
        path: '',
        component: ProductOwnerPublicContactInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductOwnerPublicContactInfoRoutingModule {}
