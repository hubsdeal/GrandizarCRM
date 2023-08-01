import { Component } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
  selector: 'app-admin-dashboard-statistics',
  templateUrl: './admin-dashboard-statistics.component.html',
  styleUrls: ['./admin-dashboard-statistics.component.css'],
  animations: [appModuleAnimation()],
})
export class AdminDashboardStatisticsComponent extends AppComponentBase  {

}
