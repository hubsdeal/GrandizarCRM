import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EmployeeTagRoutingModule } from './employeeTag-routing.module';
import { EmployeeTagsComponent } from './employeeTags.component';
import { CreateOrEditEmployeeTagModalComponent } from './create-or-edit-employeeTag-modal.component';
import { ViewEmployeeTagModalComponent } from './view-employeeTag-modal.component';
import { EmployeeTagEmployeeLookupTableModalComponent } from './employeeTag-employee-lookup-table-modal.component';
import { EmployeeTagMasterTagCategoryLookupTableModalComponent } from './employeeTag-masterTagCategory-lookup-table-modal.component';
import { EmployeeTagMasterTagLookupTableModalComponent } from './employeeTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        EmployeeTagsComponent,
        CreateOrEditEmployeeTagModalComponent,
        ViewEmployeeTagModalComponent,

        EmployeeTagEmployeeLookupTableModalComponent,
        EmployeeTagMasterTagCategoryLookupTableModalComponent,
        EmployeeTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, EmployeeTagRoutingModule, AdminSharedModule],
})
export class EmployeeTagModule {}
