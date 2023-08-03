import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MediaLibrariesServiceProxy, CreateOrEditMediaLibraryDto, MediaLibraryMasterTagLookupTableDto, ProductMediasServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { MediaLibraryMasterTagCategoryLookupTableModalComponent } from './mediaLibrary-masterTagCategory-lookup-table-modal.component';
import { MediaLibraryMasterTagLookupTableModalComponent } from './mediaLibrary-masterTag-lookup-table-modal.component';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { ProductMediaMediaLibraryLookupTableModalComponent } from '@app/main/shop/productMedias/productMedia-mediaLibrary-lookup-table-modal.component';
import { AppSessionService } from '@shared/common/session/app-session.service';

@Component({
    selector: 'createOrEditMediaLibraryModal',
    templateUrl: './create-or-edit-mediaLibrary-modal.component.html',
})
export class CreateOrEditMediaLibraryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('mediaLibraryMasterTagCategoryLookupTableModal', { static: true })
    mediaLibraryMasterTagCategoryLookupTableModal: MediaLibraryMasterTagCategoryLookupTableModalComponent;
    @ViewChild('mediaLibraryMasterTagLookupTableModal', { static: true })
    mediaLibraryMasterTagLookupTableModal: MediaLibraryMasterTagLookupTableModalComponent;
    @ViewChild('productMediaMediaLibraryLookupTableModal', { static: true }) productMediaMediaLibraryLookupTableModal: ProductMediaMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() saveProductMedia: EventEmitter<number> = new EventEmitter<number>();
    active = false;
    saving = false;

    mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();

    masterTagCategoryName = '';
    masterTagName = '';

    allMasterTags: MediaLibraryMasterTagLookupTableDto[];
    private _uploaderOptions: FileUploaderOptions = {};
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    mediaName: string;
    mediaPicture: string;
    mediaId: number;

    imageSrc: any = '';
    imageSize: any;
    imageExtension: any;
    isChangeProductPicture: boolean = false;

    isChangeProductVideo: boolean = false;
    productId: number;
    isFromMediaLibraryList: boolean = false;

    //allSelectedMedias: MediaLibraryFromSpDto[] = [];

    constructor(
        injector: Injector,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _appSessionService: AppSessionService,
        private _productMediasServiceProxy: ProductMediasServiceProxy,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService
    ) {
        super(injector);
    }

    show(mediaLibraryId?: number): void {
        if (!mediaLibraryId) {
            this.mediaLibrary = new CreateOrEditMediaLibraryDto();
            this.mediaLibrary.id = mediaLibraryId;
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._mediaLibrariesServiceProxy.getMediaLibraryForEdit(mediaLibraryId).subscribe((result) => {
                this.mediaLibrary = result.mediaLibrary;

                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
        this._mediaLibrariesServiceProxy.getAllMasterTagForTableDropdown().subscribe(result => {
            this.allMasterTags = result;
        });
    }

    // save(): void {
    //     this.saving = true;

    //     this._mediaLibrariesServiceProxy
    //         .createOrEdit(this.mediaLibrary)
    //         .pipe(
    //             finalize(() => {
    //                 this.saving = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             this.notify.info(this.l('SavedSuccessfully'));
    //             this.close();
    //             this.modalSave.emit(null);
    //         });
    // }
    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveMedia();
        }
    }
    saveMedia(fileToken?: string): void {
        this.saving = true;
        this.mediaLibrary.fileToken = fileToken;
        this.mediaLibrary.name = 'hubsdeal-' + this.mediaLibrary.name;

        this._mediaLibrariesServiceProxy.createOrEdit(this.mediaLibrary)
            .pipe(
                finalize(() => { 
                    this.saving = false; 
                }))
            .subscribe((result) => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    fileChangeEvent(event: any) {
        this.mediaLibrary.name = event.target.files[0].name
        this.mediaLibrary.size = event.target.files[0].size / 1024 + " kb";
        this.mediaLibrary.fileExtension = event.target.files[0].type;
        if (event.target.files && event.target.files[0]) {
            var reader = new FileReader();

            reader.readAsDataURL(event.target.files[0]); // read file as data url

            reader.onload = (event) => { // called once readAsDataURL is completed

                this.imageSrc = event.target.result;
            }
        }
    }


    initFileUploader(): void {

        this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/api/MediaUpload/UploadPicture' });
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
                this.saveMedia(resp.result.fileToken);
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

    openSelectMasterTagCategoryModal() {
        this.mediaLibraryMasterTagCategoryLookupTableModal.id = this.mediaLibrary.masterTagCategoryId;
        this.mediaLibraryMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.mediaLibraryMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.mediaLibraryMasterTagLookupTableModal.id = this.mediaLibrary.masterTagId;
        this.mediaLibraryMasterTagLookupTableModal.displayName = this.masterTagName;
        this.mediaLibraryMasterTagLookupTableModal.show();
    }

    setMasterTagCategoryIdNull() {
        this.mediaLibrary.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.mediaLibrary.masterTagId = null;
        this.masterTagName = '';
    }

    getNewMasterTagCategoryId() {
        this.mediaLibrary.masterTagCategoryId = this.mediaLibraryMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.mediaLibraryMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.mediaLibrary.masterTagId = this.mediaLibraryMasterTagLookupTableModal.id;
        this.masterTagName = this.mediaLibraryMasterTagLookupTableModal.displayName;
    }

    openSelectMediaLibraryModal() {
        this.productMediaMediaLibraryLookupTableModal.show();
    }
    openSelectMyMediaLibraryModal() {
        console.log(this._appSessionService.userId);
        //this.productMediaMediaLibraryLookupTableModal.employeeUserId = this._appSessionService.userId;
        this.productMediaMediaLibraryLookupTableModal.show();
    }

    setMediaLibraryIdNull() {
        this.mediaName = '';
        this.mediaPicture = null;
        this.mediaId = null;

    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this.initFileUploader();
    }

    getNewMediaLibraryId(event: any) {
        if (event) {
            //this.allSelectedMedias = event;
        }
    }
}
