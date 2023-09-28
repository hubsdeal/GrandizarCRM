import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { RewardPointAwardSettingsServiceProxy, CreateOrEditRewardPointAwardSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { RewardPointAwardSettingRewardPointTypeLookupTableModalComponent } from './rewardPointAwardSetting-rewardPointType-lookup-table-modal.component';
import { RewardPointAwardSettingStoreLookupTableModalComponent } from './rewardPointAwardSetting-store-lookup-table-modal.component';
import { RewardPointAwardSettingProductLookupTableModalComponent } from './rewardPointAwardSetting-product-lookup-table-modal.component';
import { RewardPointAwardSettingMembershipTypeLookupTableModalComponent } from './rewardPointAwardSetting-membershipType-lookup-table-modal.component';



@Component({
    selector: 'createOrEditRewardPointAwardSettingModal',
    templateUrl: './create-or-edit-rewardPointAwardSetting-modal.component.html'
})
export class CreateOrEditRewardPointAwardSettingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('rewardPointAwardSettingRewardPointTypeLookupTableModal', { static: true }) rewardPointAwardSettingRewardPointTypeLookupTableModal: RewardPointAwardSettingRewardPointTypeLookupTableModalComponent;
    @ViewChild('rewardPointAwardSettingStoreLookupTableModal', { static: true }) rewardPointAwardSettingStoreLookupTableModal: RewardPointAwardSettingStoreLookupTableModalComponent;
    @ViewChild('rewardPointAwardSettingProductLookupTableModal', { static: true }) rewardPointAwardSettingProductLookupTableModal: RewardPointAwardSettingProductLookupTableModalComponent;
    @ViewChild('rewardPointAwardSettingMembershipTypeLookupTableModal', { static: true }) rewardPointAwardSettingMembershipTypeLookupTableModal: RewardPointAwardSettingMembershipTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    rewardPointAwardSetting: CreateOrEditRewardPointAwardSettingDto = new CreateOrEditRewardPointAwardSettingDto();

    rewardPointTypeName = '';
    storeName = '';
    productName = '';
    membershipTypeName = '';



    constructor(
        injector: Injector,
        private _rewardPointAwardSettingsServiceProxy: RewardPointAwardSettingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(rewardPointAwardSettingId?: number): void {
    

        if (!rewardPointAwardSettingId) {
            this.rewardPointAwardSetting = new CreateOrEditRewardPointAwardSettingDto();
            this.rewardPointAwardSetting.id = rewardPointAwardSettingId;
            this.rewardPointTypeName = '';
            this.storeName = '';
            this.productName = '';
            this.membershipTypeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._rewardPointAwardSettingsServiceProxy.getRewardPointAwardSettingForEdit(rewardPointAwardSettingId).subscribe(result => {
                this.rewardPointAwardSetting = result.rewardPointAwardSetting;

                this.rewardPointTypeName = result.rewardPointTypeName;
                this.storeName = result.storeName;
                this.productName = result.productName;
                this.membershipTypeName = result.membershipTypeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._rewardPointAwardSettingsServiceProxy.createOrEdit(this.rewardPointAwardSetting)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectRewardPointTypeModal() {
        this.rewardPointAwardSettingRewardPointTypeLookupTableModal.id = this.rewardPointAwardSetting.rewardPointTypeId;
        this.rewardPointAwardSettingRewardPointTypeLookupTableModal.displayName = this.rewardPointTypeName;
        this.rewardPointAwardSettingRewardPointTypeLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.rewardPointAwardSettingStoreLookupTableModal.id = this.rewardPointAwardSetting.storeId;
        this.rewardPointAwardSettingStoreLookupTableModal.displayName = this.storeName;
        this.rewardPointAwardSettingStoreLookupTableModal.show();
    }
    openSelectProductModal() {
        this.rewardPointAwardSettingProductLookupTableModal.id = this.rewardPointAwardSetting.productId;
        this.rewardPointAwardSettingProductLookupTableModal.displayName = this.productName;
        this.rewardPointAwardSettingProductLookupTableModal.show();
    }
    openSelectMembershipTypeModal() {
        this.rewardPointAwardSettingMembershipTypeLookupTableModal.id = this.rewardPointAwardSetting.membershipTypeId;
        this.rewardPointAwardSettingMembershipTypeLookupTableModal.displayName = this.membershipTypeName;
        this.rewardPointAwardSettingMembershipTypeLookupTableModal.show();
    }


    setRewardPointTypeIdNull() {
        this.rewardPointAwardSetting.rewardPointTypeId = null;
        this.rewardPointTypeName = '';
    }
    setStoreIdNull() {
        this.rewardPointAwardSetting.storeId = null;
        this.storeName = '';
    }
    setProductIdNull() {
        this.rewardPointAwardSetting.productId = null;
        this.productName = '';
    }
    setMembershipTypeIdNull() {
        this.rewardPointAwardSetting.membershipTypeId = null;
        this.membershipTypeName = '';
    }


    getNewRewardPointTypeId() {
        this.rewardPointAwardSetting.rewardPointTypeId = this.rewardPointAwardSettingRewardPointTypeLookupTableModal.id;
        this.rewardPointTypeName = this.rewardPointAwardSettingRewardPointTypeLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.rewardPointAwardSetting.storeId = this.rewardPointAwardSettingStoreLookupTableModal.id;
        this.storeName = this.rewardPointAwardSettingStoreLookupTableModal.displayName;
    }
    getNewProductId() {
        this.rewardPointAwardSetting.productId = this.rewardPointAwardSettingProductLookupTableModal.id;
        this.productName = this.rewardPointAwardSettingProductLookupTableModal.displayName;
    }
    getNewMembershipTypeId() {
        this.rewardPointAwardSetting.membershipTypeId = this.rewardPointAwardSettingMembershipTypeLookupTableModal.id;
        this.membershipTypeName = this.rewardPointAwardSettingMembershipTypeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
