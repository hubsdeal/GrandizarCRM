import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DiscountCodeGeneratorRoutingModule } from './discountCodeGenerator-routing.module';
import { DiscountCodeGeneratorsComponent } from './discountCodeGenerators.component';
import { CreateOrEditDiscountCodeGeneratorModalComponent } from './create-or-edit-discountCodeGenerator-modal.component';
import { ViewDiscountCodeGeneratorModalComponent } from './view-discountCodeGenerator-modal.component';

@NgModule({
    declarations: [
        DiscountCodeGeneratorsComponent,
        CreateOrEditDiscountCodeGeneratorModalComponent,
        ViewDiscountCodeGeneratorModalComponent,
    ],
    imports: [AppSharedModule, DiscountCodeGeneratorRoutingModule, AdminSharedModule],
})
export class DiscountCodeGeneratorModule {}
