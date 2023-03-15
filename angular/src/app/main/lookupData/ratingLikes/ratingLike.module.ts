import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { RatingLikeRoutingModule } from './ratingLike-routing.module';
import { RatingLikesComponent } from './ratingLikes.component';
import { CreateOrEditRatingLikeModalComponent } from './create-or-edit-ratingLike-modal.component';
import { ViewRatingLikeModalComponent } from './view-ratingLike-modal.component';

@NgModule({
    declarations: [RatingLikesComponent, CreateOrEditRatingLikeModalComponent, ViewRatingLikeModalComponent],
    imports: [AppSharedModule, RatingLikeRoutingModule, AdminSharedModule],
})
export class RatingLikeModule {}
