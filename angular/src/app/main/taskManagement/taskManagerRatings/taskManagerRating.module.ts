import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {TaskManagerRatingRoutingModule} from './taskManagerRating-routing.module';
import {TaskManagerRatingsComponent} from './taskManagerRatings.component';
import {CreateOrEditTaskManagerRatingModalComponent} from './create-or-edit-taskManagerRating-modal.component';
import {ViewTaskManagerRatingModalComponent} from './view-taskManagerRating-modal.component';
import {TaskManagerRatingTaskEventLookupTableModalComponent} from './taskManagerRating-taskEvent-lookup-table-modal.component';
    					import {TaskManagerRatingEmployeeLookupTableModalComponent} from './taskManagerRating-employee-lookup-table-modal.component';
    					import {TaskManagerRatingTaskTeamLookupTableModalComponent} from './taskManagerRating-taskTeam-lookup-table-modal.component';
    					import {TaskManagerRatingRatingLikeLookupTableModalComponent} from './taskManagerRating-ratingLike-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        TaskManagerRatingsComponent,
        CreateOrEditTaskManagerRatingModalComponent,
        ViewTaskManagerRatingModalComponent,
        
    					TaskManagerRatingTaskEventLookupTableModalComponent,
    					TaskManagerRatingEmployeeLookupTableModalComponent,
    					TaskManagerRatingTaskTeamLookupTableModalComponent,
    					TaskManagerRatingRatingLikeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TaskManagerRatingRoutingModule , AdminSharedModule ],
    
})
export class TaskManagerRatingModule {
}
