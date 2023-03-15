import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SmsTemplatesComponent } from './smsTemplates.component';

const routes: Routes = [
    {
        path: '',
        component: SmsTemplatesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SmsTemplateRoutingModule {}
