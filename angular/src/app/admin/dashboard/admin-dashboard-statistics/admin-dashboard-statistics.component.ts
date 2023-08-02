import { Component, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CoreStatsAdminData, HostDashboardServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { result } from 'lodash-es';
// import { CoreStatsAdminData, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-admin-dashboard-statistics',
  templateUrl: './admin-dashboard-statistics.component.html',
  styleUrls: ['./admin-dashboard-statistics.component.css'],
  animations: [appModuleAnimation()],
})
export class AdminDashboardStatisticsComponent extends AppComponentBase  {
  CoreStatsData:CoreStatsAdminData;
  constructor(
    injector: Injector,
    private _coreStatsData:HostDashboardServiceProxy,
    private _tokenAuth: TokenAuthServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private _dateTimeService: DateTimeService
) {
    super(injector);
    this.getCoreStatsData();
}

getCoreStatsData(){
  this._coreStatsData.getCoreStatsAdminData().subscribe(result=>{
    this.CoreStatsData = result
  })
}

}
