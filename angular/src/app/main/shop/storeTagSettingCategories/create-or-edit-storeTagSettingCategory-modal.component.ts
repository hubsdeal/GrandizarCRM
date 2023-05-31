import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreTagSettingCategoriesServiceProxy,
    CreateOrEditStoreTagSettingCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';

@Component({
    selector: 'createOrEditStoreTagSettingCategoryModal',
    templateUrl: './create-or-edit-storeTagSettingCategory-modal.component.html',
})
export class CreateOrEditStoreTagSettingCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTagSettingCategory: CreateOrEditStoreTagSettingCategoryDto = new CreateOrEditStoreTagSettingCategoryDto();
    imageSrc: any = 'assets/common/images/c_logo.png';
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    private _uploaderOptions: FileUploaderOptions = {};
    constructor(
        injector: Injector,
        private _storeTagSettingCategoriesServiceProxy: StoreTagSettingCategoriesServiceProxy,
        private _tokenService: TokenService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.initFileUploader();
    }

    show(storeTagSettingCategoryId?: number): void {
        if (!storeTagSettingCategoryId) {
            this.storeTagSettingCategory = new CreateOrEditStoreTagSettingCategoryDto();
            this.storeTagSettingCategory.id = storeTagSettingCategoryId;

            this.active = true;
            this.modal.show();
        } else {
            this._storeTagSettingCategoriesServiceProxy
                .getStoreTagSettingCategoryForEdit(storeTagSettingCategoryId)
                .subscribe((result) => {
                    this.storeTagSettingCategory = result.storeTagSettingCategory;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

   

    saveHub(fileToken?: string): void {
        this.saving = true;
        this.storeTagSettingCategory.fileToken = fileToken;

        this._storeTagSettingCategoriesServiceProxy
            .createOrEdit(this.storeTagSettingCategory)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveHub();
        }
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    
    fileChangeEvent(event: any) {

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
                this.saveHub(resp.result.fileToken);
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

}
