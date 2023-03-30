import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SocialMediasServiceProxy, CreateOrEditSocialMediaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditSocialMediaModal',
    templateUrl: './create-or-edit-socialMedia-modal.component.html',
})
export class CreateOrEditSocialMediaModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    socialMedia: CreateOrEditSocialMediaDto = new CreateOrEditSocialMediaDto();

    constructor(
        injector: Injector,
        private _socialMediasServiceProxy: SocialMediasServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(socialMediaId?: number): void {
        if (!socialMediaId) {
            this.socialMedia = new CreateOrEditSocialMediaDto();
            this.socialMedia.id = socialMediaId;

            this.active = true;
            this.modal.show();
        } else {
            this._socialMediasServiceProxy.getSocialMediaForEdit(socialMediaId).subscribe((result) => {
                this.socialMedia = result.socialMedia;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._socialMediasServiceProxy
            .createOrEdit(this.socialMedia)
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
