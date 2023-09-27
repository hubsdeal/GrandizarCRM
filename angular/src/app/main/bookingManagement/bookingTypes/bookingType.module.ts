import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {BookingTypeRoutingModule} from './bookingType-routing.module';
import {BookingTypesComponent} from './bookingTypes.component';
import {CreateOrEditBookingTypeModalComponent} from './create-or-edit-bookingType-modal.component';
import {ViewBookingTypeModalComponent} from './view-bookingType-modal.component';



@NgModule({
    declarations: [
        BookingTypesComponent,
        CreateOrEditBookingTypeModalComponent,
        ViewBookingTypeModalComponent,
        
    ],
    imports: [AppSharedModule, BookingTypeRoutingModule , AdminSharedModule ],
    
})
export class BookingTypeModule {
}
