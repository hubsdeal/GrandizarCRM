import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { RewardPointPriceSettingsServiceProxy, CreateOrEditRewardPointPriceSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { RewardPointPriceSettingCurrencyLookupTableModalComponent } from './rewardPointPriceSetting-currency-lookup-table-modal.component';



@Component({
    selector: 'createOrEditRewardPointPriceSettingModal',
    templateUrl: './create-or-edit-rewardPointPriceSetting-modal.component.html'
})
export class CreateOrEditRewardPointPriceSettingModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('rewardPointPriceSettingCurrencyLookupTableModal', { static: true }) rewardPointPriceSettingCurrencyLookupTableModal: RewardPointPriceSettingCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    rewardPointPriceSetting: CreateOrEditRewardPointPriceSettingDto = new CreateOrEditRewardPointPriceSettingDto();

    currencyName = '';



    constructor(
        injector: Injector,
        private _rewardPointPriceSettingsServiceProxy: RewardPointPriceSettingsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(rewardPointPriceSettingId?: number): void {
    

        if (!rewardPointPriceSettingId) {
            this.rewardPointPriceSetting = new CreateOrEditRewardPointPriceSettingDto();
            this.rewardPointPriceSetting.id = rewardPointPriceSettingId;
            this.currencyName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._rewardPointPriceSettingsServiceProxy.getRewardPointPriceSettingForEdit(rewardPointPriceSettingId).subscribe(result => {
                this.rewardPointPriceSetting = result.rewardPointPriceSetting;

                this.currencyName = result.currencyName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._rewardPointPriceSettingsServiceProxy.createOrEdit(this.rewardPointPriceSetting)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectCurrencyModal() {
        this.rewardPointPriceSettingCurrencyLookupTableModal.id = this.rewardPointPriceSetting.currencyId;
        this.rewardPointPriceSettingCurrencyLookupTableModal.displayName = this.currencyName;
        this.rewardPointPriceSettingCurrencyLookupTableModal.show();
    }


    setCurrencyIdNull() {
        this.rewardPointPriceSetting.currencyId = null;
        this.currencyName = '';
    }


    getNewCurrencyId() {
        this.rewardPointPriceSetting.currencyId = this.rewardPointPriceSettingCurrencyLookupTableModal.id;
        this.currencyName = this.rewardPointPriceSettingCurrencyLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
