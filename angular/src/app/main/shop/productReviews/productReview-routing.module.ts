import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductReviewsComponent } from './productReviews.component';

const routes: Routes = [
    {
        path: '',
        component: ProductReviewsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductReviewRoutingModule {}
