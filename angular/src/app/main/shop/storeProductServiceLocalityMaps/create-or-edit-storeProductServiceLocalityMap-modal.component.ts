import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreProductServiceLocalityMapsServiceProxy, CreateOrEditStoreProductServiceLocalityMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreProductServiceLocalityMapProductLookupTableModalComponent } from './storeProductServiceLocalityMap-product-lookup-table-modal.component';
import { StoreProductServiceLocalityMapStoreLookupTableModalComponent } from './storeProductServiceLocalityMap-store-lookup-table-modal.component';
import { StoreProductServiceLocalityMapZipCodeLookupTableModalComponent } from './storeProductServiceLocalityMap-zipCode-lookup-table-modal.component';
import { StoreProductServiceLocalityMapCityLookupTableModalComponent } from './storeProductServiceLocalityMap-city-lookup-table-modal.component';
import { StoreProductServiceLocalityMapStateLookupTableModalComponent } from './storeProductServiceLocalityMap-state-lookup-table-modal.component';
import { StoreProductServiceLocalityMapCountryLookupTableModalComponent } from './storeProductServiceLocalityMap-country-lookup-table-modal.component';
import { StoreProductServiceLocalityMapHubLookupTableModalComponent } from './storeProductServiceLocalityMap-hub-lookup-table-modal.component';



