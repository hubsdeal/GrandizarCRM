import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessJobMapsServiceProxy, CreateOrEditBusinessJobMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessJobMapBusinessLookupTableModalComponent } from './businessJobMap-business-lookup-table-modal.component';
import { BusinessJobMapJobLookupTableModalComponent } from './businessJobMap-job-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessJobMapModal',
    templateUrl: './create-or-edit-businessJobMap-modal.component.html',
})
export class CreateOrEditBusinessJobMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessJobMapBusinessLookupTableModal', { static: true })
    businessJobMapBusinessLookupTableModal: BusinessJobMapBusinessLookupTableModalComponent;
    @ViewChild('businessJobMapJobLookupTableModal', { static: true })
    businessJobMapJobLookupTableModal: BusinessJobMapJobLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessJobMap: CreateOrEditBusinessJobMapDto = new CreateOrEditBusinessJobMapDto();

    businessName = '';
    jobTitle = '';

    constructor(
        injector: Injector,
        private _businessJobMapsServiceProxy: BusinessJobMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessJobMapId?: number): void {
        if (!businessJobMapId) {
            this.businessJobMap = new CreateOrEditBusinessJobMapDto();
            this.businessJobMap.id = businessJobMapId;
            this.businessName = '';
            this.jobTitle = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessJobMapsServiceProxy.getBusinessJobMapForEdit(businessJobMapId).subscribe((result) => {
                this.businessJobMap = result.businessJobMap;

                this.businessName = result.businessName;
                this.jobTitle = result.jobTitle;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessJobMapsServiceProxy
            .createOrEdit(this.businessJobMap)
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

    openSelectBusinessModal() {
        this.businessJobMapBusinessLookupTableModal.id = this.businessJobMap.businessId;
        this.businessJobMapBusinessLookupTableModal.displayName = this.businessName;
        this.businessJobMapBusinessLookupTableModal.show();
    }
    openSelectJobModal() {
        this.businessJobMapJobLookupTableModal.id = this.businessJobMap.jobId;
        this.businessJobMapJobLookupTableModal.displayName = this.jobTitle;
        this.businessJobMapJobLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessJobMap.businessId = null;
        this.businessName = '';
    }
    setJobIdNull() {
        this.businessJobMap.jobId = null;
        this.jobTitle = '';
    }

    getNewBusinessId() {
        this.businessJobMap.businessId = this.businessJobMapBusinessLookupTableModal.id;
        this.businessName = this.businessJobMapBusinessLookupTableModal.displayName;
    }
    getNewJobId() {
        this.businessJobMap.jobId = this.businessJobMapJobLookupTableModal.id;
        this.jobTitle = this.businessJobMapJobLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
