import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreMasterThemesServiceProxy,
    CreateOrEditStoreMasterThemeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditStoreMasterThemeModal',
    templateUrl: './create-or-edit-storeMasterTheme-modal.component.html',
})
export class CreateOrEditStoreMasterThemeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeMasterTheme: CreateOrEditStoreMasterThemeDto = new CreateOrEditStoreMasterThemeDto();

    constructor(
        injector: Injector,
        private _storeMasterThemesServiceProxy: StoreMasterThemesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeMasterThemeId?: number): void {
        if (!storeMasterThemeId) {
            this.storeMasterTheme = new CreateOrEditStoreMasterThemeDto();
            this.storeMasterTheme.id = storeMasterThemeId;

            this.active = true;
            this.modal.show();
        } else {
            this._storeMasterThemesServiceProxy.getStoreMasterThemeForEdit(storeMasterThemeId).subscribe((result) => {
                this.storeMasterTheme = result.storeMasterTheme;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeMasterThemesServiceProxy
            .createOrEdit(this.storeMasterTheme)
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