@Component({
    selector: 'createOrEditStoreProductServiceLocalityMapModal',
    templateUrl: './create-or-edit-storeProductServiceLocalityMap-modal.component.html'
})
export class CreateOrEditStoreProductServiceLocalityMapModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeProductServiceLocalityMapProductLookupTableModal', { static: true }) storeProductServiceLocalityMapProductLookupTableModal: StoreProductServiceLocalityMapProductLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapStoreLookupTableModal', { static: true }) storeProductServiceLocalityMapStoreLookupTableModal: StoreProductServiceLocalityMapStoreLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapZipCodeLookupTableModal', { static: true }) storeProductServiceLocalityMapZipCodeLookupTableModal: StoreProductServiceLocalityMapZipCodeLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapCityLookupTableModal', { static: true }) storeProductServiceLocalityMapCityLookupTableModal: StoreProductServiceLocalityMapCityLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapStateLookupTableModal', { static: true }) storeProductServiceLocalityMapStateLookupTableModal: StoreProductServiceLocalityMapStateLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapCountryLookupTableModal', { static: true }) storeProductServiceLocalityMapCountryLookupTableModal: StoreProductServiceLocalityMapCountryLookupTableModalComponent;
    @ViewChild('storeProductServiceLocalityMapHubLookupTableModal', { static: true }) storeProductServiceLocalityMapHubLookupTableModal: StoreProductServiceLocalityMapHubLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeProductServiceLocalityMap: CreateOrEditStoreProductServiceLocalityMapDto = new CreateOrEditStoreProductServiceLocalityMapDto();

    productName = '';
    storeName = '';
    zipCodeName = '';
    cityName = '';
    stateName = '';
    countryName = '';
    hubName = '';



    constructor(
        injector: Injector,
        private _storeProductServiceLocalityMapsServiceProxy: StoreProductServiceLocalityMapsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(storeProductServiceLocalityMapId?: number): void {
    

        if (!storeProductServiceLocalityMapId) {
            this.storeProductServiceLocalityMap = new CreateOrEditStoreProductServiceLocalityMapDto();
            this.storeProductServiceLocalityMap.id = storeProductServiceLocalityMapId;
            this.productName = '';
            this.storeName = '';
            this.zipCodeName = '';
            this.cityName = '';
            this.stateName = '';
            this.countryName = '';
            this.hubName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._storeProductServiceLocalityMapsServiceProxy.getStoreProductServiceLocalityMapForEdit(storeProductServiceLocalityMapId).subscribe(result => {
                this.storeProductServiceLocalityMap = result.storeProductServiceLocalityMap;

                this.productName = result.productName;
                this.storeName = result.storeName;
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
            
			
			
            this._storeProductServiceLocalityMapsServiceProxy.createOrEdit(this.storeProductServiceLocalityMap)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProductModal() {
        this.storeProductServiceLocalityMapProductLookupTableModal.id = this.storeProductServiceLocalityMap.productId;
        this.storeProductServiceLocalityMapProductLookupTableModal.displayName = this.productName;
        this.storeProductServiceLocalityMapProductLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.storeProductServiceLocalityMapStoreLookupTableModal.id = this.storeProductServiceLocalityMap.storeId;
        this.storeProductServiceLocalityMapStoreLookupTableModal.displayName = this.storeName;
        this.storeProductServiceLocalityMapStoreLookupTableModal.show();
    }
    openSelectZipCodeModal() {
        this.storeProductServiceLocalityMapZipCodeLookupTableModal.id = this.storeProductServiceLocalityMap.zipCodeId;
        this.storeProductServiceLocalityMapZipCodeLookupTableModal.displayName = this.zipCodeName;
        this.storeProductServiceLocalityMapZipCodeLookupTableModal.show();
    }
    openSelectCityModal() {
        this.storeProductServiceLocalityMapCityLookupTableModal.id = this.storeProductServiceLocalityMap.cityId;
        this.storeProductServiceLocalityMapCityLookupTableModal.displayName = this.cityName;
        this.storeProductServiceLocalityMapCityLookupTableModal.show();
    }
    openSelectStateModal() {
        this.storeProductServiceLocalityMapStateLookupTableModal.id = this.storeProductServiceLocalityMap.stateId;
        this.storeProductServiceLocalityMapStateLookupTableModal.displayName = this.stateName;
        this.storeProductServiceLocalityMapStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.storeProductServiceLocalityMapCountryLookupTableModal.id = this.storeProductServiceLocalityMap.countryId;
        this.storeProductServiceLocalityMapCountryLookupTableModal.displayName = this.countryName;
        this.storeProductServiceLocalityMapCountryLookupTableModal.show();
    }
    openSelectHubModal() {
        this.storeProductServiceLocalityMapHubLookupTableModal.id = this.storeProductServiceLocalityMap.hubId;
        this.storeProductServiceLocalityMapHubLookupTableModal.displayName = this.hubName;
        this.storeProductServiceLocalityMapHubLookupTableModal.show();
    }


    setProductIdNull() {
        this.storeProductServiceLocalityMap.productId = null;
        this.productName = '';
    }
    setStoreIdNull() {
        this.storeProductServiceLocalityMap.storeId = null;
        this.storeName = '';
    }
    setZipCodeIdNull() {
        this.storeProductServiceLocalityMap.zipCodeId = null;
        this.zipCodeName = '';
    }
    setCityIdNull() {
        this.storeProductServiceLocalityMap.cityId = null;
        this.cityName = '';
    }
    setStateIdNull() {
        this.storeProductServiceLocalityMap.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.storeProductServiceLocalityMap.countryId = null;
        this.countryName = '';
    }
    setHubIdNull() {
        this.storeProductServiceLocalityMap.hubId = null;
        this.hubName = '';
    }


    getNewProductId() {
        this.storeProductServiceLocalityMap.productId = this.storeProductServiceLocalityMapProductLookupTableModal.id;
        this.productName = this.storeProductServiceLocalityMapProductLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.storeProductServiceLocalityMap.storeId = this.storeProductServiceLocalityMapStoreLookupTableModal.id;
        this.storeName = this.storeProductServiceLocalityMapStoreLookupTableModal.displayName;
    }
    getNewZipCodeId() {
        this.storeProductServiceLocalityMap.zipCodeId = this.storeProductServiceLocalityMapZipCodeLookupTableModal.id;
        this.zipCodeName = this.storeProductServiceLocalityMapZipCodeLookupTableModal.displayName;
    }
    getNewCityId() {
        this.storeProductServiceLocalityMap.cityId = this.storeProductServiceLocalityMapCityLookupTableModal.id;
        this.cityName = this.storeProductServiceLocalityMapCityLookupTableModal.displayName;
    }
    getNewStateId() {
        this.storeProductServiceLocalityMap.stateId = this.storeProductServiceLocalityMapStateLookupTableModal.id;
        this.stateName = this.storeProductServiceLocalityMapStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.storeProductServiceLocalityMap.countryId = this.storeProductServiceLocalityMapCountryLookupTableModal.id;
        this.countryName = this.storeProductServiceLocalityMapCountryLookupTableModal.displayName;
    }
    getNewHubId() {
        this.storeProductServiceLocalityMap.hubId = this.storeProductServiceLocalityMapHubLookupTableModal.id;
        this.hubName = this.storeProductServiceLocalityMapHubLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
