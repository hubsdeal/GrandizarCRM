import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreReviewFeedbackRoutingModule } from './storeReviewFeedback-routing.module';
import { StoreReviewFeedbacksComponent } from './storeReviewFeedbacks.component';
import { CreateOrEditStoreReviewFeedbackModalComponent } from './create-or-edit-storeReviewFeedback-modal.component';
import { ViewStoreReviewFeedbackModalComponent } from './view-storeReviewFeedback-modal.component';
import { StoreReviewFeedbackStoreReviewLookupTableModalComponent } from './storeReviewFeedback-storeReview-lookup-table-modal.component';
import { StoreReviewFeedbackContactLookupTableModalComponent } from './storeReviewFeedback-contact-lookup-table-modal.component';
import { StoreReviewFeedbackRatingLikeLookupTableModalComponent } from './storeReviewFeedback-ratingLike-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreReviewFeedbacksComponent,
        CreateOrEditStoreReviewFeedbackModalComponent,
        ViewStoreReviewFeedbackModalComponent,

        StoreReviewFeedbackStoreReviewLookupTableModalComponent,
        StoreReviewFeedbackContactLookupTableModalComponent,
        StoreReviewFeedbackRatingLikeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreReviewFeedbackRoutingModule, AdminSharedModule],
})
export class StoreReviewFeedbackModule {}
