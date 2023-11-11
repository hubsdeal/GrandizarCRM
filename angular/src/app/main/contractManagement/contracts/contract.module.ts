import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContractRoutingModule} from './contract-routing.module';
import {ContractsComponent} from './contracts.component';
import {CreateOrEditContractModalComponent} from './create-or-edit-contract-modal.component';
import {ViewContractModalComponent} from './view-contract-modal.component';
import {ContractContractTypeLookupTableModalComponent} from './contract-contractType-lookup-table-modal.component';
    					import {ContractStoreLookupTableModalComponent} from './contract-store-lookup-table-modal.component';
    					import {ContractBusinessLookupTableModalComponent} from './contract-business-lookup-table-modal.component';
    					import {ContractEmployeeLookupTableModalComponent} from './contract-employee-lookup-table-modal.component';
    					import {ContractJobLookupTableModalComponent} from './contract-job-lookup-table-modal.component';
    					import {ContractProductLookupTableModalComponent} from './contract-product-lookup-table-modal.component';
    					import {ContractHubLookupTableModalComponent} from './contract-hub-lookup-table-modal.component';
    					import {ContractContactLookupTableModalComponent} from './contract-contact-lookup-table-modal.component';
import { ContractDashboardComponent } from './contract-dashboard/contract-dashboard.component';
import { CreateOrEditContractSignatureComponent } from './contract-dashboard/create-or-edit-contract-signature/create-or-edit-contract-signature.component';
    					


@NgModule({
    declarations: [
        ContractsComponent,
        CreateOrEditContractModalComponent,
        ViewContractModalComponent,
        
    					ContractContractTypeLookupTableModalComponent,
    					ContractStoreLookupTableModalComponent,
    					ContractBusinessLookupTableModalComponent,
    					ContractEmployeeLookupTableModalComponent,
    					ContractJobLookupTableModalComponent,
    					ContractProductLookupTableModalComponent,
    					ContractHubLookupTableModalComponent,
    					ContractContactLookupTableModalComponent,
         ContractDashboardComponent,
         CreateOrEditContractSignatureComponent,
    ],
    imports: [AppSharedModule, ContractRoutingModule , AdminSharedModule ],
    
})
export class ContractModule {
}
