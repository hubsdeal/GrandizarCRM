import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreReviewFeedbacksComponent } from './storeReviewFeedbacks.component';

const routes: Routes = [
    {
        path: '',
        component: StoreReviewFeedbacksComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreReviewFeedbackRoutingModule {}
