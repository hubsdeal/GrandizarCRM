import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductGigWorkerPortfolioForViewDto, ProductGigWorkerPortfolioDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductGigWorkerPortfolioModal',
    templateUrl: './view-productGigWorkerPortfolio-modal.component.html'
})
export class ViewProductGigWorkerPortfolioModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductGigWorkerPortfolioForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetProductGigWorkerPortfolioForViewDto();
        this.item.productGigWorkerPortfolio = new ProductGigWorkerPortfolioDto();
    }

    show(item: GetProductGigWorkerPortfolioForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
