import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConnectChannelsComponent } from './connectChannels.component';

const routes: Routes = [
    {
        path: '',
        component: ConnectChannelsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConnectChannelRoutingModule {}
