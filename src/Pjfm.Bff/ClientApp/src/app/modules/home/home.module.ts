import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './features/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { PlaybackGroupComponent } from './components/playbackgroup/playback-group.component';
import { ArtistsPipePipe } from './pipes/artists-pipe.pipe';

const COMPONENTS = [HomeComponent, PlaybackGroupComponent];

@NgModule({
  declarations: [COMPONENTS, ArtistsPipePipe],
  imports: [CommonModule, HomeRoutingModule],
  providers: [],
})
export class HomeModule {}
