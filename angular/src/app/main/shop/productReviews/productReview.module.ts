import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductReviewRoutingModule } from './productReview-routing.module';
import { ProductReviewsComponent } from './productReviews.component';
import { CreateOrEditProductReviewModalComponent } from './create-or-edit-productReview-modal.component';
import { ViewProductReviewModalComponent } from './view-productReview-modal.component';
import { ProductReviewContactLookupTableModalComponent } from './productReview-contact-lookup-table-modal.component';
import { ProductReviewProductLookupTableModalComponent } from './productReview-product-lookup-table-modal.component';
import { ProductReviewStoreLookupTableModalComponent } from './productReview-store-lookup-table-modal.component';
import { ProductReviewRatingLikeLookupTableModalComponent } from './productReview-ratingLike-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductReviewsComponent,
        CreateOrEditProductReviewModalComponent,
        ViewProductReviewModalComponent,

        ProductReviewContactLookupTableModalComponent,
        ProductReviewProductLookupTableModalComponent,
        ProductReviewStoreLookupTableModalComponent,
        ProductReviewRatingLikeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductReviewRoutingModule, AdminSharedModule],
})
export class ProductReviewModule {}
