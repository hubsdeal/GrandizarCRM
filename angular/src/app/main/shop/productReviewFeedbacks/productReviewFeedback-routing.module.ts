import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductReviewFeedbacksComponent } from './productReviewFeedbacks.component';

const routes: Routes = [
    {
        path: '',
        component: ProductReviewFeedbacksComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductReviewFeedbackRoutingModule {}
