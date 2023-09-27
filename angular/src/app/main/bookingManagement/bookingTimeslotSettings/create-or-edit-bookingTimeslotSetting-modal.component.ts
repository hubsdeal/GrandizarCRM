import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BookingTimeslotSettingsServiceProxy, CreateOrEditBookingTimeslotSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BookingTimeslotSettingStoreLookupTableModalComponent } from './bookingTimeslotSetting-store-lookup-table-modal.component';
import { BookingTimeslotSettingProductLookupTableModalComponent } from './bookingTimeslotSetting-product-lookup-table-modal.component';
import { BookingTimeslotSettingBookingTypeLookupTableModalComponent } from './bookingTimeslotSetting-bookingType-lookup-table-modal.component';
import { BookingTimeslotSettingBookingStatusLookupTableModalComponent } from './bookingTimeslotSetting-bookingStatus-lookup-table-modal.component';



@Component({
    selector: 'createOrEditBookingTimeslotSettingModal',
    templateUrl: './create-or-edit-bookingTimeslotSetting-modal.component.html'
})
export class CreateOrEditBookingTimeslotSettingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('bookingTimeslotSettingStoreLookupTableModal', { static: true }) bookingTimeslotSettingStoreLookupTableModal: BookingTimeslotSettingStoreLookupTableModalComponent;
    @ViewChild('bookingTimeslotSettingProductLookupTableModal', { static: true }) bookingTimeslotSettingProductLookupTableModal: BookingTimeslotSettingProductLookupTableModalComponent;
    @ViewChild('bookingTimeslotSettingBookingTypeLookupTableModal', { static: true }) bookingTimeslotSettingBookingTypeLookupTableModal: BookingTimeslotSettingBookingTypeLookupTableModalComponent;
    @ViewChild('bookingTimeslotSettingBookingStatusLookupTableModal', { static: true }) bookingTimeslotSettingBookingStatusLookupTableModal: BookingTimeslotSettingBookingStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    bookingTimeslotSetting: CreateOrEditBookingTimeslotSettingDto = new CreateOrEditBookingTimeslotSettingDto();

    storeName = '';
    productName = '';
    bookingTypeName = '';
    bookingStatusName = '';



    constructor(
        injector: Injector,
        private _bookingTimeslotSettingsServiceProxy: BookingTimeslotSettingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(bookingTimeslotSettingId?: number): void {
    

        if (!bookingTimeslotSettingId) {
            this.bookingTimeslotSetting = new CreateOrEditBookingTimeslotSettingDto();
            this.bookingTimeslotSetting.id = bookingTimeslotSettingId;
            this.bookingTimeslotSetting.startDate = this._dateTimeService.getStartOfDay();
            this.bookingTimeslotSetting.endDate = this._dateTimeService.getStartOfDay();
            this.storeName = '';
            this.productName = '';
            this.bookingTypeName = '';
            this.bookingStatusName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._bookingTimeslotSettingsServiceProxy.getBookingTimeslotSettingForEdit(bookingTimeslotSettingId).subscribe(result => {
                this.bookingTimeslotSetting = result.bookingTimeslotSetting;

                this.storeName = result.storeName;
                this.productName = result.productName;
                this.bookingTypeName = result.bookingTypeName;
                this.bookingStatusName = result.bookingStatusName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._bookingTimeslotSettingsServiceProxy.createOrEdit(this.bookingTimeslotSetting)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectStoreModal() {
        this.bookingTimeslotSettingStoreLookupTableModal.id = this.bookingTimeslotSetting.storeId;
        this.bookingTimeslotSettingStoreLookupTableModal.displayName = this.storeName;
        this.bookingTimeslotSettingStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.bookingTimeslotSettingProductLookupTableModal.id = this.bookingTimeslotSetting.productId;
        this.bookingTimeslotSettingProductLookupTableModal.displayName = this.productName;
        this.bookingTimeslotSettingProductLookupTableModal.show();
    }
    openSelectBookingTypeModal() {
        this.bookingTimeslotSettingBookingTypeLookupTableModal.id = this.bookingTimeslotSetting.bookingTypeId;
        this.bookingTimeslotSettingBookingTypeLookupTableModal.displayName = this.bookingTypeName;
        this.bookingTimeslotSettingBookingTypeLookupTableModal.show();
    }
    openSelectBookingStatusModal() {
        this.bookingTimeslotSettingBookingStatusLookupTableModal.id = this.bookingTimeslotSetting.bookingStatusId;
        this.bookingTimeslotSettingBookingStatusLookupTableModal.displayName = this.bookingStatusName;
        this.bookingTimeslotSettingBookingStatusLookupTableModal.show();
    }


    setStoreIdNull() {
        this.bookingTimeslotSetting.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.bookingTimeslotSetting.productId = null;
        this.productName = '';
    }
    setBookingTypeIdNull() {
        this.bookingTimeslotSetting.bookingTypeId = null;
        this.bookingTypeName = '';
    }
    setBookingStatusIdNull() {
        this.bookingTimeslotSetting.bookingStatusId = null;
        this.bookingStatusName = '';
    }


    getNewStoreId() {
        this.bookingTimeslotSetting.storeId = this.bookingTimeslotSettingStoreLookupTableModal.id;
        this.storeName = this.bookingTimeslotSettingStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.bookingTimeslotSetting.productId = this.bookingTimeslotSettingProductLookupTableModal.id;
        this.productName = this.bookingTimeslotSettingProductLookupTableModal.displayName;
    }
    getNewBookingTypeId() {
        this.bookingTimeslotSetting.bookingTypeId = this.bookingTimeslotSettingBookingTypeLookupTableModal.id;
        this.bookingTypeName = this.bookingTimeslotSettingBookingTypeLookupTableModal.displayName;
    }
    getNewBookingStatusId() {
        this.bookingTimeslotSetting.bookingStatusId = this.bookingTimeslotSettingBookingStatusLookupTableModal.id;
        this.bookingStatusName = this.bookingTimeslotSettingBookingStatusLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
