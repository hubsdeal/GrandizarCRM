import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DiscountCodeGeneratorsComponent } from './discountCodeGenerators.component';

const routes: Routes = [
    {
        path: '',
        component: DiscountCodeGeneratorsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DiscountCodeGeneratorRoutingModule {}
