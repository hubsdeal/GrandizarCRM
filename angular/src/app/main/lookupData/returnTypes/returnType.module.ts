import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ReturnTypeRoutingModule } from './returnType-routing.module';
import { ReturnTypesComponent } from './returnTypes.component';
import { CreateOrEditReturnTypeModalComponent } from './create-or-edit-returnType-modal.component';
import { ViewReturnTypeModalComponent } from './view-returnType-modal.component';

@NgModule({
    declarations: [ReturnTypesComponent, CreateOrEditReturnTypeModalComponent, ViewReturnTypeModalComponent],
    imports: [AppSharedModule, ReturnTypeRoutingModule, AdminSharedModule],
})
export class ReturnTypeModule {}
