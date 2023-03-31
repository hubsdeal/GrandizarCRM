import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DiscountCodeUserHistoriesComponent } from './discountCodeUserHistories.component';

const routes: Routes = [
    {
        path: '',
        component: DiscountCodeUserHistoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DiscountCodeUserHistoryRoutingModule {}
