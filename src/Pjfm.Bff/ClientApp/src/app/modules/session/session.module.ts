import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionRoutingModule } from './session-routing.module';
import { SessionComponent } from './features/session.component';

@NgModule({
  declarations: [SessionComponent],
  imports: [CommonModule, SessionRoutingModule],
})
export class SessionModule {}
