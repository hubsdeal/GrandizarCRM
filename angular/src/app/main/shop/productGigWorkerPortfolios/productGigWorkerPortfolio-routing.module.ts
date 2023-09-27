import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductGigWorkerPortfoliosComponent} from './productGigWorkerPortfolios.component';



const routes: Routes = [
    {
        path: '',
        component: ProductGigWorkerPortfoliosComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductGigWorkerPortfolioRoutingModule {
}
