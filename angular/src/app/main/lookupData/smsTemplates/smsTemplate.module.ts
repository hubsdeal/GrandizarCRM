import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SmsTemplateRoutingModule } from './smsTemplate-routing.module';
import { SmsTemplatesComponent } from './smsTemplates.component';
import { CreateOrEditSmsTemplateModalComponent } from './create-or-edit-smsTemplate-modal.component';
import { ViewSmsTemplateModalComponent } from './view-smsTemplate-modal.component';

@NgModule({
    declarations: [SmsTemplatesComponent, CreateOrEditSmsTemplateModalComponent, ViewSmsTemplateModalComponent],
    imports: [AppSharedModule, SmsTemplateRoutingModule, AdminSharedModule],
})
export class SmsTemplateModule {}
