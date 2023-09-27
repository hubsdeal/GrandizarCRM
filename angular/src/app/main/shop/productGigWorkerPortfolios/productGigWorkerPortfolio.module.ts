import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ProductGigWorkerPortfolioRoutingModule} from './productGigWorkerPortfolio-routing.module';
import {ProductGigWorkerPortfoliosComponent} from './productGigWorkerPortfolios.component';
import {CreateOrEditProductGigWorkerPortfolioModalComponent} from './create-or-edit-productGigWorkerPortfolio-modal.component';
import {ViewProductGigWorkerPortfolioModalComponent} from './view-productGigWorkerPortfolio-modal.component';
import {ProductGigWorkerPortfolioBusinessLookupTableModalComponent} from './productGigWorkerPortfolio-business-lookup-table-modal.component';
    					import {ProductGigWorkerPortfolioContactLookupTableModalComponent} from './productGigWorkerPortfolio-contact-lookup-table-modal.component';
    					import {ProductGigWorkerPortfolioProductLookupTableModalComponent} from './productGigWorkerPortfolio-product-lookup-table-modal.component';
    					import {ProductGigWorkerPortfolioEmployeeLookupTableModalComponent} from './productGigWorkerPortfolio-employee-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ProductGigWorkerPortfoliosComponent,
        CreateOrEditProductGigWorkerPortfolioModalComponent,
        ViewProductGigWorkerPortfolioModalComponent,
        
    					ProductGigWorkerPortfolioBusinessLookupTableModalComponent,
    					ProductGigWorkerPortfolioContactLookupTableModalComponent,
    					ProductGigWorkerPortfolioProductLookupTableModalComponent,
    					ProductGigWorkerPortfolioEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductGigWorkerPortfolioRoutingModule , AdminSharedModule ],
    
})
export class ProductGigWorkerPortfolioModule {
}
