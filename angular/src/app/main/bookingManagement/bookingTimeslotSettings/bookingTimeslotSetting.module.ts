import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {BookingTimeslotSettingRoutingModule} from './bookingTimeslotSetting-routing.module';
import {BookingTimeslotSettingsComponent} from './bookingTimeslotSettings.component';
import {CreateOrEditBookingTimeslotSettingModalComponent} from './create-or-edit-bookingTimeslotSetting-modal.component';
import {ViewBookingTimeslotSettingModalComponent} from './view-bookingTimeslotSetting-modal.component';
import {BookingTimeslotSettingStoreLookupTableModalComponent} from './bookingTimeslotSetting-store-lookup-table-modal.component';
    					import {BookingTimeslotSettingProductLookupTableModalComponent} from './bookingTimeslotSetting-product-lookup-table-modal.component';
    					import {BookingTimeslotSettingBookingTypeLookupTableModalComponent} from './bookingTimeslotSetting-bookingType-lookup-table-modal.component';
    					import {BookingTimeslotSettingBookingStatusLookupTableModalComponent} from './bookingTimeslotSetting-bookingStatus-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        BookingTimeslotSettingsComponent,
        CreateOrEditBookingTimeslotSettingModalComponent,
        ViewBookingTimeslotSettingModalComponent,
        
    					BookingTimeslotSettingStoreLookupTableModalComponent,
    					BookingTimeslotSettingProductLookupTableModalComponent,
    					BookingTimeslotSettingBookingTypeLookupTableModalComponent,
    					BookingTimeslotSettingBookingStatusLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BookingTimeslotSettingRoutingModule , AdminSharedModule ],
    
})
export class BookingTimeslotSettingModule {
}
