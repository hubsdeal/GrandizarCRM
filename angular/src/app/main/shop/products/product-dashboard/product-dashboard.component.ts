import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-product-dashboard',
  templateUrl: './product-dashboard.component.html',
  styleUrls: ['./product-dashboard.component.css']
})
export class ProductDashboardComponent extends AppComponentBase {
  productId: number;
  productShortDesc: string;
  bindingData: any;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private dialog: MatDialog
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let productId = this.route.snapshot.paramMap.get('productId')
    this.productId = parseInt(productId);
  }
  ngAfterViewInit() {

  }

  openAiModalPr(feildName:string): void {
    this.productShortDesc = "Write a  short description for a product where product name is Organic Mustard Oil and product brand is Saffola"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.productShortDesc , feildName: feildName},
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }
}
