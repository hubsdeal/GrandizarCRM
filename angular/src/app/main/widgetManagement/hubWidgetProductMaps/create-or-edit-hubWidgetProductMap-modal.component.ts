import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubWidgetProductMapsServiceProxy,
    CreateOrEditHubWidgetProductMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubWidgetProductMapHubWidgetMapLookupTableModalComponent } from './hubWidgetProductMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetProductMapProductLookupTableModalComponent } from './hubWidgetProductMap-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubWidgetProductMapModal',
    templateUrl: './create-or-edit-hubWidgetProductMap-modal.component.html',
})
export class CreateOrEditHubWidgetProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubWidgetProductMapHubWidgetMapLookupTableModal', { static: true })
    hubWidgetProductMapHubWidgetMapLookupTableModal: HubWidgetProductMapHubWidgetMapLookupTableModalComponent;
    @ViewChild('hubWidgetProductMapProductLookupTableModal', { static: true })
    hubWidgetProductMapProductLookupTableModal: HubWidgetProductMapProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubWidgetProductMap: CreateOrEditHubWidgetProductMapDto = new CreateOrEditHubWidgetProductMapDto();

    hubWidgetMapCustomName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _hubWidgetProductMapsServiceProxy: HubWidgetProductMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubWidgetProductMapId?: number): void {
        if (!hubWidgetProductMapId) {
            this.hubWidgetProductMap = new CreateOrEditHubWidgetProductMapDto();
            this.hubWidgetProductMap.id = hubWidgetProductMapId;
            this.hubWidgetMapCustomName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubWidgetProductMapsServiceProxy
                .getHubWidgetProductMapForEdit(hubWidgetProductMapId)
                .subscribe((result) => {
                    this.hubWidgetProductMap = result.hubWidgetProductMap;

                    this.hubWidgetMapCustomName = result.hubWidgetMapCustomName;
                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._hubWidgetProductMapsServiceProxy
            .createOrEdit(this.hubWidgetProductMap)
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
        this.hubWidgetProductMapHubWidgetMapLookupTableModal.id = this.hubWidgetProductMap.hubWidgetMapId;
        this.hubWidgetProductMapHubWidgetMapLookupTableModal.displayName = this.hubWidgetMapCustomName;
        this.hubWidgetProductMapHubWidgetMapLookupTableModal.show();
    }
    openSelectProductModal() {
        this.hubWidgetProductMapProductLookupTableModal.id = this.hubWidgetProductMap.productId;
        this.hubWidgetProductMapProductLookupTableModal.displayName = this.productName;
        this.hubWidgetProductMapProductLookupTableModal.show();
    }

    setHubWidgetMapIdNull() {
        this.hubWidgetProductMap.hubWidgetMapId = null;
        this.hubWidgetMapCustomName = '';
    }
    setProductIdNull() {
        this.hubWidgetProductMap.productId = null;
        this.productName = '';
    }

    getNewHubWidgetMapId() {
        this.hubWidgetProductMap.hubWidgetMapId = this.hubWidgetProductMapHubWidgetMapLookupTableModal.id;
        this.hubWidgetMapCustomName = this.hubWidgetProductMapHubWidgetMapLookupTableModal.displayName;
    }
    getNewProductId() {
        this.hubWidgetProductMap.productId = this.hubWidgetProductMapProductLookupTableModal.id;
        this.productName = this.hubWidgetProductMapProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
