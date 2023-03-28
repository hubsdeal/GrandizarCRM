import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    MasterNavigationMenusServiceProxy,
    CreateOrEditMasterNavigationMenuDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMasterNavigationMenuModal',
    templateUrl: './create-or-edit-masterNavigationMenu-modal.component.html',
})
export class CreateOrEditMasterNavigationMenuModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    masterNavigationMenu: CreateOrEditMasterNavigationMenuDto = new CreateOrEditMasterNavigationMenuDto();

    constructor(
        injector: Injector,
        private _masterNavigationMenusServiceProxy: MasterNavigationMenusServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(masterNavigationMenuId?: number): void {
        if (!masterNavigationMenuId) {
            this.masterNavigationMenu = new CreateOrEditMasterNavigationMenuDto();
            this.masterNavigationMenu.id = masterNavigationMenuId;

            this.active = true;
            this.modal.show();
        } else {
            this._masterNavigationMenusServiceProxy
                .getMasterNavigationMenuForEdit(masterNavigationMenuId)
                .subscribe((result) => {
                    this.masterNavigationMenu = result.masterNavigationMenu;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._masterNavigationMenusServiceProxy
            .createOrEdit(this.masterNavigationMenu)
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
