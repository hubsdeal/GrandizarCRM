import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductMediasServiceProxy, CreateOrEditProductMediaDto, CreateOrEditMediaLibraryDto, MediaLibraryMasterTagCategoryLookupTableDto, MediaLibraryMasterTagLookupTableDto, MediaLibrariesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductMediaProductLookupTableModalComponent } from './productMedia-product-lookup-table-modal.component';
import { ProductMediaMediaLibraryLookupTableModalComponent } from './productMedia-mediaLibrary-lookup-table-modal.component';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';

@Component({
    selector: 'createOrEditProductMediaModal',
    templateUrl: './create-or-edit-productMedia-modal.component.html',
})
export class CreateOrEditProductMediaModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productMediaProductLookupTableModal', { static: true })
    productMediaProductLookupTableModal: ProductMediaProductLookupTableModalComponent;
    @ViewChild('productMediaMediaLibraryLookupTableModal', { static: true })
    productMediaMediaLibraryLookupTableModal: ProductMediaMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() saveProductMedia: EventEmitter<number> = new EventEmitter<number>();

    active = false;
    saving = false;

    productMedia: CreateOrEditProductMediaDto = new CreateOrEditProductMediaDto();

    productName = '';
    mediaLibraryName = '';

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

    productId: number;
    constructor(
        injector: Injector,
        private _productMediasServiceProxy: ProductMediasServiceProxy,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _tokenService: TokenService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.temporaryPictureUrl = '';
        this.initFileUploader();
    }

    // show(productMediaId?: number): void {
    //     if (!productMediaId) {
    //         this.productMedia = new CreateOrEditProductMediaDto();
    //         this.productMedia.id = productMediaId;
    //         this.productName = '';
    //         this.mediaLibraryName = '';

    //         this.active = true;
    //         this.modal.show();
    //     } else {
    //         this._productMediasServiceProxy.getProductMediaForEdit(productMediaId).subscribe((result) => {
    //             this.productMedia = result.productMedia;

    //             this.productName = result.productName;
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
            this.imageSrc = 'assets/common/images/c_logo.png';
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

    saveMedia(fileToken?: string): void {
        this.mediaLibrary.fileToken = fileToken;
        if (this.selectUploadPhoto) {
            this.mediaLibrary.masterTagId = 1;
        }

        if (this.selectAddVideo) {
            this.mediaLibrary.masterTagId = 2;
        }
        this.mediaLibrary.name = 'Grandizar-' + this.mediaLibrary.name;

        if (this.mediaId != null) {
            this.saving = false;
            this.close();
            this.saveProductMedia.emit(this.mediaId);
            this.modalSave.emit(null);
        } else {
            this._mediaLibrariesServiceProxy.createOrEdit(this.mediaLibrary)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe((result) => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.saveProductMedia.emit(result);
                    this.modalSave.emit(null);
                });
        }

    }


    // save(): void {
    //     this.saving = true;

    //     this._productMediasServiceProxy
    //         .createOrEdit(this.productMedia)
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

    close(): void {
        this.active = false;
        this.modal.hide();
        this.imageSrc = null;
        this.mediaLibrary = new CreateOrEditMediaLibraryDto();
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



    openSelectProductModal() {
        this.productMediaProductLookupTableModal.id = this.productMedia.productId;
        this.productMediaProductLookupTableModal.displayName = this.productName;
        this.productMediaProductLookupTableModal.show();
    }
    openSelectMediaLibraryModal() {
        this.productMediaMediaLibraryLookupTableModal.id = this.productMedia.mediaLibraryId;
        this.productMediaMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productMediaMediaLibraryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productMedia.productId = null;
        this.productName = '';
    }
    setMediaLibraryIdNull() {
        this.productMedia.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewProductId() {
        this.productMedia.productId = this.productMediaProductLookupTableModal.id;
        this.productName = this.productMediaProductLookupTableModal.displayName;
    }
    getNewMediaLibraryId() {
        this.productMedia.mediaLibraryId = this.productMediaMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productMediaMediaLibraryLookupTableModal.displayName;
    }



}
