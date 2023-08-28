import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubNavigationMenusServiceProxy,
    CreateOrEditHubNavigationMenuDto,
    HubNavigationMenuMasterNavigationMenuLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubNavigationMenuHubLookupTableModalComponent } from './hubNavigationMenu-hub-lookup-table-modal.component';
import { HubNavigationMenuMasterNavigationMenuLookupTableModalComponent } from './hubNavigationMenu-masterNavigationMenu-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubNavigationMenuModal',
    templateUrl: './create-or-edit-hubNavigationMenu-modal.component.html',
})
export class CreateOrEditHubNavigationMenuModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubNavigationMenuHubLookupTableModal', { static: true })
    hubNavigationMenuHubLookupTableModal: HubNavigationMenuHubLookupTableModalComponent;
    @ViewChild('hubNavigationMenuMasterNavigationMenuLookupTableModal', { static: true })
    hubNavigationMenuMasterNavigationMenuLookupTableModal: HubNavigationMenuMasterNavigationMenuLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubNavigationMenu: CreateOrEditHubNavigationMenuDto = new CreateOrEditHubNavigationMenuDto();
    hubName = '';
    masterNavigationMenuName = '';

    hubId: number;
    allMasterNavigationMenu: HubNavigationMenuMasterNavigationMenuLookupTableDto[] = [];
    selectedMasterNavigationMenu: HubNavigationMenuMasterNavigationMenuLookupTableDto[] = [];

    allParentMenu: HubNavigationMenuMasterNavigationMenuLookupTableDto[] = [];
    selectedParentMenu: HubNavigationMenuMasterNavigationMenuLookupTableDto[] = [];
    constructor(
        injector: Injector,
        private _hubNavigationMenusServiceProxy: HubNavigationMenusServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubNavigationMenuId?: number): void {
        if (!hubNavigationMenuId) {
            this.hubNavigationMenu = new CreateOrEditHubNavigationMenuDto();
            this.hubNavigationMenu.id = hubNavigationMenuId;
            this.hubName = '';
            this.masterNavigationMenuName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubNavigationMenusServiceProxy
                .getHubNavigationMenuForEdit(hubNavigationMenuId).subscribe((result) => {
                    this.hubNavigationMenu = result.hubNavigationMenu;
                    if (this.hubId) {
                        this.hubId = result.hubNavigationMenu.hubId;
                    }
                    this.hubName = result.hubName;
                    this.masterNavigationMenuName = result.masterNavigationMenuName;

                    this.active = true;
                    this.modal.show();
                });
        }
        this._hubNavigationMenusServiceProxy.getAllMasterNavigationMenuForLookupTable('','',0,100000).subscribe(result => {
            this.allMasterNavigationMenu = result.items;
        });
    }

    save(): void {
        this.saving = true;
        if (this.hubId) {
            this.hubNavigationMenu.hubId = this.hubId;
        }

        this._hubNavigationMenusServiceProxy
            .createOrEdit(this.hubNavigationMenu)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectHubModal() {
        this.hubNavigationMenuHubLookupTableModal.id = this.hubNavigationMenu.hubId;
        this.hubNavigationMenuHubLookupTableModal.displayName = this.hubName;
        this.hubNavigationMenuHubLookupTableModal.show();
    }
    openSelectMasterNavigationMenuModal() {
        this.hubNavigationMenuMasterNavigationMenuLookupTableModal.id = this.hubNavigationMenu.masterNavigationMenuId;
        this.hubNavigationMenuMasterNavigationMenuLookupTableModal.displayName = this.masterNavigationMenuName;
        this.hubNavigationMenuMasterNavigationMenuLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubNavigationMenu.hubId = null;
        this.hubName = '';
    }
    setMasterNavigationMenuIdNull() {
        this.hubNavigationMenu.masterNavigationMenuId = null;
        this.masterNavigationMenuName = '';
    }

    getNewHubId() {
        this.hubNavigationMenu.hubId = this.hubNavigationMenuHubLookupTableModal.id;
        this.hubName = this.hubNavigationMenuHubLookupTableModal.displayName;
    }
    getNewMasterNavigationMenuId() {
        this.hubNavigationMenu.masterNavigationMenuId = this.hubNavigationMenuMasterNavigationMenuLookupTableModal.id;
        this.masterNavigationMenuName = this.hubNavigationMenuMasterNavigationMenuLookupTableModal.displayName;
    }

    close(): void {
        this.selectedMasterNavigationMenu=null;
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }

    
}
