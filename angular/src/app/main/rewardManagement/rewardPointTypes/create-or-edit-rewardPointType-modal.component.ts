import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { RewardPointTypesServiceProxy, CreateOrEditRewardPointTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditRewardPointTypeModal',
    templateUrl: './create-or-edit-rewardPointType-modal.component.html'
})
export class CreateOrEditRewardPointTypeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    rewardPointType: CreateOrEditRewardPointTypeDto = new CreateOrEditRewardPointTypeDto();




    constructor(
        injector: Injector,
        private _rewardPointTypesServiceProxy: RewardPointTypesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(rewardPointTypeId?: number): void {
    

        if (!rewardPointTypeId) {
            this.rewardPointType = new CreateOrEditRewardPointTypeDto();
            this.rewardPointType.id = rewardPointTypeId;


            this.active = true;
            this.modal.show();
        } else {
            this._rewardPointTypesServiceProxy.getRewardPointTypeForEdit(rewardPointTypeId).subscribe(result => {
                this.rewardPointType = result.rewardPointType;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._rewardPointTypesServiceProxy.createOrEdit(this.rewardPointType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
