﻿import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductTaskMapsServiceProxy, CreateOrEditProductTaskMapDto, CreateOrEditTaskEventDto, TaskEventTaskStatusLookupTableDto, TaskEventsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductTaskMapProductLookupTableModalComponent } from './productTaskMap-product-lookup-table-modal.component';
import { ProductTaskMapTaskEventLookupTableModalComponent } from './productTaskMap-taskEvent-lookup-table-modal.component';
import { ProductTaskMapProductCategoryLookupTableModalComponent } from './productTaskMap-productCategory-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';
import { result } from 'lodash-es';

@Component({
    selector: 'createOrEditProductTaskMapModal',
    templateUrl: './create-or-edit-productTaskMap-modal.component.html',
})
export class CreateOrEditProductTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productTaskMapProductLookupTableModal', { static: true })
    productTaskMapProductLookupTableModal: ProductTaskMapProductLookupTableModalComponent;
    @ViewChild('productTaskMapTaskEventLookupTableModal', { static: true })
    productTaskMapTaskEventLookupTableModal: ProductTaskMapTaskEventLookupTableModalComponent;
    @ViewChild('productTaskMapProductCategoryLookupTableModal', { static: true })
    productTaskMapProductCategoryLookupTableModal: ProductTaskMapProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productTaskMap: CreateOrEditProductTaskMapDto = new CreateOrEditProductTaskMapDto();

    productName = '';
    taskEventName = '';
    productCategoryName = '';

    taskEvent: CreateOrEditProductTaskMapDto = new CreateOrEditProductTaskMapDto();

    taskStatusName = '';

    allTaskStatuss: TaskEventTaskStatusLookupTableDto[];

    taskStatusOptions: SelectItem[]; 
    priorityOptions: SelectItem[];

    selectedTemplate: any;
    allTemplate: any[] = []

    selectedTeam: any;
    allTeams: any[] = [{ id: 1, displayName: "Team 1" }, { id: 2, displayName: "Team 2" }, { id: 3, displayName: "Team 3" }]

    selectedTag: any;
    allTags: any[] = [{ id: 1, displayName: "Tag 1" }, { id: 2, displayName: "Tag 2" }, { id: 3, displayName: "Tag 3" }]
    productId: number;

    constructor(
        injector: Injector,
        private _productTaskMapsServiceProxy: ProductTaskMapsServiceProxy,
        private _taskEventsServiceProxy: TaskEventsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productTaskMapId?: number): void {
        this.getAllTaskTemplates();
        if (!productTaskMapId) {
            this.productTaskMap = new CreateOrEditProductTaskMapDto();
            this.productTaskMap.id = productTaskMapId;
            this.productName = '';
            this.taskEventName = '';
            this.productCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productTaskMapsServiceProxy.getProductTaskMapForEdit(productTaskMapId).subscribe((result) => {
                this.productTaskMap = result.productTaskMap;

                this.productName = result.productName;
                this.taskEventName = result.taskEventName;
                this.productCategoryName = result.productCategoryName;

                this.active = true;
                this.modal.show();
            });
        }
        this._taskEventsServiceProxy.getAllTaskStatusForTableDropdown().subscribe((result) => {
            this.allTaskStatuss = result;
        });
        this.taskStatusOptions = [{ label: 'Completed', value: true }, { label: 'Open', value: false }];
        this.priorityOptions = [{ label: 'High', value: true }, { label: 'Low', value: false }];
    }

    save(): void {
        this.saving = true;
        if (this.productId) {
            this.taskEvent.productId = this.productId;
        }
        this._productTaskMapsServiceProxy
            .createOrEdit(this.taskEvent)
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
        this.productTaskMapProductLookupTableModal.id = this.productTaskMap.productId;
        this.productTaskMapProductLookupTableModal.displayName = this.productName;
        this.productTaskMapProductLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.productTaskMapTaskEventLookupTableModal.id = this.productTaskMap.taskEventId;
        this.productTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.productTaskMapTaskEventLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.productTaskMapProductCategoryLookupTableModal.id = this.productTaskMap.productCategoryId;
        this.productTaskMapProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productTaskMapProductCategoryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productTaskMap.productId = null;
        this.productName = '';
    }
    setTaskEventIdNull() {
        this.productTaskMap.taskEventId = null;
        this.taskEventName = '';
    }
    setProductCategoryIdNull() {
        this.productTaskMap.productCategoryId = null;
        this.productCategoryName = '';
    }

    getNewProductId() {
        this.productTaskMap.productId = this.productTaskMapProductLookupTableModal.id;
        this.productName = this.productTaskMapProductLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.productTaskMap.taskEventId = this.productTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.productTaskMapTaskEventLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.productTaskMap.productCategoryId = this.productTaskMapProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productTaskMapProductCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    startTimeValue(value: any) {
        this.taskEvent.startTime = value;
    }

    endTimeValue(value: any) {
        this.taskEvent.endTime = value;
    }


    ngOnInit(): void { }

    getAllTaskTemplates() {
        this._taskEventsServiceProxy.getAllTaskTemplateForDropdown().subscribe(result => {
            this.allTemplate = result;
        })
    }

    onTaskTemplateSelect(event: any) {
        if(event!=null && event.value!=null){
            this._taskEventsServiceProxy.getTaskEventForEdit(event.value.id).subscribe(result=>{
                this.taskEvent.name=result.taskEvent.name;
                this.taskEvent.description=result.taskEvent.description;
                this.taskEvent.priority=result.taskEvent.priority;
                this.taskEvent.eventDate=result.taskEvent.eventDate;
                this.taskEvent.endDate=result.taskEvent.endDate;
                this.taskEvent.startTime=result.taskEvent.startTime;
                this.taskEvent.endTime=result.taskEvent.endTime;
                this.taskEvent.estimatedTime=result.taskEvent.estimatedTime;
                this.taskEvent.actualTime=result.taskEvent.actualTime;
                this.taskEvent.template=false;
            });
        }
        console.log(event);
    }
}
