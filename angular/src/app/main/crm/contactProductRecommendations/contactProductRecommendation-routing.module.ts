import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactProductRecommendationsComponent} from './contactProductRecommendations.component';



const routes: Routes = [
    {
        path: '',
        component: ContactProductRecommendationsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactProductRecommendationRoutingModule {
}
