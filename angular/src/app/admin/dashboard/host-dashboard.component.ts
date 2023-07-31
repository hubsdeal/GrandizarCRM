import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
// import { AccordionModule } from 'primeng/accordion';
@Component({
    templateUrl: './host-dashboard.component.html',
    styleUrls: ['./host-dashboard.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class HostDashboardComponent extends AppComponentBase {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultHostDashboard;
    // @ViewChild('p-accordion', { static: true }) paginator: AccordionModule;
    constructor(injector: Injector) {
        super(injector);
    }
}
