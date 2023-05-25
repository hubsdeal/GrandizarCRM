import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContentsServiceProxy, CreateOrEditContentDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContentMediaLibraryLookupTableModalComponent } from './content-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditContentModal',
    templateUrl: './create-or-edit-content-modal.component.html',
})
export class CreateOrEditContentModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contentMediaLibraryLookupTableModal', { static: true })
    contentMediaLibraryLookupTableModal: ContentMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    content: CreateOrEditContentDto = new CreateOrEditContentDto();

    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _contentsServiceProxy: ContentsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contentId?: number): void {
        if (!contentId) {
            this.content = new CreateOrEditContentDto();
            this.content.id = contentId;
            this.content.publishedDate = this._dateTimeService.getStartOfDay();
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._contentsServiceProxy.getContentForEdit(contentId).subscribe((result) => {
                this.content = result.content;

                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._contentsServiceProxy
            .createOrEdit(this.content)
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

    openSelectMediaLibraryModal() {
        this.contentMediaLibraryLookupTableModal.id = this.content.bannerMediaLibraryId;
        this.contentMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.contentMediaLibraryLookupTableModal.show();
    }

    setBannerMediaLibraryIdNull() {
        this.content.bannerMediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewBannerMediaLibraryId() {
        this.content.bannerMediaLibraryId = this.contentMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.contentMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}

    startTimeValue(value: any) {
        this.content.publishTime = value;
    }
}
