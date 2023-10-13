import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadsServiceProxy, CreateOrEditLeadDto, ContactsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadContactLookupTableModalComponent } from './lead-contact-lookup-table-modal.component';
import { LeadBusinessLookupTableModalComponent } from './lead-business-lookup-table-modal.component';
import { LeadProductLookupTableModalComponent } from './lead-product-lookup-table-modal.component';
import { LeadProductCategoryLookupTableModalComponent } from './lead-productCategory-lookup-table-modal.component';
import { LeadStoreLookupTableModalComponent } from './lead-store-lookup-table-modal.component';
import { LeadEmployeeLookupTableModalComponent } from './lead-employee-lookup-table-modal.component';
import { LeadLeadSourceLookupTableModalComponent } from './lead-leadSource-lookup-table-modal.component';
import { LeadLeadPipelineStageLookupTableModalComponent } from './lead-leadPipelineStage-lookup-table-modal.component';
import { LeadHubLookupTableModalComponent } from './lead-hub-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadModal',
    templateUrl: './create-or-edit-lead-modal.component.html',
})
export class CreateOrEditLeadModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadContactLookupTableModal', { static: true })
    leadContactLookupTableModal: LeadContactLookupTableModalComponent;
    @ViewChild('leadBusinessLookupTableModal', { static: true })
    leadBusinessLookupTableModal: LeadBusinessLookupTableModalComponent;
    @ViewChild('leadProductLookupTableModal', { static: true })
    leadProductLookupTableModal: LeadProductLookupTableModalComponent;
    @ViewChild('leadProductCategoryLookupTableModal', { static: true })
    leadProductCategoryLookupTableModal: LeadProductCategoryLookupTableModalComponent;
    @ViewChild('leadStoreLookupTableModal', { static: true })
    leadStoreLookupTableModal: LeadStoreLookupTableModalComponent;
    @ViewChild('leadEmployeeLookupTableModal', { static: true })
    leadEmployeeLookupTableModal: LeadEmployeeLookupTableModalComponent;
    @ViewChild('leadLeadSourceLookupTableModal', { static: true })
    leadLeadSourceLookupTableModal: LeadLeadSourceLookupTableModalComponent;
    @ViewChild('leadLeadPipelineStageLookupTableModal', { static: true })
    leadLeadPipelineStageLookupTableModal: LeadLeadPipelineStageLookupTableModalComponent;
    @ViewChild('leadHubLookupTableModal', { static: true }) leadHubLookupTableModal: LeadHubLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    lead: CreateOrEditLeadDto = new CreateOrEditLeadDto();

    contactFullName = '';
    businessName = '';
    productName = '';
    productCategoryName = '';
    storeName = '';
    employeeName = '';
    leadSourceName = '';
    leadPipelineStageName = '';
    hubName = '';

    constructor(
        injector: Injector,
        private _leadsServiceProxy: LeadsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _contactServiceProxy: ContactsServiceProxy,
    ) {
        super(injector);
    }

    show(leadId?: number): void {
        if (!leadId) {
            this.lead = new CreateOrEditLeadDto();
            this.lead.id = leadId;
            this.lead.createdDate = this._dateTimeService.getStartOfDay();
            this.lead.expectedClosingDate = this._dateTimeService.getStartOfDay();
            this.contactFullName = '';
            this.businessName = '';
            this.productName = '';
            this.productCategoryName = '';
            this.storeName = '';
            this.employeeName = '';
            this.leadSourceName = '';
            this.leadPipelineStageName = '';
            this.hubName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadsServiceProxy.getLeadForEdit(leadId).subscribe((result) => {
                this.lead = result.lead;

                this.contactFullName = result.contactFullName;
                this.businessName = result.businessName;
                this.productName = result.productName;
                this.productCategoryName = result.productCategoryName;
                this.storeName = result.storeName;
                this.employeeName = result.employeeName;
                this.leadSourceName = result.leadSourceName;
                this.leadPipelineStageName = result.leadPipelineStageName;
                this.hubName = result.hubName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadsServiceProxy
            .createOrEdit(this.lead)
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

    openSelectContactModal() {
        this.leadContactLookupTableModal.id = this.lead.contactId;
        this.leadContactLookupTableModal.displayName = this.contactFullName;
        this.leadContactLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.leadBusinessLookupTableModal.id = this.lead.businessId;
        this.leadBusinessLookupTableModal.displayName = this.businessName;
        this.leadBusinessLookupTableModal.show();
    }
    openSelectProductModal() {
        this.leadProductLookupTableModal.id = this.lead.productId;
        this.leadProductLookupTableModal.displayName = this.productName;
        this.leadProductLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.leadProductCategoryLookupTableModal.id = this.lead.productCategoryId;
        this.leadProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.leadProductCategoryLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.leadStoreLookupTableModal.id = this.lead.storeId;
        this.leadStoreLookupTableModal.displayName = this.storeName;
        this.leadStoreLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.leadEmployeeLookupTableModal.id = this.lead.employeeId;
        this.leadEmployeeLookupTableModal.displayName = this.employeeName;
        this.leadEmployeeLookupTableModal.show();
    }
    openSelectLeadSourceModal() {
        this.leadLeadSourceLookupTableModal.id = this.lead.leadSourceId;
        this.leadLeadSourceLookupTableModal.displayName = this.leadSourceName;
        this.leadLeadSourceLookupTableModal.show();
    }
    openSelectLeadPipelineStageModal() {
        this.leadLeadPipelineStageLookupTableModal.id = this.lead.leadPipelineStageId;
        this.leadLeadPipelineStageLookupTableModal.displayName = this.leadPipelineStageName;
        this.leadLeadPipelineStageLookupTableModal.show();
    }
    openSelectHubModal() {
        this.leadHubLookupTableModal.id = this.lead.hubId;
        this.leadHubLookupTableModal.displayName = this.hubName;
        this.leadHubLookupTableModal.show();
    }

    setContactIdNull() {
        this.lead.contactId = null;
        this.contactFullName = '';
    }
    setBusinessIdNull() {
        this.lead.businessId = null;
        this.businessName = '';
    }
    setProductIdNull() {
        this.lead.productId = null;
        this.productName = '';
    }
    setProductCategoryIdNull() {
        this.lead.productCategoryId = null;
        this.productCategoryName = '';
    }
    setStoreIdNull() {
        this.lead.storeId = null;
        this.storeName = '';
    }
    setEmployeeIdNull() {
        this.lead.employeeId = null;
        this.employeeName = '';
    }
    setLeadSourceIdNull() {
        this.lead.leadSourceId = null;
        this.leadSourceName = '';
    }
    setLeadPipelineStageIdNull() {
        this.lead.leadPipelineStageId = null;
        this.leadPipelineStageName = '';
    }
    setHubIdNull() {
        this.lead.hubId = null;
        this.hubName = '';
    }

    getNewContactId() {
        this.lead.contactId = this.leadContactLookupTableModal.id;
        this.contactFullName = this.leadContactLookupTableModal.displayName;
        this._contactServiceProxy.getContactForEdit(this.lead.contactId).subscribe((result) => {
            this.lead.firstName = result.contact.firstName;
            this.lead.lastName = result.contact.lastName;
            this.lead.email = result.contact.businessEmail;
            this.lead.phone = result.contact.officePhone;
            this.lead.jobTitle = result.contact.jobTitle;
            this.lead.company = result.contact.companyName;
        });
    }
    getNewBusinessId() {
        this.lead.businessId = this.leadBusinessLookupTableModal.id;
        this.businessName = this.leadBusinessLookupTableModal.displayName;
    }
    getNewProductId() {
        this.lead.productId = this.leadProductLookupTableModal.id;
        this.productName = this.leadProductLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.lead.productCategoryId = this.leadProductCategoryLookupTableModal.id;
        this.productCategoryName = this.leadProductCategoryLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.lead.storeId = this.leadStoreLookupTableModal.id;
        this.storeName = this.leadStoreLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.lead.employeeId = this.leadEmployeeLookupTableModal.id;
        this.employeeName = this.leadEmployeeLookupTableModal.displayName;
    }
    getNewLeadSourceId() {
        this.lead.leadSourceId = this.leadLeadSourceLookupTableModal.id;
        this.leadSourceName = this.leadLeadSourceLookupTableModal.displayName;
    }
    getNewLeadPipelineStageId() {
        this.lead.leadPipelineStageId = this.leadLeadPipelineStageLookupTableModal.id;
        this.leadPipelineStageName = this.leadLeadPipelineStageLookupTableModal.displayName;
    }
    getNewHubId() {
        this.lead.hubId = this.leadHubLookupTableModal.id;
        this.hubName = this.leadHubLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
