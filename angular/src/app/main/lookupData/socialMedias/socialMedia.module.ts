import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { SocialMediaRoutingModule } from './socialMedia-routing.module';
import { SocialMediasComponent } from './socialMedias.component';
import { CreateOrEditSocialMediaModalComponent } from './create-or-edit-socialMedia-modal.component';
import { ViewSocialMediaModalComponent } from './view-socialMedia-modal.component';

@NgModule({
    declarations: [SocialMediasComponent, CreateOrEditSocialMediaModalComponent, ViewSocialMediaModalComponent],
    imports: [AppSharedModule, SocialMediaRoutingModule, AdminSharedModule],
})
export class SocialMediaModule {}
