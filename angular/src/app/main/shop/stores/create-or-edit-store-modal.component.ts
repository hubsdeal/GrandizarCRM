import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoresServiceProxy,
    CreateOrEditStoreDto,
    StoreCountryLookupTableDto,
    StoreStateLookupTableDto,
    StoreRatingLikeLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreMediaLibraryLookupTableModalComponent } from './store-mediaLibrary-lookup-table-modal.component';
import { StoreMasterTagLookupTableModalComponent } from './store-masterTag-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';
import { Router } from '@angular/router';
import { result } from 'lodash-es';

@Component({
    selector: 'createOrEditStoreModal',
    templateUrl: './create-or-edit-store-modal.component.html',
})
export class CreateOrEditStoreModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeMediaLibraryLookupTableModal', { static: true })
    storeMediaLibraryLookupTableModal: StoreMediaLibraryLookupTableModalComponent;
    @ViewChild('storeMasterTagLookupTableModal', { static: true })
    storeMasterTagLookupTableModal: StoreMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    store: CreateOrEditStoreDto = new CreateOrEditStoreDto();

    mediaLibraryName = '';
    countryName = '';
    stateName = '';
    ratingLikeName = '';
    masterTagName = '';

    allCountrys: StoreCountryLookupTableDto[];
    allStates: StoreStateLookupTableDto[];
    allRatingLikes: StoreRatingLikeLookupTableDto[];

    localOrVirtualStoreOptions: SelectItem[];

    constructor(
        injector: Injector,
        private _storesServiceProxy: StoresServiceProxy,
        private _dateTimeService: DateTimeService,
        private _router: Router
    ) {
        super(injector);
    }

    show(storeId?: number): void {
        if (!storeId) {
            this.store = new CreateOrEditStoreDto();
            this.store.id = storeId;
            this.mediaLibraryName = '';
            this.countryName = '';
            this.stateName = '';
            this.ratingLikeName = '';
            this.masterTagName = '';
            this.store.isLocalOrOnlineStore = false;
            this.active = true;
            this.modal.show();
        } else {
            this._storesServiceProxy.getStoreForEdit(storeId).subscribe((result) => {
                this.store = result.store;

                this.mediaLibraryName = result.mediaLibraryName;
                this.countryName = result.countryName;
                this.stateName = result.stateName;
                this.ratingLikeName = result.ratingLikeName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });

        }
        this._storesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
        this._storesServiceProxy.getAllStateForTableDropdown().subscribe((result) => {
            this.allStates = result;
        });
        this._storesServiceProxy.getAllRatingLikeForTableDropdown().subscribe((result) => {
            this.allRatingLikes = result;
        });
        this.localOrVirtualStoreOptions = [{ label: 'Local Store', value: false }, { label: 'Virtual Store', value: true }];
    }

    save(): void {
        this.saving = true;

        this._storesServiceProxy
            .createOrEdit(this.store)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
                this._router.navigate(['/app/main/shop/stores/dashboard/',result])
            });
            //todo - Once you click submit based on the full addess system will find automatically the lat and long, CITY, state, country will save it in the database
            //auto gpt function based on the web site address it will collect automatically the store data and save it in the database
            // display sequence number will be auto inserted based on the sequence number
    }

    openSelectMediaLibraryModal() {
        this.storeMediaLibraryLookupTableModal.id = this.store.logoMediaLibraryId;
        this.storeMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.storeMediaLibraryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.storeMasterTagLookupTableModal.id = this.store.storeCategoryId;
        this.storeMasterTagLookupTableModal.displayName = this.masterTagName;
        this.storeMasterTagLookupTableModal.show();
    }

    setLogoMediaLibraryIdNull() {
        this.store.logoMediaLibraryId = null;
        this.mediaLibraryName = '';
    }
    setStoreCategoryIdNull() {
        this.store.storeCategoryId = null;
        this.masterTagName = '';
    }

    getNewLogoMediaLibraryId() {
        this.store.logoMediaLibraryId = this.storeMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.storeMediaLibraryLookupTableModal.displayName;
    }
    getNewStoreCategoryId() {
        this.store.storeCategoryId = this.storeMasterTagLookupTableModal.id;
        this.masterTagName = this.storeMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
}
