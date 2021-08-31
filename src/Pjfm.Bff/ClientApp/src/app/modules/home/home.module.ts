import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './features/home.component';
import { HomeRoutingModule } from './home-routing.module';

const COMPONENTS = [HomeComponent];

@NgModule({
  declarations: [COMPONENTS],
  imports: [CommonModule, HomeRoutingModule],
})
export class HomeModule {}
