import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { RatingLikesServiceProxy, CreateOrEditRatingLikeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditRatingLikeModal',
    templateUrl: './create-or-edit-ratingLike-modal.component.html',
})
export class CreateOrEditRatingLikeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    ratingLike: CreateOrEditRatingLikeDto = new CreateOrEditRatingLikeDto();

    constructor(
        injector: Injector,
        private _ratingLikesServiceProxy: RatingLikesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(ratingLikeId?: number): void {
        if (!ratingLikeId) {
            this.ratingLike = new CreateOrEditRatingLikeDto();
            this.ratingLike.id = ratingLikeId;

            this.active = true;
            this.modal.show();
        } else {
            this._ratingLikesServiceProxy.getRatingLikeForEdit(ratingLikeId).subscribe((result) => {
                this.ratingLike = result.ratingLike;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._ratingLikesServiceProxy
            .createOrEdit(this.ratingLike)
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
