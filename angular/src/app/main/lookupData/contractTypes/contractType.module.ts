import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContractTypeRoutingModule } from './contractType-routing.module';
import { ContractTypesComponent } from './contractTypes.component';
import { CreateOrEditContractTypeModalComponent } from './create-or-edit-contractType-modal.component';
import { ViewContractTypeModalComponent } from './view-contractType-modal.component';

@NgModule({
    declarations: [ContractTypesComponent, CreateOrEditContractTypeModalComponent, ViewContractTypeModalComponent],
    imports: [AppSharedModule, ContractTypeRoutingModule, AdminSharedModule],
})
export class ContractTypeModule {}
