import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreReviewRoutingModule } from './storeReview-routing.module';
import { StoreReviewsComponent } from './storeReviews.component';
import { CreateOrEditStoreReviewModalComponent } from './create-or-edit-storeReview-modal.component';
import { ViewStoreReviewModalComponent } from './view-storeReview-modal.component';
import { StoreReviewStoreLookupTableModalComponent } from './storeReview-store-lookup-table-modal.component';
import { StoreReviewContactLookupTableModalComponent } from './storeReview-contact-lookup-table-modal.component';
import { StoreReviewRatingLikeLookupTableModalComponent } from './storeReview-ratingLike-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreReviewsComponent,
        CreateOrEditStoreReviewModalComponent,
        ViewStoreReviewModalComponent,

        StoreReviewStoreLookupTableModalComponent,
        StoreReviewContactLookupTableModalComponent,
        StoreReviewRatingLikeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreReviewRoutingModule, AdminSharedModule],
})
export class StoreReviewModule {}
