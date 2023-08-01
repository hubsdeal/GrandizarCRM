import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductNotesServiceProxy, CreateOrEditProductNoteDto, HostDashboardServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductNoteProductLookupTableModalComponent } from './productNote-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductNoteModal',
    templateUrl: './create-or-edit-productNote-modal.component.html',
})
export class CreateOrEditProductNoteModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productNoteProductLookupTableModal', { static: true })
    productNoteProductLookupTableModal: ProductNoteProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productNote: CreateOrEditProductNoteDto = new CreateOrEditProductNoteDto();

    productName = '';
    productId: number;

    constructor(
        injector: Injector,
        private _productNotesServiceProxy: ProductNotesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    show(productNoteId?: number): void {
        if (!productNoteId) {
            this.productNote = new CreateOrEditProductNoteDto();
            this.productNote.id = productNoteId;
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productNotesServiceProxy.getProductNoteForEdit(productNoteId).subscribe((result) => {
                this.productNote = result.productNote;

                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        if (this.productId) {
            this.productNote.productId = this.productId;
        }
        this._productNotesServiceProxy
            .createOrEdit(this.productNote)
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

    openSelectProductModal() {
        this.productNoteProductLookupTableModal.id = this.productNote.productId;
        this.productNoteProductLookupTableModal.displayName = this.productName;
        this.productNoteProductLookupTableModal.show();
    }

    setProductIdNull() {
        this.productNote.productId = null;
        this.productName = '';
    }

    getNewProductId() {
        this.productNote.productId = this.productNoteProductLookupTableModal.id;
        this.productName = this.productNoteProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void { }
}
