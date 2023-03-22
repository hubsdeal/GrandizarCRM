import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
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
}
