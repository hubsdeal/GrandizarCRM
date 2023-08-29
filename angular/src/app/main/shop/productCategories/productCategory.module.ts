import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCategoryRoutingModule } from './productCategory-routing.module';
import { ProductCategoriesComponent } from './productCategories.component';
import { CreateOrEditProductCategoryModalComponent } from './create-or-edit-productCategory-modal.component';
import { ViewProductCategoryModalComponent } from './view-productCategory-modal.component';
import { ProductCategoryMediaLibraryLookupTableModalComponent } from './productCategory-mediaLibrary-lookup-table-modal.component';
import { NodeService } from './nodeservice';
import { ProductCategoryDashboardComponent } from './productCategoriesDashboard/product-category-dashboard/product-category-dashboard.component';
import { ProductCategoryTeamsComponent } from '../productCategoryTeams/productCategoryTeams.component';
import { ProductCategoryTeamModule } from '../productCategoryTeams/productCategoryTeam.module';
import { ProductCategoryVariantMapModule } from '../productCategoryVariantMaps/productCategoryVariantMap.module';

@NgModule({
    declarations: [
        ProductCategoriesComponent,
        CreateOrEditProductCategoryModalComponent,
        ViewProductCategoryModalComponent,

        ProductCategoryMediaLibraryLookupTableModalComponent,
          ProductCategoryDashboardComponent,
    ],
    imports: [AppSharedModule, ProductCategoryRoutingModule, AdminSharedModule,ProductCategoryTeamModule,ProductCategoryVariantMapModule],
    providers: [ NodeService ]
})
export class ProductCategoryModule {}
