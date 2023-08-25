import { Component, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditHubDocumentDto, HubDocumentHubLookupTableDto, HubDocumentsServiceProxy } from '@shared/service-proxies/service-proxies';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-hub-document',
  templateUrl: './hub-document.component.html',
  styleUrls: ['./hub-document.component.css']
})
export class HubDocumentComponent extends AppComponentBase implements OnInit {
  @Input() hubId: number;
  public uploader: FileUploader;
  private _uploaderOptions: FileUploaderOptions = {};
  allHubDocumentTypes: HubDocumentHubLookupTableDto[];
  hubDocument: CreateOrEditHubDocumentDto = new CreateOrEditHubDocumentDto();
  allDocuments:any[];
  fileName: string;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _hubDocumentsService: HubDocumentsServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getHubDocuments();
    this._hubDocumentsService.getAllDocumentTypeForLookupTable('', '', 0, 10000).subscribe((result) => {
      this.allHubDocumentTypes = result.items;
    });
    this.initFileUploader();
  }

  getHubDocuments(){
    this._hubDocumentsService.getAllByHubId(this.hubId,'','','','','','',0,10000).subscribe((result) => {
      this.allDocuments = result.items;
    })
  }
  deleteHubDocuments(id:number){
    this._hubDocumentsService.delete(id).subscribe(result => {
      this.notify.success(this.l('Deleted Successfully'));
      this.getHubDocuments();
    })
  }
  saveDocument(fileToken?: string): void {
    this.hubDocument.fileToken = fileToken;
    if (this.hubId) {
      this.hubDocument.hubId = this.hubId;
    }

    this._hubDocumentsService.createOrEdit(this.hubDocument)
      .pipe(finalize(() => { }))
      .subscribe(() => {
        this.hubDocument = new CreateOrEditHubDocumentDto();
        this.notify.info(this.l('SavedSuccessfully'));
        this.getHubDocuments();
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
      this.hubDocument.documentTitle = this.fileName;
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
