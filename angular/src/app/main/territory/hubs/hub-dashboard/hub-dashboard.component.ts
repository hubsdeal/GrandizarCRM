import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-hub-dashboard',
  templateUrl: './hub-dashboard.component.html',
  styleUrls: ['./hub-dashboard.component.css']
})
export class HubDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit{
  hubId: number;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let hubId = this.route.snapshot.paramMap.get('hubId')
    this.hubId = parseInt(hubId);
  }
  ngAfterViewInit() {

  }
}
