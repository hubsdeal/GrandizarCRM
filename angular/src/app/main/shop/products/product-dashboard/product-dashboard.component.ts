import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-product-dashboard',
  templateUrl: './product-dashboard.component.html',
  styleUrls: ['./product-dashboard.component.css']
})
export class ProductDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  productId: number;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let productId = this.route.snapshot.paramMap.get('productId')
    this.productId = parseInt(productId);
  }
  ngAfterViewInit() {

  }
}
