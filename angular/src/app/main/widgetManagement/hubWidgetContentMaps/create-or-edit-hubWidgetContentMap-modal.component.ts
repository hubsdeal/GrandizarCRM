import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubWidgetContentMapsServiceProxy,
    CreateOrEditHubWidgetContentMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubWidgetContentMapHubWidgetMapLookupTableModalComponent } from './hubWidgetContentMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetContentMapContentLookupTableModalComponent } from './hubWidgetContentMap-content-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubWidgetContentMapModal',
    templateUrl: './create-or-edit-hubWidgetContentMap-modal.component.html',
})
export class CreateOrEditHubWidgetContentMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubWidgetContentMapHubWidgetMapLookupTableModal', { static: true })
    hubWidgetContentMapHubWidgetMapLookupTableModal: HubWidgetContentMapHubWidgetMapLookupTableModalComponent;
    @ViewChild('hubWidgetContentMapContentLookupTableModal', { static: true })
    hubWidgetContentMapContentLookupTableModal: HubWidgetContentMapContentLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubWidgetContentMap: CreateOrEditHubWidgetContentMapDto = new CreateOrEditHubWidgetContentMapDto();

    hubWidgetMapCustomName = '';
    contentTitle = '';

    constructor(
        injector: Injector,
        private _hubWidgetContentMapsServiceProxy: HubWidgetContentMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubWidgetContentMapId?: number): void {
        if (!hubWidgetContentMapId) {
            this.hubWidgetContentMap = new CreateOrEditHubWidgetContentMapDto();
            this.hubWidgetContentMap.id = hubWidgetContentMapId;
            this.hubWidgetMapCustomName = '';
            this.contentTitle = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubWidgetContentMapsServiceProxy
                .getHubWidgetContentMapForEdit(hubWidgetContentMapId)
                .subscribe((result) => {
                    this.hubWidgetContentMap = result.hubWidgetContentMap;

                    this.hubWidgetMapCustomName = result.hubWidgetMapCustomName;
                    this.contentTitle = result.contentTitle;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._hubWidgetContentMapsServiceProxy
            .createOrEdit(this.hubWidgetContentMap)
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

    openSelectHubWidgetMapModal() {
        this.hubWidgetContentMapHubWidgetMapLookupTableModal.id = this.hubWidgetContentMap.hubWidgetMapId;
        this.hubWidgetContentMapHubWidgetMapLookupTableModal.displayName = this.hubWidgetMapCustomName;
        this.hubWidgetContentMapHubWidgetMapLookupTableModal.show();
    }
    openSelectContentModal() {
        this.hubWidgetContentMapContentLookupTableModal.id = this.hubWidgetContentMap.contentId;
        this.hubWidgetContentMapContentLookupTableModal.displayName = this.contentTitle;
        this.hubWidgetContentMapContentLookupTableModal.show();
    }

    setHubWidgetMapIdNull() {
        this.hubWidgetContentMap.hubWidgetMapId = null;
        this.hubWidgetMapCustomName = '';
    }
    setContentIdNull() {
        this.hubWidgetContentMap.contentId = null;
        this.contentTitle = '';
    }

    getNewHubWidgetMapId() {
        this.hubWidgetContentMap.hubWidgetMapId = this.hubWidgetContentMapHubWidgetMapLookupTableModal.id;
        this.hubWidgetMapCustomName = this.hubWidgetContentMapHubWidgetMapLookupTableModal.displayName;
    }
    getNewContentId() {
        this.hubWidgetContentMap.contentId = this.hubWidgetContentMapContentLookupTableModal.id;
        this.contentTitle = this.hubWidgetContentMapContentLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
