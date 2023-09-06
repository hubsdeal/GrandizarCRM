import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProductTaskMapsServiceProxy, ProductTaskMapTaskEventLookupTableDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LazyLoadEvent } from 'primeng/api';
import { Paginator } from 'primeng/paginator';
import { Table } from 'primeng/table';

@Component({
  selector: 'taskEventLookupTableModal',
  templateUrl: './task-events-lookup-table-modal.component.html',
  styleUrls: ['./task-events-lookup-table-modal.component.scss']
})
export class TaskEventsLookupTableModalComponent extends AppComponentBase {
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  filterText = '';
  id: number;
  displayName: string;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;

  constructor(injector: Injector, private _productTaskMapsServiceProxy: ProductTaskMapsServiceProxy) {
    super(injector);
  }

  show(): void {
    this.active = true;
    this.paginator.rows = 5;
    this.getAll();
    this.modal.show();
  }

  getAll(event?: LazyLoadEvent) {
    if (!this.active) {
      return;
    }

    if (this.primengTableHelper.shouldResetPaging(event)) {
      this.paginator.changePage(0);
      if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
        return;
      }
    }

    this.primengTableHelper.showLoadingIndicator();

    this._productTaskMapsServiceProxy
      .getAllTaskEventForLookupTable(
        this.filterText,
        this.primengTableHelper.getSorting(this.dataTable),
        this.primengTableHelper.getSkipCount(this.paginator, event),
        this.primengTableHelper.getMaxResultCount(this.paginator, event)
      )
      .subscribe((result) => {
        this.primengTableHelper.totalRecordsCount = result.totalCount;
        this.primengTableHelper.records = result.items;
        this.primengTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }

  setAndSave(taskEvent: ProductTaskMapTaskEventLookupTableDto) {
    this.id = taskEvent.id;
    this.displayName = taskEvent.displayName;
    this.active = false;
    this.modal.hide();
    this.modalSave.emit(null);
  }

  close(): void {
    this.active = false;
    this.modal.hide();
    this.modalSave.emit(null);
  }
}