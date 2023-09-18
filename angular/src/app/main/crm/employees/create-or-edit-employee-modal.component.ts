import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeesServiceProxy, CreateOrEditEmployeeDto, CreateOrEditMediaLibraryDto, TaskTeamEmployeeLookupTableDto, TaskTeamsServiceProxy, StatesServiceProxy, HubCountryLookupTableDto, HubStateLookupTableDto, HubsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeStateLookupTableModalComponent } from './employee-state-lookup-table-modal.component';
import { EmployeeCountryLookupTableModalComponent } from './employee-country-lookup-table-modal.component';
import { EmployeeContactLookupTableModalComponent } from './employee-contact-lookup-table-modal.component';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';

@Component({
    selector: 'createOrEditEmployeeModal',
    templateUrl: './create-or-edit-employee-modal.component.html',
})
export class CreateOrEditEmployeeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeStateLookupTableModal', { static: true })
    employeeStateLookupTableModal: EmployeeStateLookupTableModalComponent;
    @ViewChild('employeeCountryLookupTableModal', { static: true })
    employeeCountryLookupTableModal: EmployeeCountryLookupTableModalComponent;
    @ViewChild('employeeContactLookupTableModal', { static: true })
    employeeContactLookupTableModal: EmployeeContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employee: CreateOrEditEmployeeDto = new CreateOrEditEmployeeDto();

    stateName = '';
    countryName = '';
    contactFullName = '';
    imageSrc: any = '/assets/common/images/sampleProfilePics/noimg.png';

    private _uploaderOptions: FileUploaderOptions = {};
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();

    @ViewChild('mobile') mobile: ElementRef;
    @ViewChild('phone') phone: ElementRef;
    mediaLibraryName = '';

    employeeList: TaskTeamEmployeeLookupTableDto[] = [];
    selectedEmployees: TaskTeamEmployeeLookupTableDto[] = [];

    selectedCountry: any;
    selectedState: any;

    allCountrys: HubCountryLookupTableDto[];
    allStates: HubStateLookupTableDto[];
    constructor(
        injector: Injector,
        private _employeesServiceProxy: EmployeesServiceProxy,
        private _dateTimeService: DateTimeService,
        private _stateServiceProxy: StatesServiceProxy,
        private _taskTeamsServiceProxy: TaskTeamsServiceProxy,
        private _hubsServiceProxy: HubsServiceProxy,
        private _tokenService: TokenService,
       // private _employeeDepartmentMapServiceProxy: EmployeeDepartmentMapsServiceProxy
    ) {
        super(injector);
    }

    show(employeeId?: number): void {
        if (!employeeId) {
            this.employee = new CreateOrEditEmployeeDto();
            this.employee.id = employeeId;
            this.employee.dateOfBirth = this._dateTimeService.getStartOfDay();
            this.employee.hireDate = this._dateTimeService.getStartOfDay();
            this.stateName = '';
            this.countryName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeesServiceProxy.getEmployeeForEdit(employeeId).subscribe((result) => {
                this.employee = result.employee;

                this.stateName = result.stateName;
                this.countryName = result.countryName;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
        this._hubsServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._taskTeamsServiceProxy.getAllEmployeeForLookupTable('','',0,1000).subscribe(result => {
            this.employeeList = result.items;
        });
    }

    // save(): void {
    //     this.saving = true;

    //     this._employeesServiceProxy
    //         .createOrEdit(this.employee)
    //         .pipe(
    //             finalize(() => {
    //                 this.saving = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.close();
    //             this.modalSave.emit(null);
    //         });
    // }
    saveEmployee(fileToken?: string): void {
        this.saving = true;
        this.employee.fileToken = fileToken;
        this.employee.name = this.employee.firstName + " " + this.employee.lastName;
       

        //this.employee.departments = this.selectedDepartment;
        this._employeesServiceProxy.createOrEdit(this.employee)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe((result) => {
                // this._employeeDepartmentMapServiceProxy.saveMultipleDepartmentByEmployee(result,this.selectedDepartment.map(x=>x.id)).subscribe(r=>{
                //     this.notify.info(this.l('SavedSuccessfully'));
                //     this.close();
                //     this.modalSave.emit(null);
                // })
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveEmployee();
        }
    }

    openSelectStateModal() {
        this.employeeStateLookupTableModal.id = this.employee.stateId;
        this.employeeStateLookupTableModal.displayName = this.stateName;
        this.employeeStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.employeeCountryLookupTableModal.id = this.employee.countryId;
        this.employeeCountryLookupTableModal.displayName = this.countryName;
        this.employeeCountryLookupTableModal.show();
    }
    openSelectContactModal() {
        this.employeeContactLookupTableModal.id = this.employee.contactId;
        this.employeeContactLookupTableModal.displayName = this.contactFullName;
        this.employeeContactLookupTableModal.show();
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

    // saveAndMakeUser() {
    //     this.savingUser = true;

    //     this.employee.isRequestToMakeUser = true;
    //     this.employee.name = this.employee.firstName + " " + this.employee.lastName;
    //     this._employeesServiceProxy.createOrEdit(this.employee)
    //         .pipe(finalize(() => { this.saving = false; }))
    //         .subscribe((result) => {
    //             this._employeeDepartmentMapServiceProxy.saveMultipleDepartmentByEmployee(result,this.selectedDepartment.map(x=>x.id)).subscribe(r=>{
    //                 this.notify.info(this.l('SavedSuccessfully'));
    //                 this.close();
    //                 this.modalSave.emit(null);
    //                 this.employee.isRequestToMakeUser = false;
    //             })
               
    //         });
    // }
    setStateIdNull() {
        this.employee.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.employee.countryId = null;
        this.countryName = '';
    }
    setContactIdNull() {
        this.employee.contactId = null;
        this.contactFullName = '';
    }

    getNewStateId() {
        this.employee.stateId = this.employeeStateLookupTableModal.id;
        this.stateName = this.employeeStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.employee.countryId = this.employeeCountryLookupTableModal.id;
        this.countryName = this.employeeCountryLookupTableModal.displayName;
    }
    getNewContactId() {
        this.employee.contactId = this.employeeContactLookupTableModal.id;
        this.contactFullName = this.employeeContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this.initFileUploader();
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
}
