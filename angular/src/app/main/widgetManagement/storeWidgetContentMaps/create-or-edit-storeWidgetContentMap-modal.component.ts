import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreWidgetContentMapsServiceProxy,
    CreateOrEditStoreWidgetContentMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreWidgetContentMapStoreWidgetMapLookupTableModalComponent } from './storeWidgetContentMap-storeWidgetMap-lookup-table-modal.component';
import { StoreWidgetContentMapContentLookupTableModalComponent } from './storeWidgetContentMap-content-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreWidgetContentMapModal',
    templateUrl: './create-or-edit-storeWidgetContentMap-modal.component.html',
})
export class CreateOrEditStoreWidgetContentMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeWidgetContentMapStoreWidgetMapLookupTableModal', { static: true })
    storeWidgetContentMapStoreWidgetMapLookupTableModal: StoreWidgetContentMapStoreWidgetMapLookupTableModalComponent;
    @ViewChild('storeWidgetContentMapContentLookupTableModal', { static: true })
    storeWidgetContentMapContentLookupTableModal: StoreWidgetContentMapContentLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeWidgetContentMap: CreateOrEditStoreWidgetContentMapDto = new CreateOrEditStoreWidgetContentMapDto();

    storeWidgetMapCustomName = '';
    contentTitle = '';

    constructor(
        injector: Injector,
        private _storeWidgetContentMapsServiceProxy: StoreWidgetContentMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeWidgetContentMapId?: number): void {
        if (!storeWidgetContentMapId) {
            this.storeWidgetContentMap = new CreateOrEditStoreWidgetContentMapDto();
            this.storeWidgetContentMap.id = storeWidgetContentMapId;
            this.storeWidgetMapCustomName = '';
            this.contentTitle = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeWidgetContentMapsServiceProxy
                .getStoreWidgetContentMapForEdit(storeWidgetContentMapId)
                .subscribe((result) => {
                    this.storeWidgetContentMap = result.storeWidgetContentMap;

                    this.storeWidgetMapCustomName = result.storeWidgetMapCustomName;
                    this.contentTitle = result.contentTitle;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._storeWidgetContentMapsServiceProxy
            .createOrEdit(this.storeWidgetContentMap)
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

    openSelectStoreWidgetMapModal() {
        this.storeWidgetContentMapStoreWidgetMapLookupTableModal.id = this.storeWidgetContentMap.storeWidgetMapId;
        this.storeWidgetContentMapStoreWidgetMapLookupTableModal.displayName = this.storeWidgetMapCustomName;
        this.storeWidgetContentMapStoreWidgetMapLookupTableModal.show();
    }
    openSelectContentModal() {
        this.storeWidgetContentMapContentLookupTableModal.id = this.storeWidgetContentMap.contentId;
        this.storeWidgetContentMapContentLookupTableModal.displayName = this.contentTitle;
        this.storeWidgetContentMapContentLookupTableModal.show();
    }

    setStoreWidgetMapIdNull() {
        this.storeWidgetContentMap.storeWidgetMapId = null;
        this.storeWidgetMapCustomName = '';
    }
    setContentIdNull() {
        this.storeWidgetContentMap.contentId = null;
        this.contentTitle = '';
    }

    getNewStoreWidgetMapId() {
        this.storeWidgetContentMap.storeWidgetMapId = this.storeWidgetContentMapStoreWidgetMapLookupTableModal.id;
        this.storeWidgetMapCustomName = this.storeWidgetContentMapStoreWidgetMapLookupTableModal.displayName;
    }
    getNewContentId() {
        this.storeWidgetContentMap.contentId = this.storeWidgetContentMapContentLookupTableModal.id;
        this.contentTitle = this.storeWidgetContentMapContentLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
