import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  storeId: number;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let storeId = this.route.snapshot.paramMap.get('storeId')
    this.storeId = parseInt(storeId);
  }
  ngAfterViewInit() {

  }
}
