import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreProductBlogForViewDto, StoreProductBlogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreProductBlogModal',
    templateUrl: './view-storeProductBlog-modal.component.html'
})
export class ViewStoreProductBlogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreProductBlogForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetStoreProductBlogForViewDto();
        this.item.storeProductBlog = new StoreProductBlogDto();
    }

    show(item: GetStoreProductBlogForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
