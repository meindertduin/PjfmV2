import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './features/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { PlaybackgroupComponent } from './components/playbackgroup/playbackgroup.component';
import { ArtistsPipePipe } from './pipes/artists-pipe.pipe';

const COMPONENTS = [HomeComponent];

@NgModule({
  declarations: [COMPONENTS, PlaybackgroupComponent, ArtistsPipePipe],
  imports: [CommonModule, HomeRoutingModule],
  providers: [],
})
export class HomeModule {}
