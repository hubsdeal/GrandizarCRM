import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreMediasServiceProxy, CreateOrEditStoreMediaDto, CreateOrEditMediaLibraryDto, MediaLibraryMasterTagCategoryLookupTableDto, MediaLibraryMasterTagLookupTableDto, MediaLibrariesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreMediaStoreLookupTableModalComponent } from './storeMedia-store-lookup-table-modal.component';
import { StoreMediaMediaLibraryLookupTableModalComponent } from './storeMedia-mediaLibrary-lookup-table-modal.component';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';

@Component({
    selector: 'createOrEditStoreMediaModal',
    templateUrl: './create-or-edit-storeMedia-modal.component.html',
})
export class CreateOrEditStoreMediaModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeMediaStoreLookupTableModal', { static: true })
    storeMediaStoreLookupTableModal: StoreMediaStoreLookupTableModalComponent;
    @ViewChild('storeMediaMediaLibraryLookupTableModal', { static: true })
    storeMediaMediaLibraryLookupTableModal: StoreMediaMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() saveStoreMedia: EventEmitter<number> = new EventEmitter<number>();

    active = false;
    saving = false;

    storeMedia: CreateOrEditStoreMediaDto = new CreateOrEditStoreMediaDto();

    storeName = '';
    mediaLibraryName = '';

    storeId: number;

    selectUploadPhoto: boolean = false;
    selectAddVideo: boolean = false;

    mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();
    masterTagCategoryName = '';
    masterTagName = '';
    allMasterTagCategorys: MediaLibraryMasterTagCategoryLookupTableDto[];
    allMasterTags: MediaLibraryMasterTagLookupTableDto[];
    private _uploaderOptions: FileUploaderOptions = {};
    public uploader: FileUploader;
    public temporaryPictureUrl: string;

    mediaName: string;
    mediaPicture: string;
    mediaId: number;
    imageSize: any;
    imageExtension: any;
    imageSrc: any = 'assets/common/images/c_logo.png';

    constructor(
        injector: Injector,
        private _storeMediasServiceProxy: StoreMediasServiceProxy,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _tokenService: TokenService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(){
        this.temporaryPictureUrl = '';
        this.initFileUploader();
    }

    // show(storeMediaId?: number): void {
    //     if (!storeMediaId) {
    //         this.storeMedia = new CreateOrEditStoreMediaDto();
    //         this.storeMedia.id = storeMediaId;
    //         this.storeName = '';
    //         this.mediaLibraryName = '';

    //         this.active = true;
    //         this.modal.show();
    //     } else {
    //         this._storeMediasServiceProxy.getStoreMediaForEdit(storeMediaId).subscribe((result) => {
    //             this.storeMedia = result.storeMedia;

    //             this.storeName = result.storeName;
    //             this.mediaLibraryName = result.mediaLibraryName;

    //             this.active = true;
    //             this.modal.show();
    //         });
    //     }
    // }

    show(mediaLibraryId?: number): void {
        this.mediaName = '';
        this.mediaPicture = null;
        this.mediaId = null;

        if (!mediaLibraryId) {
            this.mediaLibrary = new CreateOrEditMediaLibraryDto();
            this.mediaLibrary.id = mediaLibraryId;
            this.mediaLibrary.masterTagId = 1;
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._mediaLibrariesServiceProxy.getMediaLibraryForEdit(mediaLibraryId).subscribe(result => {
                this.mediaLibrary = result.mediaLibrary;

                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
        this._mediaLibrariesServiceProxy.getAllMasterTagCategoryForLookupTable('', '', 0, 1000000).subscribe(result => {
            this.allMasterTagCategorys = result.items;
        });
        this._mediaLibrariesServiceProxy.getAllMasterTagForTableDropdown().subscribe(result => {
            this.allMasterTags = result;
        });

    }


    // save(): void {
    //     this.saving = true;

    //     this._storeMediasServiceProxy
    //         .createOrEdit(this.storeMedia)
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
    saveMedia(fileToken?: string): void {
        this.mediaLibrary.fileToken = fileToken;
        if (this.selectUploadPhoto) {
            this.mediaLibrary.masterTagId = 1;
        }

        if (this.selectAddVideo) {
            this.mediaLibrary.masterTagId = 2;
        }
        this.mediaLibrary.name = 'hubsdeal-' + this.mediaLibrary.name;

        if (this.mediaId != null) {
            this.saving = false;
            this.close();
            this.saveStoreMedia.emit(this.mediaId);
            this.modalSave.emit(null);
        } else {
            this._mediaLibrariesServiceProxy.createOrEdit(this.mediaLibrary)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe((result) => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.saveStoreMedia.emit(result);
                    this.modalSave.emit(null);
                });
        }

    }

    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveMedia();
        }
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.selectUploadPhoto = false;
        this.selectAddVideo = false;
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

    openSelectStoreModal() {
        this.storeMediaStoreLookupTableModal.id = this.storeMedia.storeId;
        this.storeMediaStoreLookupTableModal.displayName = this.storeName;
        this.storeMediaStoreLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.storeMediaMediaLibraryLookupTableModal.id = this.storeMedia.mediaLibraryId;
        this.storeMediaMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.storeMediaMediaLibraryLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeMedia.storeId = null;
        this.storeName = '';
    }
    setMediaLibraryIdNull() {
        this.storeMedia.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewStoreId() {
        this.storeMedia.storeId = this.storeMediaStoreLookupTableModal.id;
        this.storeName = this.storeMediaStoreLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.storeMedia.mediaLibraryId = this.storeMediaMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.storeMediaMediaLibraryLookupTableModal.displayName;
    }

}
