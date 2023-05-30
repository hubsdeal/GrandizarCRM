import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubWidgetMapsServiceProxy, CreateOrEditHubWidgetMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubWidgetMapHubLookupTableModalComponent } from './hubWidgetMap-hub-lookup-table-modal.component';
import { HubWidgetMapMasterWidgetLookupTableModalComponent } from './hubWidgetMap-masterWidget-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubWidgetMapModal',
    templateUrl: './create-or-edit-hubWidgetMap-modal.component.html',
})
export class CreateOrEditHubWidgetMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubWidgetMapHubLookupTableModal', { static: true })
    hubWidgetMapHubLookupTableModal: HubWidgetMapHubLookupTableModalComponent;
    @ViewChild('hubWidgetMapMasterWidgetLookupTableModal', { static: true })
    hubWidgetMapMasterWidgetLookupTableModal: HubWidgetMapMasterWidgetLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubWidgetMap: CreateOrEditHubWidgetMapDto = new CreateOrEditHubWidgetMapDto();

    hubName = '';
    masterWidgetName = '';

    constructor(
        injector: Injector,
        private _hubWidgetMapsServiceProxy: HubWidgetMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubWidgetMapId?: number): void {
        if (!hubWidgetMapId) {
            this.hubWidgetMap = new CreateOrEditHubWidgetMapDto();
            this.hubWidgetMap.id = hubWidgetMapId;
            this.hubName = '';
            this.masterWidgetName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubWidgetMapsServiceProxy.getHubWidgetMapForEdit(hubWidgetMapId).subscribe((result) => {
                this.hubWidgetMap = result.hubWidgetMap;

                this.hubName = result.hubName;
                this.masterWidgetName = result.masterWidgetName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._hubWidgetMapsServiceProxy
            .createOrEdit(this.hubWidgetMap)
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
        this.hubWidgetMapHubLookupTableModal.id = this.hubWidgetMap.hubId;
        this.hubWidgetMapHubLookupTableModal.displayName = this.hubName;
        this.hubWidgetMapHubLookupTableModal.show();
    }
    openSelectMasterWidgetModal() {
        this.hubWidgetMapMasterWidgetLookupTableModal.id = this.hubWidgetMap.masterWidgetId;
        this.hubWidgetMapMasterWidgetLookupTableModal.displayName = this.masterWidgetName;
        this.hubWidgetMapMasterWidgetLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubWidgetMap.hubId = null;
        this.hubName = '';
    }
    setMasterWidgetIdNull() {
        this.hubWidgetMap.masterWidgetId = null;
        this.masterWidgetName = '';
    }

    getNewHubId() {
        this.hubWidgetMap.hubId = this.hubWidgetMapHubLookupTableModal.id;
        this.hubName = this.hubWidgetMapHubLookupTableModal.displayName;
    }
    getNewMasterWidgetId() {
        this.hubWidgetMap.masterWidgetId = this.hubWidgetMapMasterWidgetLookupTableModal.id;
        this.masterWidgetName = this.hubWidgetMapMasterWidgetLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
