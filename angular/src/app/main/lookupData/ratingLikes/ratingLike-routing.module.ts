import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RatingLikesComponent } from './ratingLikes.component';

const routes: Routes = [
    {
        path: '',
        component: RatingLikesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RatingLikeRoutingModule {}
