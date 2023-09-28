import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BookingRoutingModule } from './booking-routing.module';
import { BookingsComponent } from './bookings.component';
import { CreateOrEditBookingModalComponent } from './create-or-edit-booking-modal.component';
import { ViewBookingModalComponent } from './view-booking-modal.component';
import { BookingBookingTypeLookupTableModalComponent } from './booking-bookingType-lookup-table-modal.component';
import { BookingBookingStatusLookupTableModalComponent } from './booking-bookingStatus-lookup-table-modal.component';
import { BookingContactLookupTableModalComponent } from './booking-contact-lookup-table-modal.component';
import { BookingStoreLookupTableModalComponent } from './booking-store-lookup-table-modal.component';
import { BookingProductLookupTableModalComponent } from './booking-product-lookup-table-modal.component';
import { BookingOrderLookupTableModalComponent } from './booking-order-lookup-table-modal.component';



@NgModule({
	declarations: [
		BookingsComponent,
		CreateOrEditBookingModalComponent,
		ViewBookingModalComponent,

		BookingBookingTypeLookupTableModalComponent,
		BookingBookingStatusLookupTableModalComponent,
		BookingContactLookupTableModalComponent,
		BookingStoreLookupTableModalComponent,
		BookingProductLookupTableModalComponent,
		BookingOrderLookupTableModalComponent,
	],
	imports: [AppSharedModule, BookingRoutingModule, AdminSharedModule],

})
export class BookingModule {
}
