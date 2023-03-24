import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeTagsComponent } from './employeeTags.component';

const routes: Routes = [
    {
        path: '',
        component: EmployeeTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeTagRoutingModule {}
