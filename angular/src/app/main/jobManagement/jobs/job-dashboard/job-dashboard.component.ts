import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-job-dashboard',
  templateUrl: './job-dashboard.component.html',
  styleUrls: ['./job-dashboard.component.css']
})
export class JobDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  jobId: number;
  bindingData: any;
  remoteOnsiteOptions: SelectItem[];
  fullTimeOrGigWorkOptions: SelectItem[];
  selectBTN: boolean = false;
  description: string;
  salaryBasedOrFixedPriceOptions: SelectItem[];
  publishedOptions: SelectItem[];
  showCalendarView: boolean;
  showListView: boolean;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
    this.remoteOnsiteOptions = [{ label: 'Onsite/Local', value: false }, { label: 'Remote', value: true }];
    this.salaryBasedOrFixedPriceOptions = [{ label: 'Salary/Staffing Rate', value: true }, { label: 'Fixed Price', value: false }];
    this.fullTimeOrGigWorkOptions = [{ label: 'Full Time Job', value: true }, { label: 'Gig Work', value: false }];
    this.salaryBasedOrFixedPriceOptions = [{ label: 'Salary/Staffing Rate', value: true }, { label: 'Fixed Price', value: false }];
  }

  ngOnInit(): void {
    let jobId = this.route.snapshot.paramMap.get('jobId')
    this.jobId = parseInt(jobId);
  }
  ngAfterViewInit() {

  }

  openAiModal(fsf: string) {

  }

  onCalenderView() {
    this.showCalendarView = !this.showCalendarView;
    this.showListView = !this.showListView;
  }

  onListView() {
    this.showListView = !this.showListView;
    this.showCalendarView = !this.showCalendarView;
  }
}