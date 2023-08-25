import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MediaLibrariesServiceProxy, MediaLibraryMasterTagLookupTableDto } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-edit-bulk-media-library-modal',
  templateUrl: './create-or-edit-bulk-media-library-modal.component.html',
  styleUrls: ['./create-or-edit-bulk-media-library-modal.component.css']
})
export class CreateOrEditBulkMediaLibraryModalComponent extends AppComponentBase implements OnInit {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  private _uploaderOptions: FileUploaderOptions = {};
  public uploader: FileUploader;
  public temporaryPictureUrl: string;

  active = false;
  saving = false;

  public hasBaseDropZoneOver: boolean = false;
  public hasAnotherDropZoneOver: boolean = false;

  allMasterTags: MediaLibraryMasterTagLookupTableDto[];

  masterTagId: number;
  seoTag: string;
  altTag: string;

  constructor(
    injector: Injector,
    private _tokenService: TokenService,
    private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.temporaryPictureUrl = '';
    //this.initFileUploader();
  }

  show(): void {
    this.initFileUploader();
    this.masterTagId=1;
    this.seoTag='Grandizar';
    this.altTag='photo';
    this._mediaLibrariesServiceProxy.getAllMasterTagForTableDropdown().subscribe(result => {
      this.allMasterTags = result;
    });
    this.active = true;
    this.modal.show();
  }

  save(): void {
    this.saving = true;
    this.close();
    this.modalSave.emit(null);
  }

  initFileUploader(): void {
    this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/api/MediaUpload/UploadBulkMediaToMediaLibrary' });
    this._uploaderOptions.autoUpload = false;
    this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
    this.uploader.setOptions(this._uploaderOptions);
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      file.url = AppConsts.remoteServiceBaseUrl + '/api/MediaUpload/UploadBulkMediaToMediaLibrary?seoTag=' + this.seoTag + '&altTag=' + this.altTag + '&mediaTypeId='+this.masterTagId;
    };

    this.uploader.onSuccessItem = (item, response, status) => {
      const resp = <IAjaxResponse>JSON.parse(response);
      if (resp.success) {

      } else {
        this.message.error(resp.error.message);
      }
    };
    this.uploader.onCompleteAll = () => {
      this.notify.info(this.l('SavedSuccessfully'));
      this.seoTag=null;
      this.altTag=null;
      this.close();
      this.modalSave.emit(null);

    };
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  close(): void {
    this.active = false;
    this.modal.hide();
  }
}