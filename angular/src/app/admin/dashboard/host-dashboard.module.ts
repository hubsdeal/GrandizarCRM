import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { HostDashboardRoutingModule } from './host-dashboard-routing.module';
import { HostDashboardComponent } from './host-dashboard.component';
import { CustomizableDashboardModule } from '@app/shared/common/customizable-dashboard/customizable-dashboard.module';
import { AdminDashboardRecentStoreComponent } from './admin-dashboard-recent-store/admin-dashboard-recent-store.component';
import { AdminDashboardRecentTaskComponent } from './admin-dashboard-recent-task/admin-dashboard-recent-task.component';
import { AdminDashboardRecentOrdersComponent } from './admin-dashboard-recent-orders/admin-dashboard-recent-orders.component';
import { AccordionModule } from 'primeng/accordion';
import { AdminDashboardStatisticsComponent } from './admin-dashboard-statistics/admin-dashboard-statistics.component';
import { MainModule } from '@app/main/main.module';
import { TaskAndScheduleDashbaordComponent } from './task-and-schedule-dashbaord/task-and-schedule-dashbaord.component';
@NgModule({
    declarations: [HostDashboardComponent, AdminDashboardRecentStoreComponent, AdminDashboardRecentTaskComponent, AdminDashboardRecentOrdersComponent, AdminDashboardStatisticsComponent, TaskAndScheduleDashbaordComponent],
    imports: [AppSharedModule, AdminSharedModule, HostDashboardRoutingModule, CustomizableDashboardModule,AccordionModule, MainModule],
})
export class HostDashboardModule {}
