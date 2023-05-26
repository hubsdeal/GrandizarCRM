import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DateTimeService } from '../common/timing/date-time.service';

@Component({
  selector: 'oneToOneConnectModal',
  templateUrl: './one-to-one-connect-modal.component.html',
  styleUrls: ['./one-to-one-connect-modal.component.scss']
})
export class OneToOneConnectModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('oneToOneConnectModal', { static: true }) modal: ModalDirective;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  contactId: number;
  employeeId: number;
  storeId: number;
  vendorId: number;
  companyId: number;
  dataRepoId:number;
  applicantId:number;
  salesLeadId:number;

  constructor(
    injector: Injector,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);
  }

  show(): void {
    this.active = true;
    this.modal.show();
  }

  save(): void {
    this.saving = true;
    this.notify.info(this.l('SavedSuccessfully'));
    this.close();
    this.modalSave.emit(null);
  }

  

  close(): void {
    this.active = false;
    this.modal.hide();
  }

  ngOnInit(): void { }

  
}
