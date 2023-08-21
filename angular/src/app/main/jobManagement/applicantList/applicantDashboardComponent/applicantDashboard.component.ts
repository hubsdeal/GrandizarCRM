import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContactsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTime } from 'luxon';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import {trim} from "lodash-es";


@Component({
    templateUrl: './applicantDashboard.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./applicantDashboard.component.scss'],
    animations: [appModuleAnimation()],
})
export class ApplicantDashboardComponent extends AppComponentBase {
    imageSrc: any;


}
