import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CustomerLocalitiesZipCodeMapsServiceProxy, CreateOrEditCustomerLocalitiesZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CustomerLocalitiesZipCodeMapContactLookupTableModalComponent } from './customerLocalitiesZipCodeMap-contact-lookup-table-modal.component';
import { CustomerLocalitiesZipCodeMapZipCodeLookupTableModalComponent } from './customerLocalitiesZipCodeMap-zipCode-lookup-table-modal.component';
import { CustomerLocalitiesZipCodeMapCityLookupTableModalComponent } from './customerLocalitiesZipCodeMap-city-lookup-table-modal.component';
import { CustomerLocalitiesZipCodeMapStateLookupTableModalComponent } from './customerLocalitiesZipCodeMap-state-lookup-table-modal.component';
import { CustomerLocalitiesZipCodeMapCountryLookupTableModalComponent } from './customerLocalitiesZipCodeMap-country-lookup-table-modal.component';
import { CustomerLocalitiesZipCodeMapHubLookupTableModalComponent } from './customerLocalitiesZipCodeMap-hub-lookup-table-modal.component';



@Component({
    selector: 'createOrEditCustomerLocalitiesZipCodeMapModal',
    templateUrl: './create-or-edit-customerLocalitiesZipCodeMap-modal.component.html'
})
export class CreateOrEditCustomerLocalitiesZipCodeMapModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('customerLocalitiesZipCodeMapContactLookupTableModal', { static: true }) customerLocalitiesZipCodeMapContactLookupTableModal: CustomerLocalitiesZipCodeMapContactLookupTableModalComponent;
    @ViewChild('customerLocalitiesZipCodeMapZipCodeLookupTableModal', { static: true }) customerLocalitiesZipCodeMapZipCodeLookupTableModal: CustomerLocalitiesZipCodeMapZipCodeLookupTableModalComponent;
    @ViewChild('customerLocalitiesZipCodeMapCityLookupTableModal', { static: true }) customerLocalitiesZipCodeMapCityLookupTableModal: CustomerLocalitiesZipCodeMapCityLookupTableModalComponent;
    @ViewChild('customerLocalitiesZipCodeMapStateLookupTableModal', { static: true }) customerLocalitiesZipCodeMapStateLookupTableModal: CustomerLocalitiesZipCodeMapStateLookupTableModalComponent;
    @ViewChild('customerLocalitiesZipCodeMapCountryLookupTableModal', { static: true }) customerLocalitiesZipCodeMapCountryLookupTableModal: CustomerLocalitiesZipCodeMapCountryLookupTableModalComponent;
    @ViewChild('customerLocalitiesZipCodeMapHubLookupTableModal', { static: true }) customerLocalitiesZipCodeMapHubLookupTableModal: CustomerLocalitiesZipCodeMapHubLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    customerLocalitiesZipCodeMap: CreateOrEditCustomerLocalitiesZipCodeMapDto = new CreateOrEditCustomerLocalitiesZipCodeMapDto();

    contactFullName = '';
    zipCodeName = '';
    cityName = '';
    stateName = '';
    countryName = '';
    hubName = '';



    constructor(
        injector: Injector,
        private _customerLocalitiesZipCodeMapsServiceProxy: CustomerLocalitiesZipCodeMapsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(customerLocalitiesZipCodeMapId?: number): void {
    

        if (!customerLocalitiesZipCodeMapId) {
            this.customerLocalitiesZipCodeMap = new CreateOrEditCustomerLocalitiesZipCodeMapDto();
            this.customerLocalitiesZipCodeMap.id = customerLocalitiesZipCodeMapId;
            this.contactFullName = '';
            this.zipCodeName = '';
            this.cityName = '';
            this.stateName = '';
            this.countryName = '';
            this.hubName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._customerLocalitiesZipCodeMapsServiceProxy.getCustomerLocalitiesZipCodeMapForEdit(customerLocalitiesZipCodeMapId).subscribe(result => {
                this.customerLocalitiesZipCodeMap = result.customerLocalitiesZipCodeMap;

                this.contactFullName = result.contactFullName;
                this.zipCodeName = result.zipCodeName;
                this.cityName = result.cityName;
                this.stateName = result.stateName;
                this.countryName = result.countryName;
                this.hubName = result.hubName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._customerLocalitiesZipCodeMapsServiceProxy.createOrEdit(this.customerLocalitiesZipCodeMap)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContactModal() {
        this.customerLocalitiesZipCodeMapContactLookupTableModal.id = this.customerLocalitiesZipCodeMap.contactId;
        this.customerLocalitiesZipCodeMapContactLookupTableModal.displayName = this.contactFullName;
        this.customerLocalitiesZipCodeMapContactLookupTableModal.show();
    }
    openSelectZipCodeModal() {
        this.customerLocalitiesZipCodeMapZipCodeLookupTableModal.id = this.customerLocalitiesZipCodeMap.zipCodeId;
        this.customerLocalitiesZipCodeMapZipCodeLookupTableModal.displayName = this.zipCodeName;
        this.customerLocalitiesZipCodeMapZipCodeLookupTableModal.show();
    }
    openSelectCityModal() {
        this.customerLocalitiesZipCodeMapCityLookupTableModal.id = this.customerLocalitiesZipCodeMap.cityId;
        this.customerLocalitiesZipCodeMapCityLookupTableModal.displayName = this.cityName;
        this.customerLocalitiesZipCodeMapCityLookupTableModal.show();
    }
    openSelectStateModal() {
        this.customerLocalitiesZipCodeMapStateLookupTableModal.id = this.customerLocalitiesZipCodeMap.stateId;
        this.customerLocalitiesZipCodeMapStateLookupTableModal.displayName = this.stateName;
        this.customerLocalitiesZipCodeMapStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.customerLocalitiesZipCodeMapCountryLookupTableModal.id = this.customerLocalitiesZipCodeMap.countryId;
        this.customerLocalitiesZipCodeMapCountryLookupTableModal.displayName = this.countryName;
        this.customerLocalitiesZipCodeMapCountryLookupTableModal.show();
    }
    openSelectHubModal() {
        this.customerLocalitiesZipCodeMapHubLookupTableModal.id = this.customerLocalitiesZipCodeMap.hubId;
        this.customerLocalitiesZipCodeMapHubLookupTableModal.displayName = this.hubName;
        this.customerLocalitiesZipCodeMapHubLookupTableModal.show();
    }


    setContactIdNull() {
        this.customerLocalitiesZipCodeMap.contactId = null;
        this.contactFullName = '';
    }
    setZipCodeIdNull() {
        this.customerLocalitiesZipCodeMap.zipCodeId = null;
        this.zipCodeName = '';
    }
    setCityIdNull() {
        this.customerLocalitiesZipCodeMap.cityId = null;
        this.cityName = '';
    }
    setStateIdNull() {
        this.customerLocalitiesZipCodeMap.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.customerLocalitiesZipCodeMap.countryId = null;
        this.countryName = '';
    }
    setHubIdNull() {
        this.customerLocalitiesZipCodeMap.hubId = null;
        this.hubName = '';
    }


    getNewContactId() {
        this.customerLocalitiesZipCodeMap.contactId = this.customerLocalitiesZipCodeMapContactLookupTableModal.id;
        this.contactFullName = this.customerLocalitiesZipCodeMapContactLookupTableModal.displayName;
    }
    getNewZipCodeId() {
        this.customerLocalitiesZipCodeMap.zipCodeId = this.customerLocalitiesZipCodeMapZipCodeLookupTableModal.id;
        this.zipCodeName = this.customerLocalitiesZipCodeMapZipCodeLookupTableModal.displayName;
    }
    getNewCityId() {
        this.customerLocalitiesZipCodeMap.cityId = this.customerLocalitiesZipCodeMapCityLookupTableModal.id;
        this.cityName = this.customerLocalitiesZipCodeMapCityLookupTableModal.displayName;
    }
    getNewStateId() {
        this.customerLocalitiesZipCodeMap.stateId = this.customerLocalitiesZipCodeMapStateLookupTableModal.id;
        this.stateName = this.customerLocalitiesZipCodeMapStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.customerLocalitiesZipCodeMap.countryId = this.customerLocalitiesZipCodeMapCountryLookupTableModal.id;
        this.countryName = this.customerLocalitiesZipCodeMapCountryLookupTableModal.displayName;
    }
    getNewHubId() {
        this.customerLocalitiesZipCodeMap.hubId = this.customerLocalitiesZipCodeMapHubLookupTableModal.id;
        this.hubName = this.customerLocalitiesZipCodeMapHubLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
