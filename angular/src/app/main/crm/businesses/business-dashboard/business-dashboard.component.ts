import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-business-dashboard',
  templateUrl: './business-dashboard.component.html',
  styleUrls: ['./business-dashboard.component.scss']
})
export class BusinessDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  businessId: number;
  
  showCalendarView: boolean;
  showListView: boolean;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let businessId = this.route.snapshot.paramMap.get('businessId')
    this.businessId = parseInt(businessId);
  }
  ngAfterViewInit() {

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
