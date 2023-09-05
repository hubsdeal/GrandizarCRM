import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductCategoriesServiceProxy, CreateOrEditProductCategoryDto, MediaLibrariesServiceProxy, CreateOrEditMediaLibraryDto, TaskTeamEmployeeLookupTableDto, ProductCategoryTeamEmployeeLookupTableDto, ProductCategoryTeamsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCategoryMediaLibraryLookupTableModalComponent } from './productCategory-mediaLibrary-lookup-table-modal.component';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { SelectItem } from 'primeng/api';

@Component({
    selector: 'createOrEditProductCategoryModal',
    templateUrl: './create-or-edit-productCategory-modal.component.html',
})
export class CreateOrEditProductCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryMediaLibraryLookupTableModal', { static: true })
    productCategoryMediaLibraryLookupTableModal: ProductCategoryMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategory: CreateOrEditProductCategoryDto = new CreateOrEditProductCategoryDto();
    mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();

    mediaLibraryName = '';
    allParentCategories: any[];
    imageSrc: any = 'assets/common/images/c_logo.png';
    productServiceOptions: SelectItem[];
    PublishingOptions: SelectItem[];
    private _uploaderOptions: FileUploaderOptions = {};
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    employeeList: ProductCategoryTeamEmployeeLookupTableDto[] = [];
    selectedEmployees: ProductCategoryTeamEmployeeLookupTableDto[] = [];
    constructor(
        injector: Injector,
        private _productCategoriesServiceProxy: ProductCategoriesServiceProxy,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
        private _productCategoryTeamsServiceProxy:ProductCategoryTeamsServiceProxy
    ) {
        super(injector);
    }

    show(productCategoryId?: number): void {
        this._productCategoryTeamsServiceProxy.getAllEmployeeForLookupTable('','',0,1000).subscribe(result => {
            this.employeeList = result.items;
        });
        if (!productCategoryId) {
            this.productCategory = new CreateOrEditProductCategoryDto();
            this.productCategory.id = productCategoryId;
            this.mediaLibraryName = '';
            this.temporaryPictureUrl = '';
           

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoriesServiceProxy.getProductCategoryForEdit(productCategoryId).subscribe((result) => {
                this.productCategory = result.productCategory;

                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._productCategoriesServiceProxy.getAllProductCategoryForTableDropdown().subscribe(result => {
            debugger;
            this.allParentCategories = result;
            //this.allParentCategories = this.allParentCategories.sort((a, b) => a.localeCompare(b));
        });
        this.productCategory.productOrService=true;
        this.productCategory.published=false;
        this.productServiceOptions = [{ label: 'Product', value: true }, { label: 'Service', value: false }];
        this.PublishingOptions= [{ label: 'Published', value: true }, { label: 'Unpublished', value: false }];
        this.initFileUploader();
    }

    // save(): void {
    //     this.saving = true;

    //     this._productCategoriesServiceProxy
    //         .createOrEdit(this.productCategory)
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
    saveCategory(fileToken?: string): void {
        if (fileToken != null) {
            this.saving = true;
            this.mediaLibrary.fileToken = fileToken;
            this.mediaLibrary.masterTagCategoryId = 1;
            this.mediaLibrary.masterTagId = 1;
            this._mediaLibrariesServiceProxy.createOrEdit(this.mediaLibrary)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe((result) => {
                    this.productCategory.mediaLibraryId = result;
                    this._productCategoriesServiceProxy.createOrEdit(this.productCategory)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe(() => {
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        });
                });

        } else {
            this._productCategoriesServiceProxy.createOrEdit(this.productCategory)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                });
        }

    }
    save() {
        if (this.uploader.queue != null && this.uploader.queue.length > 0) {
            this.uploader.uploadAll();
        } else {
            this.saveCategory();
        }
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
                this.saveCategory(resp.result.fileToken);
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

    openSelectMediaLibraryModal() {
        this.productCategoryMediaLibraryLookupTableModal.id = this.productCategory.mediaLibraryId;
        this.productCategoryMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productCategoryMediaLibraryLookupTableModal.show();
    }

    setMediaLibraryIdNull() {
        this.productCategory.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewMediaLibraryId() {
        this.productCategory.mediaLibraryId = this.productCategoryMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productCategoryMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
    onEmployeeSelect(event: any) {
        if (event) {
            let index =this.productCategory.teams?this.productCategory.teams.findIndex(x => x.id == event.itemValue.id):-1;
            if(index<0){
                this.productCategory.teams = event.value;
            }else if(index>=0 && this.productCategory.id){
                // this._taskTeamsServiceProxy.deleteByTask(this.taskEvent.id,event.itemValue.id).subscribe(result=>{
                //     this.taskEvent.teams.splice(index, 1);
                // });
            }
        }

        // console.log(event);
        // if (event.value.length > 0) {

        // }
    }
}
