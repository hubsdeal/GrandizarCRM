import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatGptResponseModalComponent } from '@app/shared/chat-gpt-response-modal/chat-gpt-response-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-contact-dashboard',
  templateUrl: './contact-dashboard.component.html',
  styleUrls: ['./contact-dashboard.component.css']
})
export class ContactDashboardComponent extends AppComponentBase {
  contactId: number;
  @ViewChild('profile_editor_tab') profileEditorTab: ElementRef<HTMLAnchorElement>;

  companyDescriptionGenerateCommand: string;
  bindingDataprofile: any;
  showCalendarView: boolean;
  showListView: boolean;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private dialog: MatDialog
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let contactId = this.route.snapshot.paramMap.get('contactId')
    this.contactId = parseInt(contactId);
  }
  ngAfterViewInit() {

  }

  activateTab(profile_editor_tab: ElementRef<HTMLAnchorElement>) {
    profile_editor_tab.nativeElement.click();
  }

  openAiModal(): void {
    // this.companyDescriptionGenerateCommand = "Write a Company Description where Company Name: " + 'jobaar'
    //   + " Company Location: " + 'USA' + `,\n`
    //   + " Company tradeName :" + 'jobaar' + `,\n`
    //   + " Address : " + 'USA' + `,\n`
    this.companyDescriptionGenerateCommand = "Write a short profile for Rashedur Rahman where job title is angular developer and company is jobaar"
    const feildName = 'Short Profile';
    const dialogRef = this.dialog.open(ChatGptResponseModalComponent, {
      data: { promtFromAnotherComponent: this.companyDescriptionGenerateCommand , feildName: feildName},
      width: '1100px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result)
      this.bindingDataprofile = result.data;
    });
  }

  onCalenderView() {
    this.showCalendarView = !this.showCalendarView;
    this.showListView = !this.showListView;
  }

  onListView() {
    this.showListView = !this.showListView;
    this.showCalendarView = !this.showCalendarView;
  }
}
