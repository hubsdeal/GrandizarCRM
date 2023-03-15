import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { EmailTemplateRoutingModule } from './emailTemplate-routing.module';
import { EmailTemplatesComponent } from './emailTemplates.component';
import { CreateOrEditEmailTemplateModalComponent } from './create-or-edit-emailTemplate-modal.component';
import { ViewEmailTemplateModalComponent } from './view-emailTemplate-modal.component';

@NgModule({
    declarations: [EmailTemplatesComponent, CreateOrEditEmailTemplateModalComponent, ViewEmailTemplateModalComponent],
    imports: [AppSharedModule, EmailTemplateRoutingModule, AdminSharedModule],
})
export class EmailTemplateModule {}
