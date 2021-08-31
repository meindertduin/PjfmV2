import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './features/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { PlaybackgroupComponent } from './components/playbackgroup/playbackgroup.component';

const COMPONENTS = [HomeComponent];

@NgModule({
  declarations: [COMPONENTS, PlaybackgroupComponent],
  imports: [CommonModule, HomeRoutingModule],
})
export class HomeModule {}
