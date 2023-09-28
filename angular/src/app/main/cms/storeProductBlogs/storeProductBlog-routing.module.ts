import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {StoreProductBlogsComponent} from './storeProductBlogs.component';



const routes: Routes = [
    {
        path: '',
        component: StoreProductBlogsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreProductBlogRoutingModule {
}
