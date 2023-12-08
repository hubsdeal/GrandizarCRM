import { AfterViewInit, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditTaskDocumentDto, DocumentTypesServiceProxy, GetDocumentTypeForViewDto, GetTaskEventForViewDto, TaskDocumentsServiceProxy, TaskEventsServiceProxy, TaskTagsServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { finalize } from 'rxjs/operators';
import { CreateOrEditTaskEventModalComponent } from '../create-or-edit-taskEvent-modal.component';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { CreateOrEditTaskTagModalComponent } from '../../taskTags/create-or-edit-taskTag-modal.component';
import { ViewTaskTagModalComponent } from '../../taskTags/view-taskTag-modal.component';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-task-events-dashboard',
  templateUrl: './task-events-dashboard.component.html',
  styleUrls: ['./task-events-dashboard.component.css'],
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class TaskEventsDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  @ViewChild('createOrEditTaskEventModal', { static: true })
  createOrEditTaskEventModal: CreateOrEditTaskEventModalComponent;

   @ViewChild('createOrEditTaskTagModal', { static: true })
    createOrEditTaskTagModal: CreateOrEditTaskTagModalComponent;
    @ViewChild('viewTaskTagModal', { static: true }) viewTaskTagModal: ViewTaskTagModalComponent;
    
    safeHtmlContent: SafeHtml;

  taskEventId: number;
  taskEvent: GetTaskEventForViewDto = new GetTaskEventForViewDto();
  docTypes: GetDocumentTypeForViewDto[];
  taskDocument: CreateOrEditTaskDocumentDto = new CreateOrEditTaskDocumentDto();
  private _uploaderOptions: FileUploaderOptions = {};
  public uploader: FileUploader;
  fileName: string;
  taskDocuments: any;
  advancedFiltersAreShown = false;
  filterText = '';
  nameFilter = '';
  fileBinaryObjectIdFilter = '';
  taskEventNameFilter = '';
  documentTypeNameFilter = '';

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  taskDescription: any;
  sidebarVisible: boolean = false;
  
  customTagFilter = '';
  tagValueFilter = '';
  verfiedFilter = -1;
  maxSequenceFilter: number;
  maxSequenceFilterEmpty: number;
  minSequenceFilter: number;
  minSequenceFilterEmpty: number;
  masterTagCategoryNameFilter = '';
  masterTagNameFilter = '';

taskTags:any;

  constructor(
      injector: Injector,
      private _taskTagsServiceProxy: TaskTagsServiceProxy,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _taskEventsServiceProxy: TaskEventsServiceProxy,
    private _documentTypes: DocumentTypesServiceProxy,
    private _taskDocumentsServiceProxy: TaskDocumentsServiceProxy,
    private _activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
    private _fileDownloadService: FileDownloadService,
    private sanitizer: DomSanitizer
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let taskEventId = this.route.snapshot.paramMap.get('taskEventId')
    this.taskEventId = parseInt(taskEventId);
    this.getTaskById();
    this.getDocTypes();
    this.getTaskDocuments();
    this.initFileUploader();
    this.getTaskTags();
  }
  ngAfterViewInit() {

  }
  getTaskById() {
    this._taskEventsServiceProxy.getTaskEventForView(this.taskEventId).subscribe((result) => {
      this.taskEvent = result
      const dirtyHtmlContent = this.taskEvent?.taskEvent?.description;
      this.safeHtmlContent = this.sanitizer.bypassSecurityTrustHtml(dirtyHtmlContent);
    })
  }
 
  getDocTypes() {
    this._documentTypes.getAll('', '', '', 0, 50).subscribe((result) => {
      this.docTypes = result.items
    })
  }
  fileChangeEvent(event: any) {
    if (event.target.files && event.target.files[0]) {
      this.fileName = event.target.files[0].name;
      this.taskDocument.documentTitle = this.fileName;
    }
  }
  getTaskDocuments(event?: LazyLoadEvent) {
    // if (this.primengTableHelper.shouldResetPaging(event)) {
    //   this.paginator.changePage(0);
    //   return;
    // }

    //this.primengTableHelper.showLoadingIndicator();



    this._taskDocumentsServiceProxy.getAllByTaskEventId(
      this.taskEventId,
      this.filterText,
      this.nameFilter,
      this.fileBinaryObjectIdFilter,
      this.taskEventNameFilter,
      this.documentTypeNameFilter,
      '',
      0,
      100
    ).subscribe(result => {
      //this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.taskDocuments = result.items;
      // this.totalCount.emit(this.primengTableHelper.totalRecordsCount);
      //this.primengTableHelper.hideLoadingIndicator();
    });
  }
  saveDocument(fileToken?: string): void {
    this.taskDocument.fileToken = fileToken;
    if (this.taskEventId) {
      this.taskDocument.taskEventId = this.taskEventId;
    }

    this._taskDocumentsServiceProxy.createOrEdit(this.taskDocument)
      .pipe(finalize(() => { }))
      .subscribe(() => {
        this.taskDocument = new CreateOrEditTaskDocumentDto();
        this.notify.info(this.l('SavedSuccessfully'));
        this.getTaskDocuments();
      });
  }
  addDocument() {
    if (this.uploader.queue != null && this.uploader.queue.length > 0) {
      this.uploader.uploadAll();
    } else {
      this.saveDocument();
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
  deleteTaskDocuments(id: number) {
    this._taskDocumentsServiceProxy.delete(id).subscribe(result => {
      this.getTaskDocuments();
      this.notify.info(this.l('DeletedSuccessfully'));
    });
  }
  createTaskEvent(): void {
    this.createOrEditTaskEventModal.show();
  }

  createTaskTag(): void {
    this.createOrEditTaskTagModal.show(null);
}
getTaskTags() {
  // if (this.primengTableHelper.shouldResetPaging(event)) {
  //     this.paginator.changePage(0);
  //     if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
  //         return;
  //     }
  // }

  // this.primengTableHelper.showLoadingIndicator();

  this._taskTagsServiceProxy
      .getAll(
          undefined,
          this.customTagFilter,
          this.tagValueFilter,
          this.verfiedFilter,
          this.maxSequenceFilter == null ? this.maxSequenceFilterEmpty : this.maxSequenceFilter,
          this.minSequenceFilter == null ? this.minSequenceFilterEmpty : this.minSequenceFilter,
          this.taskEventNameFilter,
          this.masterTagCategoryNameFilter,
          this.masterTagNameFilter,
          this.taskEventId,
          '',
          0,
          100
      )
      .subscribe((result) => {
          //this.primengTableHelper.totalRecordsCount = result.totalCount;
          this.taskTags = result.items;
          //this.primengTableHelper.hideLoadingIndicator();
      });
}

  openAiModalPr(fieldName: string): void {
    const taskName = this.taskEvent.taskEvent.name;
    this.taskDescription = `Write a ${fieldName} for a task where task title is ${taskName}`;
    var modalTitle = `AI Task Description Generator`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.taskDescription, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        this.taskEvent.taskEvent.description = result.data;
      }
    });
  }
}
