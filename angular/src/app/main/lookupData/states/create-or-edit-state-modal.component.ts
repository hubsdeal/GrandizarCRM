import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StatesServiceProxy,
    CreateOrEditStateDto,
    StateCountryLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditStateModal',
    templateUrl: './create-or-edit-state-modal.component.html',
})
export class CreateOrEditStateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    state: CreateOrEditStateDto = new CreateOrEditStateDto();

    countryName = '';

    allCountrys: StateCountryLookupTableDto[];

    constructor(
        injector: Injector,
        private _statesServiceProxy: StatesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(stateId?: number): void {
        if (!stateId) {
            this.state = new CreateOrEditStateDto();
            this.state.id = stateId;
            this.countryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._statesServiceProxy.getStateForEdit(stateId).subscribe((result) => {
                this.state = result.state;

                this.countryName = result.countryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._statesServiceProxy.getAllCountryForTableDropdown().subscribe((result) => {
            this.allCountrys = result;
        });
    }

    save(): void {
        this.saving = true;

        this._statesServiceProxy
            .createOrEdit(this.state)
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
