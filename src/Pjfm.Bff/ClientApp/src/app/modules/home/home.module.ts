import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './features/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { PlaybackGroupComponent } from './components/playbackgroup/playback-group.component';
import { SharedModule } from '../../shared/shared.module';
import { FormInputsModule } from '../../core/form-inputs/form-inputs.module';

const COMPONENTS = [HomeComponent, PlaybackGroupComponent];

@NgModule({
  declarations: [COMPONENTS],
  imports: [CommonModule, HomeRoutingModule, SharedModule, FormInputsModule],
  providers: [],
})
export class HomeModule {}
