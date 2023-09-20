import { AfterViewInit, Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditEmployeeDto, EmployeesServiceProxy, HubCountryLookupTableDto, HubStateLookupTableDto, HubsServiceProxy, StatesServiceProxy, TaskEventsServiceProxy, TaskTeamEmployeeLookupTableDto, TaskTeamsServiceProxy, TaskWorkItemsServiceProxy } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { finalize } from 'rxjs';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

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

  saving = false;
  savingUser = false;

  stateOptions: any = [];
  countryOptions: any = [];
  employee: CreateOrEditEmployeeDto = new CreateOrEditEmployeeDto();

  private _uploaderOptions: FileUploaderOptions = {};
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  imageSrc: any = '';
  picture: string;
  fullName: string;

  stateName: string;
  countryName: string;
  isUser: boolean = false;
  expandBasicEditor: boolean;

  selectedCountry: any;
  selectedState: any;
  allCountrys: HubCountryLookupTableDto[];
  allStates: HubStateLookupTableDto[];
  employeeList: TaskTeamEmployeeLookupTableDto[] = [];
  selectedEmployees: TaskTeamEmployeeLookupTableDto[] = [];
  @Output() isReload: EventEmitter<boolean> = new EventEmitter<boolean>();
  workItems: any;
  taskEventList: any;
  advancedFiltersAreShown = false;
  filterText = '';
  nameFilter = '';
  estimatedHoursFilter = '';
  actualHoursFilter = '';
  maxStartDateFilter: DateTime;
  minStartDateFilter: DateTime;
  maxEndDateFilter: DateTime;
  minEndDateFilter: DateTime;
  startTimeFilter = '';
  endTimeFilter = '';
  openOrClosedFilter = -1;
  maxCompletionPercentageFilter: number;
  maxCompletionPercentageFilterEmpty: number;
  minCompletionPercentageFilter: number;
  minCompletionPercentageFilterEmpty: number;
  taskEventNameFilter = '';
  employeeNameFilter = '';

  sanitizedHtml: SafeHtml;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _employeeServiceProxy: EmployeesServiceProxy,
    private _hubsServiceProxy: HubsServiceProxy,
    private _stateServiceProxy: StatesServiceProxy,
    private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
    private _tokenService: TokenService,
    private _taskWorkItemsServiceProxy: TaskWorkItemsServiceProxy,
    private _dateTimeService: DateTimeService,
    private _taskEventsServiceProxy: TaskEventsServiceProxy,
    private sanitizer: DomSanitizer
  ) {
    super(injector);
    //this.sanitizedHtml = this.sanitizeHtml(this.rawHtml);
    this.loadAllDropdown();
  }

  ngOnInit(): void {
    let employeeId = this.route.snapshot.paramMap.get('employeeId')
    this.employeeId = parseInt(employeeId);
    this.getEmployeeDetails(this.employeeId);
    this.temporaryPictureUrl = '';
    this.initFileUploader();
    this.getTaskWorkItems();
    this.getTaskEvents();
  }

  getEmployeeDetails(id: number) {
    this._employeeServiceProxy.getEmployeeForEdit(id).subscribe(result => {
      this.employee = result.employee;
      this.fullName = result.employee.firstName + " " + result.employee.lastName;
      if (this.employee.countryId == null) {
        this.employee.countryId = AppConsts.defaultCountryId;
      }
      this.picture = result.picture;
      this.imageSrc = result.picture;
      this.stateName = result.stateName;
      this.countryName = result.countryName;
      this.isUser = result.isUser;
    });
  }

  loadAllDropdown() {
    this._hubsServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
      this.allCountrys = result;
    });
    this._taskTeamsServiceProxy.getAllEmployeeForLookupTable('', '', 0, 1000).subscribe(result => {
      this.employeeList = result.items;
    });
  }

  saveEmployee(fileToken?: string): void {
    this.saving = true;
    this.employee.fileToken = fileToken;
    this.employee.name = this.employee.firstName + " " + this.employee.lastName;


    //this.employee.departments = this.selectedDepartment;
    this._employeeServiceProxy.createOrEdit(this.employee)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe((result) => {
        // })
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }

  save() {
    if (this.uploader.queue != null && this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      this.saveEmployee();
    }
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

  onEmployeeSelect(event: any) {
  }

  onCountryChange(event: any) {
    if (event.value != null) {
      this.employee.countryId = event.value.id;
      this._stateServiceProxy.getAllStateForTableDropdown(event.value.id).subscribe((result) => {
        this.allStates = result;
      });
    }
    console.log("countryId" + this.selectedCountry.displayName);
  }

  initFileUploader(): void {

    this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/api/MediaUpload/UploadPicture' });
    this._uploaderOptions.autoUpload = false;
    this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
    this._uploaderOptions.removeAfterUpload = true;
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
      form.append('FileToken', this.guid());
    };

    this.uploader.onSuccessItem = (item, response, status) => {
      const resp = <IAjaxResponse>JSON.parse(response);
      if (resp.success) {
        this.saveEmployee(resp.result.fileToken);
      } else {
        this.message.error(resp.error.message);
      }
    };

    this.uploader.setOptions(this._uploaderOptions);
  }

  fileChangeEvent(event: any) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed

        this.imageSrc = event.target.result;
      }
    }
  }

  guid(): string {
    function s4() {
      return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
  }
  getTaskWorkItems() {
    // if (this.primengTableHelper.shouldResetPaging(event)) {
    //   this.paginator.changePage(0);
    //   return;
    // }

    // this.primengTableHelper.showLoadingIndicator();

    this._taskWorkItemsServiceProxy.getAllByTaskEventId(
      //this.taskEventId,
      undefined,
      this.filterText,
      this.nameFilter,
      this.estimatedHoursFilter,
      this.actualHoursFilter,
      this.maxStartDateFilter === undefined
        ? this.maxStartDateFilter
        : this._dateTimeService.getEndOfDayForDate(this.maxStartDateFilter),
      this.minStartDateFilter === undefined
        ? this.minStartDateFilter
        : this._dateTimeService.getStartOfDayForDate(this.minStartDateFilter),
      this.maxEndDateFilter === undefined
        ? this.maxEndDateFilter
        : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
      this.minEndDateFilter === undefined
        ? this.minEndDateFilter
        : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
      this.startTimeFilter,
      this.endTimeFilter,
      this.openOrClosedFilter,
      this.maxCompletionPercentageFilter == null
        ? this.maxCompletionPercentageFilterEmpty
        : this.maxCompletionPercentageFilter,
      this.minCompletionPercentageFilter == null
        ? this.minCompletionPercentageFilterEmpty
        : this.minCompletionPercentageFilter,
      this.taskEventNameFilter,
      this.employeeNameFilter,
      this.employeeId,
      '',
      0,
      50
      // this.primengTableHelper.getSorting(this.dataTable),
      // this.primengTableHelper.getSkipCount(this.paginator, event),
      // this.primengTableHelper.getMaxResultCount(this.paginator, event)
    ).subscribe(result => {
      //   this.primengTableHelper.totalRecordsCount = result.totalCount;
      //   this.primengTableHelper.records = result.items;
      //   this.totalCount.emit(this.primengTableHelper.totalRecordsCount);
      //this.isReload.emit(true);
      //this.primengTableHelper.hideLoadingIndicator();
      this.workItems = result.items

    });
  }
  getTaskEvents() {


    this._taskEventsServiceProxy
      .getAll(
        this.filterText,
        this.nameFilter,
        undefined,
        undefined,
        undefined,
        undefined,
        undefined,
        this.startTimeFilter,
        this.endTimeFilter,
        0,
        undefined,
        this.maxEndDateFilter === undefined
          ? this.maxEndDateFilter
          : this._dateTimeService.getEndOfDayForDate(this.maxEndDateFilter),
        this.minEndDateFilter === undefined
          ? this.minEndDateFilter
          : this._dateTimeService.getStartOfDayForDate(this.minEndDateFilter),
        undefined,
        undefined,
        undefined,
        this.employeeId,
        '',
        0,
        100
      )
      .subscribe((result) => {
        this.taskEventList = result.taskEvents;
      });
  }
  sanitizeHtml(input: string) {
    // Remove single and double quotation marks from the HTML content
    const modifiedHtml = input.replace(/['"]/g, '');
    this.sanitizedHtml =  this.sanitizer.bypassSecurityTrustHtml(modifiedHtml);
    return true;
  }
}

