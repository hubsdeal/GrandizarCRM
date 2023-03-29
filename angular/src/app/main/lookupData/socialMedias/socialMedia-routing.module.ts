import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SocialMediasComponent } from './socialMedias.component';

const routes: Routes = [
    {
        path: '',
        component: SocialMediasComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SocialMediaRoutingModule {}
