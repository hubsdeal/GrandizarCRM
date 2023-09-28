import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContractFinancialTermRoutingModule} from './contractFinancialTerm-routing.module';
import {ContractFinancialTermsComponent} from './contractFinancialTerms.component';
import {CreateOrEditContractFinancialTermModalComponent} from './create-or-edit-contractFinancialTerm-modal.component';
import {ViewContractFinancialTermModalComponent} from './view-contractFinancialTerm-modal.component';
import {ContractFinancialTermContractLookupTableModalComponent} from './contractFinancialTerm-contract-lookup-table-modal.component';
    					import {ContractFinancialTermHubLookupTableModalComponent} from './contractFinancialTerm-hub-lookup-table-modal.component';
    					import {ContractFinancialTermBusinessLookupTableModalComponent} from './contractFinancialTerm-business-lookup-table-modal.component';
    					import {ContractFinancialTermStoreLookupTableModalComponent} from './contractFinancialTerm-store-lookup-table-modal.component';
    					import {ContractFinancialTermJobLookupTableModalComponent} from './contractFinancialTerm-job-lookup-table-modal.component';
    					import {ContractFinancialTermProductCategoryLookupTableModalComponent} from './contractFinancialTerm-productCategory-lookup-table-modal.component';
    					import {ContractFinancialTermProductLookupTableModalComponent} from './contractFinancialTerm-product-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContractFinancialTermsComponent,
        CreateOrEditContractFinancialTermModalComponent,
        ViewContractFinancialTermModalComponent,
        
    					ContractFinancialTermContractLookupTableModalComponent,
    					ContractFinancialTermHubLookupTableModalComponent,
    					ContractFinancialTermBusinessLookupTableModalComponent,
    					ContractFinancialTermStoreLookupTableModalComponent,
    					ContractFinancialTermJobLookupTableModalComponent,
    					ContractFinancialTermProductCategoryLookupTableModalComponent,
    					ContractFinancialTermProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContractFinancialTermRoutingModule , AdminSharedModule ],
    
})
export class ContractFinancialTermModule {
}
