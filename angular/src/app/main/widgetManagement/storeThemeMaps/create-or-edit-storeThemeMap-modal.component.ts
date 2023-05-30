import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreThemeMapsServiceProxy, CreateOrEditStoreThemeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreThemeMapStoreMasterThemeLookupTableModalComponent } from './storeThemeMap-storeMasterTheme-lookup-table-modal.component';
import { StoreThemeMapStoreLookupTableModalComponent } from './storeThemeMap-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreThemeMapModal',
    templateUrl: './create-or-edit-storeThemeMap-modal.component.html',
})
export class CreateOrEditStoreThemeMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeThemeMapStoreMasterThemeLookupTableModal', { static: true })
    storeThemeMapStoreMasterThemeLookupTableModal: StoreThemeMapStoreMasterThemeLookupTableModalComponent;
    @ViewChild('storeThemeMapStoreLookupTableModal', { static: true })
    storeThemeMapStoreLookupTableModal: StoreThemeMapStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeThemeMap: CreateOrEditStoreThemeMapDto = new CreateOrEditStoreThemeMapDto();

    storeMasterThemeName = '';
    storeName = '';

    constructor(
        injector: Injector,
        private _storeThemeMapsServiceProxy: StoreThemeMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeThemeMapId?: number): void {
        if (!storeThemeMapId) {
            this.storeThemeMap = new CreateOrEditStoreThemeMapDto();
            this.storeThemeMap.id = storeThemeMapId;
            this.storeMasterThemeName = '';
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeThemeMapsServiceProxy.getStoreThemeMapForEdit(storeThemeMapId).subscribe((result) => {
                this.storeThemeMap = result.storeThemeMap;

                this.storeMasterThemeName = result.storeMasterThemeName;
                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeThemeMapsServiceProxy
            .createOrEdit(this.storeThemeMap)
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

    openSelectStoreMasterThemeModal() {
        this.storeThemeMapStoreMasterThemeLookupTableModal.id = this.storeThemeMap.storeMasterThemeId;
        this.storeThemeMapStoreMasterThemeLookupTableModal.displayName = this.storeMasterThemeName;
        this.storeThemeMapStoreMasterThemeLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.storeThemeMapStoreLookupTableModal.id = this.storeThemeMap.storeId;
        this.storeThemeMapStoreLookupTableModal.displayName = this.storeName;
        this.storeThemeMapStoreLookupTableModal.show();
    }

    setStoreMasterThemeIdNull() {
        this.storeThemeMap.storeMasterThemeId = null;
        this.storeMasterThemeName = '';
    }
    setStoreIdNull() {
        this.storeThemeMap.storeId = null;
        this.storeName = '';
    }

    getNewStoreMasterThemeId() {
        this.storeThemeMap.storeMasterThemeId = this.storeThemeMapStoreMasterThemeLookupTableModal.id;
        this.storeMasterThemeName = this.storeThemeMapStoreMasterThemeLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.storeThemeMap.storeId = this.storeThemeMapStoreLookupTableModal.id;
        this.storeName = this.storeThemeMapStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
