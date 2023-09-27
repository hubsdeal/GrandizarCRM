import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ConnectMessagesServiceProxy, CreateOrEditConnectMessageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ConnectMessageConnectChannelLookupTableModalComponent } from './connectMessage-connectChannel-lookup-table-modal.component';
import { ConnectMessageUserLookupTableModalComponent } from './connectMessage-user-lookup-table-modal.component';
import { ConnectMessageContactLookupTableModalComponent } from './connectMessage-contact-lookup-table-modal.component';



@Component({
    selector: 'createOrEditConnectMessageModal',
    templateUrl: './create-or-edit-connectMessage-modal.component.html'
})
export class CreateOrEditConnectMessageModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('connectMessageConnectChannelLookupTableModal', { static: true }) connectMessageConnectChannelLookupTableModal: ConnectMessageConnectChannelLookupTableModalComponent;
    @ViewChild('connectMessageUserLookupTableModal', { static: true }) connectMessageUserLookupTableModal: ConnectMessageUserLookupTableModalComponent;
    @ViewChild('connectMessageContactLookupTableModal', { static: true }) connectMessageContactLookupTableModal: ConnectMessageContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    connectMessage: CreateOrEditConnectMessageDto = new CreateOrEditConnectMessageDto();

    connectChannelName = '';
    userName = '';
    contactFullName = '';



    constructor(
        injector: Injector,
        private _connectMessagesServiceProxy: ConnectMessagesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(connectMessageId?: number): void {
    

        if (!connectMessageId) {
            this.connectMessage = new CreateOrEditConnectMessageDto();
            this.connectMessage.id = connectMessageId;
            this.connectMessage.sendDate = this._dateTimeService.getStartOfDay();
            this.connectMessage.viewDate = this._dateTimeService.getStartOfDay();
            this.connectMessage.scheduleSendDateTime = this._dateTimeService.getStartOfDay();
            this.connectChannelName = '';
            this.userName = '';
            this.contactFullName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._connectMessagesServiceProxy.getConnectMessageForEdit(connectMessageId).subscribe(result => {
                this.connectMessage = result.connectMessage;

                this.connectChannelName = result.connectChannelName;
                this.userName = result.userName;
                this.contactFullName = result.contactFullName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._connectMessagesServiceProxy.createOrEdit(this.connectMessage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectConnectChannelModal() {
        this.connectMessageConnectChannelLookupTableModal.id = this.connectMessage.connectChannelId;
        this.connectMessageConnectChannelLookupTableModal.displayName = this.connectChannelName;
        this.connectMessageConnectChannelLookupTableModal.show();
    }
    openSelectUserModal() {
        this.connectMessageUserLookupTableModal.id = this.connectMessage.fromUserId;
        this.connectMessageUserLookupTableModal.displayName = this.userName;
        this.connectMessageUserLookupTableModal.show();
    }
    openSelectContactModal() {
        this.connectMessageContactLookupTableModal.id = this.connectMessage.sendToContactId;
        this.connectMessageContactLookupTableModal.displayName = this.contactFullName;
        this.connectMessageContactLookupTableModal.show();
    }


    setConnectChannelIdNull() {
        this.connectMessage.connectChannelId = null;
        this.connectChannelName = '';
    }
    setFromUserIdNull() {
        this.connectMessage.fromUserId = null;
        this.userName = '';
    }
    setSendToContactIdNull() {
        this.connectMessage.sendToContactId = null;
        this.contactFullName = '';
    }


    getNewConnectChannelId() {
        this.connectMessage.connectChannelId = this.connectMessageConnectChannelLookupTableModal.id;
        this.connectChannelName = this.connectMessageConnectChannelLookupTableModal.displayName;
    }
    getNewFromUserId() {
        this.connectMessage.fromUserId = this.connectMessageUserLookupTableModal.id;
        this.userName = this.connectMessageUserLookupTableModal.displayName;
    }
    getNewSendToContactId() {
        this.connectMessage.sendToContactId = this.connectMessageContactLookupTableModal.id;
        this.contactFullName = this.connectMessageContactLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
