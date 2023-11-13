import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContractsServiceProxy } from '@shared/service-proxies/service-proxies';
import { SignaturePad } from 'angular2-signaturepad';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'createOrEditContractSignature',
  templateUrl: './create-or-edit-contract-signature.component.html',
  styleUrls: ['./create-or-edit-contract-signature.component.scss']
})
export class CreateOrEditContractSignatureComponent extends AppComponentBase implements OnInit {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  active = false;
  saving = false;

  @ViewChild(SignaturePad) signaturePad: SignaturePad;

  showSignature: boolean = false;
  showSignatory: boolean = false;
  showReference: boolean = false;

  id: number;

  public signaturePadOptions: Object = { // passed through to szimek/signature_pad constructor
    'minWidth': 0.5,
    'canvasWidth': 765,
    'canvasHeight': 200,
    'penColor': "rgb(0,0,0)",
    'backgroundColor': "rgb(255,255,255)"
  }

  public _signature: any = null;

  public propagateChange: Function = null;

  get signature(): any {
    return this._signature;
  }

  set signature(value: any) {
    this._signature = value;
  }




  contractId: number;

  private _uploaderOptions: FileUploaderOptions = {};
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  imageSrc: any = '';


  constructor(
    injector: Injector,
    private _contractsServiceProxy: ContractsServiceProxy,
    private _dateTimeService: DateTimeService
  ) {
    super(injector);
  }

  ngAfterViewInit() {
    this.signaturePad;
    // this.signaturePad.clear();
  }

  public writeValue(value: any): void {
    if (!value) {
      return;
    }
    this._signature = value;
    this.signaturePad.fromDataURL(this.signature);
  }


  public drawBegin(): void {
    console.log('Begin Drawing');
  }

  public drawComplete(): void {
    // this.signature = this.signaturePad.toDataURL('image/jpeg', 0.5);
    // this.contractSign.base64String = this.signature.split('data:image/jpeg;base64,')[1];
  }


  drawStart() {
    // will be notified of szimek/signature_pad's onBegin event
    console.log('begin drawing');
  }


  show(contractId?: number): void {


    if (!contractId) {
      this.active = true;
      this.modal.show();
    } else {
      this._contractsServiceProxy.getContractForEdit(contractId).subscribe(result => {
        this.active = true;
        this.modal.show();
      });
    }


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

  ngOnInit(): void {

  }
}
