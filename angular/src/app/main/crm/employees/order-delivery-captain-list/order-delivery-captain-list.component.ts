import { Component, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EmployeesServiceProxy, TokenAuthServiceProxy, EmployeeDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { NotifyService } from 'abp-ng2-module';
import { DateTime } from 'luxon';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { CreateOrEditEmployeeModalComponent } from '../create-or-edit-employee-modal.component';
import { ViewEmployeeModalComponent } from '../view-employee-modal.component';

@Component({
  selector: 'app-order-delivery-captain-list',
  templateUrl: './order-delivery-captain-list.component.html',
  styleUrls: ['./order-delivery-captain-list.component.css'],
  encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class OrderDeliveryCaptainListComponent extends AppComponentBase {
  @ViewChild('createOrEditEmployeeModal', { static: true })
  createOrEditEmployeeModal: CreateOrEditEmployeeModalComponent;
  @ViewChild('viewEmployeeModal', { static: true }) viewEmployeeModal: ViewEmployeeModalComponent;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  nameFilter = '';
  firstNameFilter = '';
  lastNameFilter = '';
  fullAddressFilter = '';
  addressFilter = '';
  zipCodeFilter = '';
  cityFilter = '';
  maxDateOfBirthFilter: DateTime;
  minDateOfBirthFilter: DateTime;
  mobileFilter = '';
  officePhoneFilter = '';
  personalEmailFilter = '';
  businessEmailFilter = '';
  jobTitleFilter = '';
  companyNameFilter = '';
  profileFilter = '';
  maxHireDateFilter: DateTime;
  minHireDateFilter: DateTime;
  facebookFilter = '';
  linkedInFilter = '';
  faxFilter = '';
  profilePictureIdFilter = '';
  currentEmployeeFilter = -1;
  stateNameFilter = '';
  countryNameFilter = '';
  contactFullNameFilter = '';

  constructor(
      injector: Injector,
      private _employeesServiceProxy: EmployeesServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService,
      private _dateTimeService: DateTimeService
  ) {
      super(injector);
  }

  getEmployees(event?: LazyLoadEvent) {
      if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
          if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
              return;
          }
      }

      this.primengTableHelper.showLoadingIndicator();

      this._employeesServiceProxy
          .getAll(
              this.filterText,
              this.nameFilter,
              this.firstNameFilter,
              this.lastNameFilter,
              this.fullAddressFilter,
              this.addressFilter,
              this.zipCodeFilter,
              this.cityFilter,
              this.maxDateOfBirthFilter === undefined
                  ? this.maxDateOfBirthFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxDateOfBirthFilter),
              this.minDateOfBirthFilter === undefined
                  ? this.minDateOfBirthFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minDateOfBirthFilter),
              this.mobileFilter,
              this.officePhoneFilter,
              this.personalEmailFilter,
              this.businessEmailFilter,
              this.jobTitleFilter,
              this.companyNameFilter,
              this.profileFilter,
              this.maxHireDateFilter === undefined
                  ? this.maxHireDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxHireDateFilter),
              this.minHireDateFilter === undefined
                  ? this.minHireDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minHireDateFilter),
              this.facebookFilter,
              this.linkedInFilter,
              this.faxFilter,
              this.profilePictureIdFilter,
              this.currentEmployeeFilter,
              this.stateNameFilter,
              this.countryNameFilter,
              this.contactFullNameFilter,
              undefined,//organizationUnitDisplayNameFilter
              undefined, //contactNameFilter
              -1, //currentEmployee
              undefined, //departmentIdFilter
              this.primengTableHelper.getSorting(this.dataTable),
              this.primengTableHelper.getSkipCount(this.paginator, event),
              this.primengTableHelper.getMaxResultCount(this.paginator, event)
          )
          .subscribe((result) => {
              this.primengTableHelper.totalRecordsCount = result.totalCount;
              this.primengTableHelper.records = result.items;
              this.primengTableHelper.hideLoadingIndicator();
          });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  createEmployee(): void {
      this.createOrEditEmployeeModal.show();
  }

  deleteEmployee(employee: EmployeeDto): void {
      this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
          if (isConfirmed) {
              this._employeesServiceProxy.delete(employee.id).subscribe(() => {
                  this.reloadPage();
                  this.notify.success(this.l('SuccessfullyDeleted'));
              });
          }
      });
  }

  exportToExcel(): void {
      this._employeesServiceProxy
          .getEmployeesToExcel(
              this.filterText,
              this.nameFilter,
              this.firstNameFilter,
              this.lastNameFilter,
              this.fullAddressFilter,
              this.addressFilter,
              this.zipCodeFilter,
              this.cityFilter,
              this.maxDateOfBirthFilter === undefined
                  ? this.maxDateOfBirthFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxDateOfBirthFilter),
              this.minDateOfBirthFilter === undefined
                  ? this.minDateOfBirthFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minDateOfBirthFilter),
              this.mobileFilter,
              this.officePhoneFilter,
              this.personalEmailFilter,
              this.businessEmailFilter,
              this.jobTitleFilter,
              this.companyNameFilter,
              this.profileFilter,
              this.maxHireDateFilter === undefined
                  ? this.maxHireDateFilter
                  : this._dateTimeService.getEndOfDayForDate(this.maxHireDateFilter),
              this.minHireDateFilter === undefined
                  ? this.minHireDateFilter
                  : this._dateTimeService.getStartOfDayForDate(this.minHireDateFilter),
              this.facebookFilter,
              this.linkedInFilter,
              this.faxFilter,
              this.profilePictureIdFilter,
              this.currentEmployeeFilter,
              this.stateNameFilter,
              this.countryNameFilter,
              this.contactFullNameFilter
          )
          .subscribe((result) => {
              this._fileDownloadService.downloadTempFile(result);
          });
  }

  resetFilters(): void {
      this.filterText = '';
      this.nameFilter = '';
      this.firstNameFilter = '';
      this.lastNameFilter = '';
      this.fullAddressFilter = '';
      this.addressFilter = '';
      this.zipCodeFilter = '';
      this.cityFilter = '';
      this.maxDateOfBirthFilter = undefined;
      this.minDateOfBirthFilter = undefined;
      this.mobileFilter = '';
      this.officePhoneFilter = '';
      this.personalEmailFilter = '';
      this.businessEmailFilter = '';
      this.jobTitleFilter = '';
      this.companyNameFilter = '';
      this.profileFilter = '';
      this.maxHireDateFilter = undefined;
      this.minHireDateFilter = undefined;
      this.facebookFilter = '';
      this.linkedInFilter = '';
      this.faxFilter = '';
      this.profilePictureIdFilter = '';
      this.currentEmployeeFilter = -1;
      this.stateNameFilter = '';
      this.countryNameFilter = '';
      this.contactFullNameFilter = '';

      this.getEmployees();
  }
}
