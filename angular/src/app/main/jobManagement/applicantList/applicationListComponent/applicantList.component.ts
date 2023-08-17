import {Component, Injector, ViewEncapsulation} from '@angular/core';

import {appModuleAnimation} from '@shared/animations/routerTransition';
import {AppComponentBase} from '@shared/common/app-component-base';

@Component({
    templateUrl: './applicantList.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class ApplicantListComponent extends AppComponentBase {


    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    filterText = '';
    advancedFiltersAreShown = false;


    createApplicant() {

    }

    getContacts() {

    }

    resetFilters() {

    }
}
