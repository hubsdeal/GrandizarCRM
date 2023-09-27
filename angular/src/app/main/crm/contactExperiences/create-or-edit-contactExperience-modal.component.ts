import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactExperiencesServiceProxy, CreateOrEditContactExperienceDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactExperienceContactLookupTableModalComponent } from './contactExperience-contact-lookup-table-modal.component';
import { ContactExperienceEmployeeLookupTableModalComponent } from './contactExperience-employee-lookup-table-modal.component';
import { ContactExperienceBusinessLookupTableModalComponent } from './contactExperience-business-lookup-table-modal.component';
import { ContactExperienceCurrencyLookupTableModalComponent } from './contactExperience-currency-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContactExperienceModal',
    templateUrl: './create-or-edit-contactExperience-modal.component.html'
})
export class CreateOrEditContactExperienceModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactExperienceContactLookupTableModal', { static: true }) contactExperienceContactLookupTableModal: ContactExperienceContactLookupTableModalComponent;
    @ViewChild('contactExperienceEmployeeLookupTableModal', { static: true }) contactExperienceEmployeeLookupTableModal: ContactExperienceEmployeeLookupTableModalComponent;
    @ViewChild('contactExperienceBusinessLookupTableModal', { static: true }) contactExperienceBusinessLookupTableModal: ContactExperienceBusinessLookupTableModalComponent;
    @ViewChild('contactExperienceCurrencyLookupTableModal', { static: true }) contactExperienceCurrencyLookupTableModal: ContactExperienceCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactExperience: CreateOrEditContactExperienceDto = new CreateOrEditContactExperienceDto();

    contactFullName = '';
    employeeName = '';
    businessName = '';
    currencyName = '';



    constructor(
        injector: Injector,
        private _contactExperiencesServiceProxy: ContactExperiencesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contactExperienceId?: number): void {
    

        if (!contactExperienceId) {
            this.contactExperience = new CreateOrEditContactExperienceDto();
            this.contactExperience.id = contactExperienceId;
            this.contactExperience.startDate = this._dateTimeService.getStartOfDay();
            this.contactExperience.endDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.employeeName = '';
            this.businessName = '';
            this.currencyName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contactExperiencesServiceProxy.getContactExperienceForEdit(contactExperienceId).subscribe(result => {
                this.contactExperience = result.contactExperience;

                this.contactFullName = result.contactFullName;
                this.employeeName = result.employeeName;
                this.businessName = result.businessName;
                this.currencyName = result.currencyName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contactExperiencesServiceProxy.createOrEdit(this.contactExperience)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.contactExperienceContactLookupTableModal.id = this.contactExperience.contactId;
        this.contactExperienceContactLookupTableModal.displayName = this.contactFullName;
        this.contactExperienceContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contactExperienceEmployeeLookupTableModal.id = this.contactExperience.verifiedByEmployeeId;
        this.contactExperienceEmployeeLookupTableModal.displayName = this.employeeName;
        this.contactExperienceEmployeeLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.contactExperienceBusinessLookupTableModal.id = this.contactExperience.businessId;
        this.contactExperienceBusinessLookupTableModal.displayName = this.businessName;
        this.contactExperienceBusinessLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.contactExperienceCurrencyLookupTableModal.id = this.contactExperience.currencyId;
        this.contactExperienceCurrencyLookupTableModal.displayName = this.currencyName;
        this.contactExperienceCurrencyLookupTableModal.show();
    }


    setContactIdNull() {
        this.contactExperience.contactId = null;
        this.contactFullName = '';
    }
    setVerifiedByEmployeeIdNull() {
        this.contactExperience.verifiedByEmployeeId = null;
        this.employeeName = '';
    }
    setBusinessIdNull() {
        this.contactExperience.businessId = null;
        this.businessName = '';
    }
    setCurrencyIdNull() {
        this.contactExperience.currencyId = null;
        this.currencyName = '';
    }


    getNewContactId() {
        this.contactExperience.contactId = this.contactExperienceContactLookupTableModal.id;
        this.contactFullName = this.contactExperienceContactLookupTableModal.displayName;
    }
    getNewVerifiedByEmployeeId() {
        this.contactExperience.verifiedByEmployeeId = this.contactExperienceEmployeeLookupTableModal.id;
        this.employeeName = this.contactExperienceEmployeeLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.contactExperience.businessId = this.contactExperienceBusinessLookupTableModal.id;
        this.businessName = this.contactExperienceBusinessLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.contactExperience.currencyId = this.contactExperienceCurrencyLookupTableModal.id;
        this.currencyName = this.contactExperienceCurrencyLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
