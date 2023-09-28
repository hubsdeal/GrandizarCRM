import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {DeliveryTypeRoutingModule} from './deliveryType-routing.module';
import {DeliveryTypesComponent} from './deliveryTypes.component';
import {CreateOrEditDeliveryTypeModalComponent} from './create-or-edit-deliveryType-modal.component';
import {ViewDeliveryTypeModalComponent} from './view-deliveryType-modal.component';



@NgModule({
    declarations: [
        DeliveryTypesComponent,
        CreateOrEditDeliveryTypeModalComponent,
        ViewDeliveryTypeModalComponent,
        
    ],
    imports: [AppSharedModule, DeliveryTypeRoutingModule , AdminSharedModule ],
    
})
export class DeliveryTypeModule {
}
