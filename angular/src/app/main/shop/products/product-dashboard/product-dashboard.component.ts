import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditProductDto, ProductsServiceProxy } from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-product-dashboard',
  templateUrl: './product-dashboard.component.html',
  styleUrls: ['./product-dashboard.component.css']
})
export class ProductDashboardComponent extends AppComponentBase {
 
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
  publish: Boolean = false;

  storeName: string;
  storeTags: any[] = [];
  numberOfRatings: number;
  ratingScore: number;

  allProductAdditionalCategory:any[]=[];
  additionalCategoryId: number;
  allAdditionalCategories: any[] = [];

  pickupOrDeliveryTags:any[]=[];
  membershipPrice:number;
  membershipName:string;

  isUrlAvailble: boolean = false;
  isUrlNotAvailble: boolean = false;

  isSkuAvailble: boolean = false;
  isSkuNotAvailble: boolean = false;

  templateOptions: any;

  productCategoryOptions: any;
  measurementUnitOptions: any;
  currencyOptions: any;

  ratingLikeOptions: any;
  publishOptions: SelectItem[];

  // teams: any[] = [];
  // numberOfTasks: number;
  // numberOfNotes: number;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private dialog: MatDialog,
    private _productsServiceProxy: ProductsServiceProxy,
    private titleService: Title
  ) {
    super(injector);
    this.productPublishedOptions = [{ label: 'Draft', value: false }, { label: 'Published', value: true }];
    this.productServiceOptions = [{ label: 'Not Service', value: false }, { label: 'Service Product', value: true }];
  }

  ngOnInit(): void {
    let productId = this.route.snapshot.paramMap.get('productId')
    this.productId = parseInt(productId);
    this.getProductDetails(this.productId);
  }
  ngAfterViewInit() {

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
      this.allProductAdditionalCategory = result.additionalCategories;
    });
    console.log(this.product)

    // this._productsServiceProxy.getProductTopStats(id).subscribe(result => {
    //   this.countView = result;
    // });

  }

  openAiModalPr(feildName: string): void {
    this.productShortDesc = "Write a  short description for a product where product name is Organic Mustard Oil and product brand is Saffola"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.productShortDesc, feildName: feildName },
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }
}
