import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductReviewFeedbackRoutingModule } from './productReviewFeedback-routing.module';
import { ProductReviewFeedbacksComponent } from './productReviewFeedbacks.component';
import { CreateOrEditProductReviewFeedbackModalComponent } from './create-or-edit-productReviewFeedback-modal.component';
import { ViewProductReviewFeedbackModalComponent } from './view-productReviewFeedback-modal.component';
import { ProductReviewFeedbackContactLookupTableModalComponent } from './productReviewFeedback-contact-lookup-table-modal.component';
import { ProductReviewFeedbackProductReviewLookupTableModalComponent } from './productReviewFeedback-productReview-lookup-table-modal.component';
import { ProductReviewFeedbackRatingLikeLookupTableModalComponent } from './productReviewFeedback-ratingLike-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductReviewFeedbacksComponent,
        CreateOrEditProductReviewFeedbackModalComponent,
        ViewProductReviewFeedbackModalComponent,

        ProductReviewFeedbackContactLookupTableModalComponent,
        ProductReviewFeedbackProductReviewLookupTableModalComponent,
        ProductReviewFeedbackRatingLikeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductReviewFeedbackRoutingModule, AdminSharedModule],
})
export class ProductReviewFeedbackModule {}
