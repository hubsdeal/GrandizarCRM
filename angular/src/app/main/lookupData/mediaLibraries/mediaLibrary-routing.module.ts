import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MediaLibrariesComponent } from './mediaLibraries.component';

const routes: Routes = [
    {
        path: '',
        component: MediaLibrariesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MediaLibraryRoutingModule {}
