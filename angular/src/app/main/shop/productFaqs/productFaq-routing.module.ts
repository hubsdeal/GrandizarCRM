import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductFaqsComponent } from './productFaqs.component';

const routes: Routes = [
    {
        path: '',
        component: ProductFaqsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductFaqRoutingModule {}
