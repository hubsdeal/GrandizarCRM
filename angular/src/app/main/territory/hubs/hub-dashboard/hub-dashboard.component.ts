import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-hub-dashboard',
  templateUrl: './hub-dashboard.component.html',
  styleUrls: ['./hub-dashboard.component.css']
})
export class HubDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit{
  hubId: number;
  hubShortDesc: string;
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
    let hubId = this.route.snapshot.paramMap.get('hubId')
    this.hubId = parseInt(hubId);
  }
  ngAfterViewInit() {

  }

  openAiModalPr(feildName:string): void {
    this.hubShortDesc = "Write a description for a HUB where hub name is New Delhi and hub type is City"
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.hubShortDesc , feildName: feildName},
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingData = result.data;
    });
  }
}
