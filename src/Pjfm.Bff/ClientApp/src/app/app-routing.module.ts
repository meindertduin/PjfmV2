import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultLayoutComponent } from './core/layouts/default-layout/default-layout.component';
import { RouteData } from './shared/models/route-data';

const routes: Routes = [
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./modules/home/home.module').then((m) => m.HomeModule),
      },
      {
        path: 'session',
        loadChildren: () => import('./modules/session/session.module').then((m) => m.SessionModule),
        data: {
          isDetailPage: true,
        } as RouteData,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
