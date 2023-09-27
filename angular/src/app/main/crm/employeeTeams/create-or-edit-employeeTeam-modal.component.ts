import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeeTeamsServiceProxy, CreateOrEditEmployeeTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeTeamEmployeeLookupTableModalComponent } from './employeeTeam-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditEmployeeTeamModal',
    templateUrl: './create-or-edit-employeeTeam-modal.component.html'
})
export class CreateOrEditEmployeeTeamModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeTeamEmployeeLookupTableModal', { static: true }) employeeTeamEmployeeLookupTableModal: EmployeeTeamEmployeeLookupTableModalComponent;
    @ViewChild('employeeTeamEmployeeLookupTableModal2', { static: true }) employeeTeamEmployeeLookupTableModal2: EmployeeTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeTeam: CreateOrEditEmployeeTeamDto = new CreateOrEditEmployeeTeamDto();

    employeeName = '';
    employeeName2 = '';



    constructor(
        injector: Injector,
        private _employeeTeamsServiceProxy: EmployeeTeamsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(employeeTeamId?: number): void {
    

        if (!employeeTeamId) {
            this.employeeTeam = new CreateOrEditEmployeeTeamDto();
            this.employeeTeam.id = employeeTeamId;
            this.employeeName = '';
            this.employeeName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._employeeTeamsServiceProxy.getEmployeeTeamForEdit(employeeTeamId).subscribe(result => {
                this.employeeTeam = result.employeeTeam;

                this.employeeName = result.employeeName;
                this.employeeName2 = result.employeeName2;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._employeeTeamsServiceProxy.createOrEdit(this.employeeTeam)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectEmployeeModal() {
        this.employeeTeamEmployeeLookupTableModal.id = this.employeeTeam.primaryEmployeeId;
        this.employeeTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.employeeTeamEmployeeLookupTableModal.show();
    }
    openSelectEmployeeModal2() {
        this.employeeTeamEmployeeLookupTableModal2.id = this.employeeTeam.employeeId;
        this.employeeTeamEmployeeLookupTableModal2.displayName = this.employeeName;
        this.employeeTeamEmployeeLookupTableModal2.show();
    }


    setPrimaryEmployeeIdNull() {
        this.employeeTeam.primaryEmployeeId = null;
        this.employeeName = '';
    }
    setEmployeeIdNull() {
        this.employeeTeam.employeeId = null;
        this.employeeName2 = '';
    }


    getNewPrimaryEmployeeId() {
        this.employeeTeam.primaryEmployeeId = this.employeeTeamEmployeeLookupTableModal.id;
        this.employeeName = this.employeeTeamEmployeeLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.employeeTeam.employeeId = this.employeeTeamEmployeeLookupTableModal2.id;
        this.employeeName2 = this.employeeTeamEmployeeLookupTableModal2.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
