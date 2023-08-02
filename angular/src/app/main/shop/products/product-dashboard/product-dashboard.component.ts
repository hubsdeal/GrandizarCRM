import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditProductDto, CreateOrEditProductMediaDto, GetProductAccountTeamForViewDto, GetProductMediaForViewDto, GetStoreMediaForViewDto, ProductAccountTeamsServiceProxy, ProductDashboardStatisticsForViewDto, ProductMediasServiceProxy, ProductsServiceProxy, ProductStoreLookupTableDto, ProductTeamsServiceProxy } from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { SelectItem } from 'primeng/api';
import { finalize } from 'rxjs';
import { CreateOrEditProductMediaModalComponent } from '../../productMedias/create-or-edit-productMedia-modal.component';
import { CreateOrEditProductAccountTeamModalComponent } from '../../productAccountTeams/create-or-edit-productAccountTeam-modal.component';
import { CreateOrEditProductNoteModalComponent } from '../../productNotes/create-or-edit-productNote-modal.component';
import { CreateOrEditProductTaskMapModalComponent } from '../../productTaskMaps/create-or-edit-productTaskMap-modal.component';

@Component({
  selector: 'app-product-dashboard',
  templateUrl: './product-dashboard.component.html',
  styleUrls: ['./product-dashboard.component.css']
})
export class ProductDashboardComponent extends AppComponentBase {

  @ViewChild('createOrEditProductAccountTeamModal', { static: true })
  createOrEditProductAccountTeamModal: CreateOrEditProductAccountTeamModalComponent;
  @ViewChild('createOrEditProductNoteModal', { static: true })
  createOrEditProductNoteModal: CreateOrEditProductNoteModalComponent;
  @ViewChild('createOrEditProductTaskMapModal', { static: true })
  createOrEditProductTaskMapModal: CreateOrEditProductTaskMapModalComponent;
  saving = false;
  productShortDesc: string;
  bindingData: any;
  productPublishedOptions: SelectItem[];
  productServiceOptions: SelectItem[];
  selectBTN: boolean = false;

  product: CreateOrEditProductDto = new CreateOrEditProductDto();
  productId: number;
  mediaLibraryName = '';
  categoryName: string;
  picture: string;
  video: string;
  images: GetProductMediaForViewDto[] = [];
  videos: any[] = [];


  imageSrc: any = 'assets/common/images/c_logo.png';
  public uploader: FileUploader;
  public temporaryPictureUrl: string;
  private _uploaderOptions: FileUploaderOptions = {};
  publish: Boolean = false;

  storeName: string;
  storeTags: any[] = [];
  numberOfRatings: number;
  ratingScore: number;

  allProductAdditionalCategory: any[] = [];
  additionalCategoryId: number;
  allAdditionalCategories: any[] = [];

  pickupOrDeliveryTags: any[] = [];
  membershipPrice: number;
  membershipName: string;

  isUrlAvailble: boolean = false;
  isUrlNotAvailble: boolean = false;

  isSkuAvailble: boolean = false;
  isSkuNotAvailble: boolean = false;

  templateOptions: any;

  measurementUnitOptions: any;
  currencyOptions: any;

  ratingLikeOptions: any;
  publishOptions: SelectItem[];

  teams: GetProductAccountTeamForViewDto[] = [];
  // numberOfTasks: number;
  // numberOfNotes: number;
  productCategoryId: number;

  productCategoryOptions: any = []
  selectedProductCategory: any;


  storeOptions: any=[];
  selectedStore: any;


  measurementUnitName: string;
  isManufacturerSkuAvailble: boolean = false;
  isManufacturerSkuNotAvailble: boolean = false;
  temporaryMediaLibraryId: number;
  publicViewUrl:string;

  statistics: ProductDashboardStatisticsForViewDto = new ProductDashboardStatisticsForViewDto();

