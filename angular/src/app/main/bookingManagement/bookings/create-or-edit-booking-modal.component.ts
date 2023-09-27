import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BookingsServiceProxy, CreateOrEditBookingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BookingBookingTypeLookupTableModalComponent } from './booking-bookingType-lookup-table-modal.component';
import { BookingBookingStatusLookupTableModalComponent } from './booking-bookingStatus-lookup-table-modal.component';
import { BookingContactLookupTableModalComponent } from './booking-contact-lookup-table-modal.component';
import { BookingStoreLookupTableModalComponent } from './booking-store-lookup-table-modal.component';
import { BookingProductLookupTableModalComponent } from './booking-product-lookup-table-modal.component';
import { BookingOrderLookupTableModalComponent } from './booking-order-lookup-table-modal.component';



@Component({
    selector: 'createOrEditBookingModal',
    templateUrl: './create-or-edit-booking-modal.component.html'
})
export class CreateOrEditBookingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('bookingBookingTypeLookupTableModal', { static: true }) bookingBookingTypeLookupTableModal: BookingBookingTypeLookupTableModalComponent;
    @ViewChild('bookingBookingStatusLookupTableModal', { static: true }) bookingBookingStatusLookupTableModal: BookingBookingStatusLookupTableModalComponent;
    @ViewChild('bookingContactLookupTableModal', { static: true }) bookingContactLookupTableModal: BookingContactLookupTableModalComponent;
    @ViewChild('bookingStoreLookupTableModal', { static: true }) bookingStoreLookupTableModal: BookingStoreLookupTableModalComponent;
    @ViewChild('bookingProductLookupTableModal', { static: true }) bookingProductLookupTableModal: BookingProductLookupTableModalComponent;
    @ViewChild('bookingOrderLookupTableModal', { static: true }) bookingOrderLookupTableModal: BookingOrderLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    booking: CreateOrEditBookingDto = new CreateOrEditBookingDto();

    bookingTypeName = '';
    bookingStatusName = '';
    contactFullName = '';
    storeName = '';
    productName = '';
    orderInvoiceNumber = '';



    constructor(
        injector: Injector,
        private _bookingsServiceProxy: BookingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(bookingId?: number): void {
    

        if (!bookingId) {
            this.booking = new CreateOrEditBookingDto();
            this.booking.id = bookingId;
            this.booking.startDate = this._dateTimeService.getStartOfDay();
            this.booking.endDate = this._dateTimeService.getStartOfDay();
            this.booking.checkInDateTime = this._dateTimeService.getStartOfDay();
            this.booking.checkOutDateTime = this._dateTimeService.getStartOfDay();
            this.bookingTypeName = '';
            this.bookingStatusName = '';
            this.contactFullName = '';
            this.storeName = '';
            this.productName = '';
            this.orderInvoiceNumber = '';


            this.active = true;
            this.modal.show();
        } else {
            this._bookingsServiceProxy.getBookingForEdit(bookingId).subscribe(result => {
                this.booking = result.booking;

                this.bookingTypeName = result.bookingTypeName;
                this.bookingStatusName = result.bookingStatusName;
                this.contactFullName = result.contactFullName;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.orderInvoiceNumber = result.orderInvoiceNumber;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._bookingsServiceProxy.createOrEdit(this.booking)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectBookingTypeModal() {
        this.bookingBookingTypeLookupTableModal.id = this.booking.bookingTypeId;
        this.bookingBookingTypeLookupTableModal.displayName = this.bookingTypeName;
        this.bookingBookingTypeLookupTableModal.show();
    }
    openSelectBookingStatusModal() {
        this.bookingBookingStatusLookupTableModal.id = this.booking.bookingStatusId;
        this.bookingBookingStatusLookupTableModal.displayName = this.bookingStatusName;
        this.bookingBookingStatusLookupTableModal.show();
    }
    openSelectContactModal() {
        this.bookingContactLookupTableModal.id = this.booking.contactId;
        this.bookingContactLookupTableModal.displayName = this.contactFullName;
        this.bookingContactLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.bookingStoreLookupTableModal.id = this.booking.storeId;
        this.bookingStoreLookupTableModal.displayName = this.storeName;
        this.bookingStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.bookingProductLookupTableModal.id = this.booking.productId;
        this.bookingProductLookupTableModal.displayName = this.productName;
        this.bookingProductLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.bookingOrderLookupTableModal.id = this.booking.orderId;
        this.bookingOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.bookingOrderLookupTableModal.show();
    }


    setBookingTypeIdNull() {
        this.booking.bookingTypeId = null;
        this.bookingTypeName = '';
    }
    setBookingStatusIdNull() {
        this.booking.bookingStatusId = null;
        this.bookingStatusName = '';
    }
    setContactIdNull() {
        this.booking.contactId = null;
        this.contactFullName = '';
    }
    setStoreIdNull() {
        this.booking.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.booking.productId = null;
        this.productName = '';
    }
    setOrderIdNull() {
        this.booking.orderId = null;
        this.orderInvoiceNumber = '';
    }


    getNewBookingTypeId() {
        this.booking.bookingTypeId = this.bookingBookingTypeLookupTableModal.id;
        this.bookingTypeName = this.bookingBookingTypeLookupTableModal.displayName;
    }
    getNewBookingStatusId() {
        this.booking.bookingStatusId = this.bookingBookingStatusLookupTableModal.id;
        this.bookingStatusName = this.bookingBookingStatusLookupTableModal.displayName;
    }
    getNewContactId() {
        this.booking.contactId = this.bookingContactLookupTableModal.id;
        this.contactFullName = this.bookingContactLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.booking.storeId = this.bookingStoreLookupTableModal.id;
        this.storeName = this.bookingStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.booking.productId = this.bookingProductLookupTableModal.id;
        this.productName = this.bookingProductLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.booking.orderId = this.bookingOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.bookingOrderLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
