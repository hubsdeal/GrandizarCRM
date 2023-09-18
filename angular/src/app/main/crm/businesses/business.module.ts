import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessRoutingModule } from './business-routing.module';
import { BusinessesComponent } from './businesses.component';
import { CreateOrEditBusinessModalComponent } from './create-or-edit-business-modal.component';
import { ViewBusinessModalComponent } from './view-business-modal.component';
import { BusinessMediaLibraryLookupTableModalComponent } from './business-mediaLibrary-lookup-table-modal.component';
import { BusinessDashboardComponent } from './business-dashboard/business-dashboard.component';
import { MainModule } from '@app/main/main.module';
import { TaskEventModule } from '@app/main/taskManagement/taskEvents/taskEvent.module';

@NgModule({
    declarations: [
        BusinessesComponent,
        CreateOrEditBusinessModalComponent,
        ViewBusinessModalComponent,

        BusinessMediaLibraryLookupTableModalComponent,
        BusinessDashboardComponent,
    ],
    imports: [AppSharedModule, BusinessRoutingModule, AdminSharedModule, MainModule, 
        TaskEventModule
    ],
})
export class BusinessModule { }
