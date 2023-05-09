import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditStoreDto } from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  storeId: number;
  productShortDesc: string;
  modalTile:string
  bindingData: any;
  localOrVirtualStoreOptions: SelectItem[];
  store: CreateOrEditStoreDto = new CreateOrEditStoreDto();

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private dialog: MatDialog
  ) {
    super(injector);
    this.store.isLocalOrOnlineStore=true;
  }

  ngOnInit(): void {
    let storeId = this.route.snapshot.paramMap.get('storeId')
    this.storeId = parseInt(storeId);
    this.localOrVirtualStoreOptions = [{ label: 'Local Store', value: false }, { label: 'Virtual Store', value: true }];
  }
  ngAfterViewInit() {

  }

  openAiModal(feildName:string): void {
    this.productShortDesc = "Write Store About where store name is Saffola"
    var modalTitle = "AI Text Generator - About Store"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.productShortDesc , feildName: feildName, modalTitle: modalTitle},
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }
}