  @ViewChild('createOrEditProductMediaModal', { static: true }) createOrEditProductMediaModal: CreateOrEditProductMediaModalComponent;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private dialog: MatDialog,
    private _productsServiceProxy: ProductsServiceProxy,
    private _productMediasServiceProxy: ProductMediasServiceProxy,
    private _sanitizer: DomSanitizer,
    private titleService: Title,
    private _productAccountTeamsServiceProxy: ProductAccountTeamsServiceProxy
  ) {
    super(injector);
    this.productPublishedOptions = [{ label: 'Draft', value: false }, { label: 'Published', value: true }];
    this.productServiceOptions = [{ label: 'Product', value: true }, { label: 'Service', value: false }];
  }

  ngOnInit(): void {
    let productId = this.route.snapshot.paramMap.get('productId')
    this.productId = parseInt(productId);
    this.loadAllDropdown();
    this.getProductDetails(this.productId);
    this.getProductTeams(this.productId);
    this.getStatisticsData();
  }
  ngAfterViewInit() {

  }

  getProductTeams(productId: number) {
    this._productAccountTeamsServiceProxy.getAllByProductId(productId).subscribe(result => {
      this.teams = result.items;
    });
  }

  getProductDetails(id: number) {
    this._productsServiceProxy.getProductForEdit(id).subscribe(result => {
      this.product = result.product;
      this.titleService.setTitle(this.product.name);
      this.mediaLibraryName = result.mediaLibraryName;
      this.categoryName = result.productCategoryName;
      this.picture = result.picture;
      this.publish = result.product.isPublished ? true : false;
      this.storeName = result.storeName;
      this.storeTags = result.storeTags;
      this.numberOfRatings = result.numberOfRatings;
      this.ratingScore = result.ratingScore;
      this.allAdditionalCategories = result.additionalCategories;
      // this.numberOfTasks = result.numberOfTasks;
      // this.numberOfNotes = result.numberOfNotes;
      // this.teams = result.teams;
      this.pickupOrDeliveryTags = result.pickupOrDeliveryTags;
      this.membershipPrice = result.membershipPrice;
      this.membershipName = result.membershipName;
      this.measurementUnitName = result.measurementUnitName;
      this.allProductAdditionalCategory = result.additionalCategories;
      this.publicViewUrl=result.publicViewUrl;
      //this.selectedProductCategory = result.product.productCategoryId;
      if (!result.product.isService) {
        this.product.isService = true;
      }
      console.log(this.product)
      this.getProductPhotos();
    });


    // this._productsServiceProxy.getProductTopStats(id).subscribe(result => {
    //   this.countView = result;
    // });

  }

  loadAllDropdown() {
    this._productsServiceProxy.getAllProductCategoryForLookupTable('', '', 0, 10000).subscribe(result => {
      this.productCategoryOptions = result.items;
    });
    this._productsServiceProxy.getAllStoreForLookupTable('', '', 0, 10000).subscribe(result => {
      this.storeOptions = result.items;
      console.log(this.storeOptions)
    });
    this._productsServiceProxy.getAllMeasurementUnitForLookupTable('', '', 0, 10000).subscribe(result => {
      this.measurementUnitOptions = result.items;
    });
    this._productsServiceProxy.getAllCurrencyForLookupTable('', '', 0, 10000).subscribe(result => {
      this.currencyOptions = result.items;
    });
    this._productsServiceProxy.getAllRatingLikeForLookupTable('', '', 0, 10000).subscribe(result => {
      this.ratingLikeOptions = result.items;
    });
  }

  // onProductCategoryClick(event: any) {
  //   console.log(event);
  //   if (event.value != null) {
  //     this.product.productCategoryId = event.value.id;
  //   }
  // }

  // onStoreClick(event: any) {
  //   console.log(event);
  //   if (event.value != null) {
  //     this.product.storeId = event.value.id;
  //   }
  // }


  openAiModalPr(fieldName: string): void {
    const productName = this.product.name;
    this.productShortDesc = `Write a ${fieldName} for a product where product name is ${productName} and product category is ${this.categoryName}`;
    var modalTitle = `AI Text Generator - Product ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.productShortDesc, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        if (fieldName == 'Short Description') {
          this.product.shortDescription = result.data;
        } else {
          this.product.description = result.data;
        }
      }
    });
  }

  openAiModal(fieldName: string): void {
    const productName = this.product.name;
    const promt = `Generate product data as json format with key pair and the key is Product_URL, SKU, Manufacturer_SKU, SEO_Title, Meta_Keywords, 
                  Meta_Description, Product_Description, Product_Short_Description where the Product Name is ${productName} and the product Category is ${this.categoryName}`;
    var modalTitle = `AI Text Generator - ${fieldName}`;
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: promt, feildName: fieldName, modalTitle: modalTitle },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.data != null) {
        this.insertProductDetails(result.data);
      }
    });
  }


  insertProductDetails(response: string): void {
    const cleanedResponse = response.replace(/<br>/g, '');
    const productDetails = JSON.parse(cleanedResponse);
    this.product.url = productDetails.Product_URL;
    this.product.sku = productDetails.SKU;
    this.product.productManufacturerSku = productDetails.Manufacturer_SKU;
    this.product.seoTitle = productDetails.SEO_Title;
    this.product.metaKeywords = productDetails.Meta_Keywords;
    this.product.metaDescription = productDetails.Meta_Description;
    this.product.description = productDetails.Product_Description;
    this.product.shortDescription = productDetails.Product_Short_Description;
  }


  save(): void {
    this.saving = true;
    if (this.product.salePrice != null && this.product.regularPrice <= this.product.salePrice) {
      this.notify.error(this.l('Regular price can not be equal or less than Sales Price'));
    } else {
      if (this.product.salePrice != null) {
        this.product.priceDiscountPercentage = ((this.product.regularPrice - this.product.salePrice) / this.product.regularPrice) * 100;
        this.product.priceDiscountAmount = this.product.regularPrice - this.product.salePrice;
      }
      this._productsServiceProxy.createOrEdit(this.product)
        .pipe(finalize(() => { this.saving = false; }))
        .subscribe(result => {
          this.getProductDetails(this.productId);
          this.notify.info(this.l('SavedSuccessfully'));
        });
    }
  }

  checkUrlAvailability(id: number, url: string) {
    this._productsServiceProxy.checkUrlAvailability(id, url).subscribe(result => {
      if (result) {
        this.isUrlAvailble = true;
        this.isUrlNotAvailble = false;
      } else {
        this.isUrlNotAvailble = true;
        this.isUrlAvailble = false;
      }
    });
  }

  checkSkuAvailability(id: number, url: string) {
    this._productsServiceProxy.checkSkuAvailability(id, url).subscribe(result => {
      if (result) {
        this.isSkuAvailble = true;
        this.isSkuNotAvailble = false;
      } else {
        this.isSkuNotAvailble = true;
        this.isSkuAvailble = false;
      }
    });
  }

  checkManufacturerSkuAvailability(id: number, url: string) {
    this._productsServiceProxy.checkProductManufacturerSkuAvailability(id, url).subscribe(result => {
      if (result) {
        this.isManufacturerSkuAvailble = true;
        this.isManufacturerSkuNotAvailble = false;
      } else {
        this.isManufacturerSkuNotAvailble = true;
        this.isManufacturerSkuAvailble = false;
      }
    });
  }


  createProductMediaPhoto(): void {
    this.createOrEditProductMediaModal.selectUploadPhoto = true;
    this.createOrEditProductMediaModal.productId = this.productId;
    this.createOrEditProductMediaModal.show();
  }

  createProductMediaVideo(): void {
    this.createOrEditProductMediaModal.selectAddVideo = true;
    this.createOrEditProductMediaModal.productId = this.productId;
    this.createOrEditProductMediaModal.show();
  }

  getSafeEmbeddedVideoUrl(url: string) {
    return this._sanitizer.bypassSecurityTrustResourceUrl(url);
  }
  getProductPhotos() {

    this.images = [];
    this.videos = [];
    this._productMediasServiceProxy.getAllByProductIdForDashboard(
      this.productId
    ).subscribe(result => {
      this.images.push(...result.items);
      this.videos.push(...result.items.filter(x => x.videoUrl != null));
    });
  }

  onPhotoOrVideoClick(data: any) {
    if (data.picture) {
      this.picture = data.picture;
      this.temporaryMediaLibraryId = data.productMedia.mediaLibraryId;
    } else if (data.videoUrl) {
      this.video = data.videoUrl;
      this.temporaryMediaLibraryId = data.productMedia.mediaLibraryId;
    }
  }
  deleteMedia(id: number) {
    this._productMediasServiceProxy.delete(id).subscribe(result => {
      this.notify.success(this.l('DeletedSuccessfully'));
      this.getProductDetails(this.productId);
    })
  }

  uploadProductMedia(event: any) {
    if (event) {
      var media = new CreateOrEditProductMediaDto();
      media.productId = this.productId;
      media.mediaLibraryId = event;
      this._productMediasServiceProxy.createOrEdit(media).subscribe(result => {
        this.notify.info(this.l('SavedSuccessfully'));
        if (this.product.mediaLibraryId == null) {
          this.product.mediaLibraryId = event;
          this._productsServiceProxy.createOrEdit(this.product).subscribe(r => {
            this.getProductDetails(this.productId);
            this.temporaryMediaLibraryId = event;
          });
        }
        this.getProductDetails(this.productId);
      });
    }
  }

  onPrimaryProductPhotoOrVideoClick() {
    this.product.mediaLibraryId = this.temporaryMediaLibraryId;
    this._productsServiceProxy.createOrEdit(this.product).subscribe(result => {
      this.notify.info('Updated Successfully');
    })
  }

  createProductAccountTeam(): void {
    this.createOrEditProductAccountTeamModal.productId = this.productId;
    this.createOrEditProductAccountTeamModal.show();
  }

  createProductNote(): void {
    this.createOrEditProductNoteModal.productId = this.productId;
    this.createOrEditProductNoteModal.show();
  }

  createProductTaskMap(): void {
    this.createOrEditProductTaskMapModal.productId = this.productId;
    this.createOrEditProductTaskMapModal.show();
  }

  getStatisticsData() {
    this._productsServiceProxy.getProductDashboardStatistics(this.productId).subscribe(result => {
      this.statistics = result;
    });
  }
}
