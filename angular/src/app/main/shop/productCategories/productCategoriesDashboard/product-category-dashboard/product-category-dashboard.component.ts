import { AfterViewInit, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditMediaLibraryDto, CreateOrEditProductCategoryDto, MediaLibrariesServiceProxy, ProductCategoriesServiceProxy, ProductCategoryTeamEmployeeLookupTableDto, ProductCategoryTeamsServiceProxy, ProductProductCategoryLookupTableDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { FileItem, FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';
import { ProductCategoryMediaLibraryLookupTableModalComponent } from '../../productCategory-mediaLibrary-lookup-table-modal.component';
import { finalize } from 'rxjs';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MatDialog } from '@angular/material/dialog';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';

@Component({
  selector: 'app-product-category-dashboard',
  templateUrl: './product-category-dashboard.component.html',
  styleUrls: ['./product-category-dashboard.component.css'],
  encapsulation: ViewEncapsulation.Emulated,
  animations: [appModuleAnimation()]
})
export class ProductCategoryDashboardComponent extends AppComponentBase implements OnInit {

  productCategoryId: number;
  productCategory: CreateOrEditProductCategoryDto = new CreateOrEditProductCategoryDto();

  saving = false;
  mediaLibraryName = '';

  isUrlAvailble: boolean = false;
  isUrlNotAvailble: boolean = false;

  allParentCategories: any[] = [];
  employeeList: ProductCategoryTeamEmployeeLookupTableDto[] = [];
  selectedEmployees: ProductCategoryTeamEmployeeLookupTableDto[] = [];

  @ViewChild('productCategoryMediaLibraryLookupTableModal', { static: true }) productCategoryMediaLibraryLookupTableModal: ProductCategoryMediaLibraryLookupTableModalComponent;
  private _uploaderOptions: FileUploaderOptions = {};
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  imageSrc: any;
  mediaLibrary: CreateOrEditMediaLibraryDto = new CreateOrEditMediaLibraryDto();
  parentCategoryName: string;
  selectedParentCategory: ProductProductCategoryLookupTableDto = new ProductProductCategoryLookupTableDto();
  chatGPTPromt: string;
  constructor(
    injector: Injector,
    private _route: ActivatedRoute,
    private _productCategoryServiceProxy: ProductCategoriesServiceProxy,
    private _tokenService: TokenService,
    private _mediaLibrariesServiceProxy: MediaLibrariesServiceProxy,
    private dialog: MatDialog,
    private _productCategoryTeamsServiceProxy: ProductCategoryTeamsServiceProxy
  ) {
    super(injector);
    this._productCategoryServiceProxy.getAllProductCategoryForTableDropdown().subscribe(result => {
      this.allParentCategories = result;
    });
  }

  ngOnInit(): void {
    let productCategoryId = this._route.snapshot.paramMap.get('productCategoryId');
    this.productCategoryId = parseInt(productCategoryId);
    this.temporaryPictureUrl = '';
    this.initFileUploader();

    this.getProductCategoryDetails(this.productCategoryId);
    this._productCategoryTeamsServiceProxy.getAllEmployeeForLookupTable('', '', 0, 1000).subscribe(result => {
      this.employeeList = result.items;
    });
  }

  getProductCategoryDetails(id: number) {
    this._productCategoryServiceProxy.getProductCategoryForEdit(id).subscribe(result => {
      this.productCategory = result.productCategory;
      this.mediaLibraryName = result.mediaLibraryName;
      this.imageSrc = result.picture;
      this.parentCategoryName = result.parentCategoryName;
      this.selectedParentCategory.id = result.productCategory.parentCategoryId;
      this.selectedParentCategory.displayName = result.productCategoryTreeViewName;
      this.selectedParentCategory.parentcategoryId = result.productCategoryParentId;
    })
  }

  saveCategory(fileToken?: string): void {
    if (fileToken != null) {
      this.saving = true;
      this.mediaLibrary.fileToken = fileToken;
      this.mediaLibrary.masterTagCategoryId = 1;
      this.mediaLibrary.masterTagId = 1;
      if (this.selectedParentCategory) {
        this.productCategory.parentCategoryId = this.selectedParentCategory?.id;
      }
      this._mediaLibrariesServiceProxy.createOrEdit(this.mediaLibrary)
        .pipe(finalize(() => { this.saving = false; }))
        .subscribe((result) => {
          this.productCategory.mediaLibraryId = result;
          this._productCategoryServiceProxy.createOrEdit(this.productCategory)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
              this.selectedParentCategory = new ProductProductCategoryLookupTableDto();
              this.notify.info(this.l('SavedSuccessfully'));
              this.getProductCategoryDetails(this.productCategoryId);
            });
        });

    } else {
      if (this.selectedParentCategory) {
        this.productCategory.parentCategoryId = this.selectedParentCategory?.id;
      }
      this._productCategoryServiceProxy.createOrEdit(this.productCategory)
        .pipe(finalize(() => { this.saving = false; }))
        .subscribe(() => {
          this.selectedParentCategory = new ProductProductCategoryLookupTableDto();
          this.notify.info(this.l('SavedSuccessfully'));
          this.getProductCategoryDetails(this.productCategoryId);
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

  onSelectFromMediaLibrary() {
    this.productCategoryMediaLibraryLookupTableModal.id = this.productCategory.mediaLibraryId;
    this.productCategoryMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
    this.productCategoryMediaLibraryLookupTableModal.show();
  }

  getNewMediaLibraryId() {
    this.productCategory.mediaLibraryId = this.productCategoryMediaLibraryLookupTableModal.id;
    this.mediaLibraryName = this.productCategoryMediaLibraryLookupTableModal.displayName;
    //this.imageSrc = this.productCategoryMediaLibraryLookupTableModal.picture;

  }

  checkUrlAvailability(id: number, url: string) {
    this._productCategoryServiceProxy.checkUrlAvailability(id, url).subscribe(result => {
      if (result) {
        this.isUrlAvailble = true;
        this.isUrlNotAvailble = false;
      } else {
        this.isUrlNotAvailble = true;
        this.isUrlAvailble = false;
      }
    });
  }

  initFileUploader(): void {

    this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/MediaUpload/UploadPicture' });
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
  getFileExtension(filename) {
    return filename.split('.').pop();
  }
  onEmployeeSelect(event: any) {
    if (event) {
      let index = this.productCategory.teams ? this.productCategory.teams.findIndex(x => x.id == event.itemValue.id) : -1;
      if (index < 0) {
        this.productCategory.teams = event.value;
      } else if (index >= 0 && this.productCategory.id) {
        // this._taskTeamsServiceProxy.deleteByTask(this.taskEvent.id,event.itemValue.id).subscribe(result=>{
        //     this.taskEvent.teams.splice(index, 1);
        // });
      }
    }

    // console.log(event);
    // if (event.value.length > 0) {

    // }
  }

  openAiModal(fieldName: string): void {
    this.chatGPTPromt = `Generate Product Category Information where 
    Category Name: ${this.productCategory.name}
    Please just add the value for 
    Description, 
    URL, 
    MetaTitle, 
    MetaKeywords
    to json format based on key and pair.`;
    var modalTitle = `AI Text Generator - ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.chatGPTPromt, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        console.log(result.data);
        const responseText = this.extractCategoryeData(result.data);
        if (responseText) {
          this.productCategory.description = responseText.Description;
          this.productCategory.url = responseText.URL;
          this.productCategory.metaTitle = responseText.MetaTitle;
          this.productCategory.metaKeywords = responseText.MetaKeywords;
        }
      }
    });
  }

  private extractCategoryeData(responseText: string): { Description: string, URL: string, MetaTitle: string, MetaKeywords: string} {
    // Remove HTML tags and line breaks from the response text
    const cleanText = responseText.replace(/<br>/g, '').replace(/\n/g, '');
    console.log(cleanText);
    try {
      const response = JSON.parse(cleanText);

      if (response.Description && response.URL && response.MetaTitle && response.MetaKeywords) {
        const Description = response.Description;
        const URL = response.URL;
        const MetaTitle = response.MetaTitle;
        const MetaKeywords = response.MetaKeywords;
        return { Description, URL, MetaTitle, MetaKeywords };
      }
    } catch (error) {
      // JSON parsing failed, handle the error as needed
      throw new Error('Unable to parse the response as JSON');
    }
    throw new Error('Unable to extract data from the response');
  }

}
