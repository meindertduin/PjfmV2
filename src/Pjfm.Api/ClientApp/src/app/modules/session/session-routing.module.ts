import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { SessionComponent } from './features/session.component';

const routes: Routes = [
  {
    path: '',
  },
  {
    path: ':id',
    component: SessionComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SessionRoutingModule {}
