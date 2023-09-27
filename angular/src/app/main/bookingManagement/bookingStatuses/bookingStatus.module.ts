import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {BookingStatusRoutingModule} from './bookingStatus-routing.module';
import {BookingStatusesComponent} from './bookingStatuses.component';
import {CreateOrEditBookingStatusModalComponent} from './create-or-edit-bookingStatus-modal.component';
import {ViewBookingStatusModalComponent} from './view-bookingStatus-modal.component';



@NgModule({
    declarations: [
        BookingStatusesComponent,
        CreateOrEditBookingStatusModalComponent,
        ViewBookingStatusModalComponent,
        
    ],
    imports: [AppSharedModule, BookingStatusRoutingModule , AdminSharedModule ],
    
})
export class BookingStatusModule {
}
