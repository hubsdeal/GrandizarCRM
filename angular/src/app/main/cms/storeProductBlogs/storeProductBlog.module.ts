import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {StoreProductBlogRoutingModule} from './storeProductBlog-routing.module';
import {StoreProductBlogsComponent} from './storeProductBlogs.component';
import {CreateOrEditStoreProductBlogModalComponent} from './create-or-edit-storeProductBlog-modal.component';
import {ViewStoreProductBlogModalComponent} from './view-storeProductBlog-modal.component';
import {StoreProductBlogContentLookupTableModalComponent} from './storeProductBlog-content-lookup-table-modal.component';
    					import {StoreProductBlogProductLookupTableModalComponent} from './storeProductBlog-product-lookup-table-modal.component';
    					import {StoreProductBlogStoreLookupTableModalComponent} from './storeProductBlog-store-lookup-table-modal.component';
    					import {StoreProductBlogHubLookupTableModalComponent} from './storeProductBlog-hub-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        StoreProductBlogsComponent,
        CreateOrEditStoreProductBlogModalComponent,
        ViewStoreProductBlogModalComponent,
        
    					StoreProductBlogContentLookupTableModalComponent,
    					StoreProductBlogProductLookupTableModalComponent,
    					StoreProductBlogStoreLookupTableModalComponent,
    					StoreProductBlogHubLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreProductBlogRoutingModule , AdminSharedModule ],
    
})
export class StoreProductBlogModule {
}
