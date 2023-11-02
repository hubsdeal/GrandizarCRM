import { Component, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditStoreDocumentDto, StoreDocumentDocumentTypeLookupTableDto, StoreDocumentsServiceProxy } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-store-documents',
  templateUrl: './store-documents.component.html',
  styleUrls: ['./store-documents.component.css']
})
export class StoreDocumentsComponent extends AppComponentBase implements OnInit {
  @Input() storeId: number;
  public uploader: FileUploader;
  private _uploaderOptions: FileUploaderOptions = {};
  allstoreDocumentTypes: StoreDocumentDocumentTypeLookupTableDto[];
  storeDocument: CreateOrEditStoreDocumentDto = new CreateOrEditStoreDocumentDto();
  allDocuments: any[];
  fileName: string;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _storeDocumentsService: StoreDocumentsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getstoreDocuments();
    this._storeDocumentsService.getAllDocumentTypeForLookupTable('', '', 0, 10000).subscribe((result) => {
      this.allstoreDocumentTypes = result.items;
    });
    this.initFileUploader();
  }

  getstoreDocuments() {
    this._storeDocumentsService.getAllByStoreId(this.storeId, '', '', '', '', '', '', 0, 10000).subscribe((result) => {
      this.allDocuments = result.items;
    })
  }
  deletestoreDocuments(id: number) {
    this._storeDocumentsService.delete(id).subscribe(result => {
      this.notify.success(this.l('Deleted Successfully'));
      this.getstoreDocuments();
    })
  }
  saveDocument(fileToken?: string): void {
    this.storeDocument.fileToken = fileToken;
    if (this.storeId) {
      this.storeDocument.storeId = this.storeId;
    }

    this._storeDocumentsService.createOrEdit(this.storeDocument)
      .pipe(finalize(() => { }))
      .subscribe(() => {
        this.storeDocument = new CreateOrEditStoreDocumentDto();
        this.notify.info(this.l('SavedSuccessfully'));
        this.getstoreDocuments();
      });
  }
  addDocument() {
    if (this.uploader.queue != null && this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      this.saveDocument();
    }
  }

  fileChangeEvent(event: any) {
    if (event.target.files && event.target.files[0]) {
      this.fileName = event.target.files[0].name;
      this.storeDocument.documentTitle = this.fileName;
    }
  }

  initFileUploader(): void {

    this.uploader = new FileUploader({
      url: AppConsts.remoteServiceBaseUrl + '/DocumentUpload/UploadFile'
    });
    this._uploaderOptions.autoUpload = false;
    this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
    this._uploaderOptions.removeAfterUpload = true;
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
      form.append('FileToken', this.guid());
    };

    this.uploader.onSuccessItem = (item, response, status) => {
      const resp = <IAjaxResponse>JSON.parse(response);
      if (resp.success) {
        this.saveDocument(resp.result.fileToken);
      } else {
        this.message.error(resp.error.message);
      }
    };

    this.uploader.setOptions(this._uploaderOptions);
  }

  guid(): string {
    function s4() {
      return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
  }
  getFileExtension(filename) {
    return filename.split('.').pop();
  }
}
