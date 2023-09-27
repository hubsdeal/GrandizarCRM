import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactProductRecommendationRoutingModule} from './contactProductRecommendation-routing.module';
import {ContactProductRecommendationsComponent} from './contactProductRecommendations.component';
import {CreateOrEditContactProductRecommendationModalComponent} from './create-or-edit-contactProductRecommendation-modal.component';
import {ViewContactProductRecommendationModalComponent} from './view-contactProductRecommendation-modal.component';
import {ContactProductRecommendationUserLookupTableModalComponent} from './contactProductRecommendation-user-lookup-table-modal.component';
    					import {ContactProductRecommendationContactLookupTableModalComponent} from './contactProductRecommendation-contact-lookup-table-modal.component';
    					import {ContactProductRecommendationHubLookupTableModalComponent} from './contactProductRecommendation-hub-lookup-table-modal.component';
    					import {ContactProductRecommendationStoreLookupTableModalComponent} from './contactProductRecommendation-store-lookup-table-modal.component';
    					import {ContactProductRecommendationProductLookupTableModalComponent} from './contactProductRecommendation-product-lookup-table-modal.component';
    					import {ContactProductRecommendationProductCategoryLookupTableModalComponent} from './contactProductRecommendation-productCategory-lookup-table-modal.component';
    					import {ContactProductRecommendationJobLookupTableModalComponent} from './contactProductRecommendation-job-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactProductRecommendationsComponent,
        CreateOrEditContactProductRecommendationModalComponent,
        ViewContactProductRecommendationModalComponent,
        
    					ContactProductRecommendationUserLookupTableModalComponent,
    					ContactProductRecommendationContactLookupTableModalComponent,
    					ContactProductRecommendationHubLookupTableModalComponent,
    					ContactProductRecommendationStoreLookupTableModalComponent,
    					ContactProductRecommendationProductLookupTableModalComponent,
    					ContactProductRecommendationProductCategoryLookupTableModalComponent,
    					ContactProductRecommendationJobLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactProductRecommendationRoutingModule , AdminSharedModule ],
    
})
export class ContactProductRecommendationModule {
}
