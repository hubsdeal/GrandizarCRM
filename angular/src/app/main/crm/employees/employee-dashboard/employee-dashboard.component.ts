import { AfterViewInit, Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrls: ['./employee-dashboard.component.css']
})
export class EmployeeDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  employeeId: number;
  @ViewChild('profile_editor_tab') profileEditorTab: ElementRef<HTMLAnchorElement>;

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
    let employeeId = this.route.snapshot.paramMap.get('employeeId')
    this.employeeId = parseInt(employeeId);
  }
  ngAfterViewInit() {

  }
  activateTab(profile_editor_tab: ElementRef<HTMLAnchorElement>) {
    profile_editor_tab.nativeElement.click();
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

