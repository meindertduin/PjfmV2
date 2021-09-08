import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArtistsPipePipe } from './pipes/artists-pipe.pipe';

const PIPES = [ArtistsPipePipe];

@NgModule({
  declarations: [PIPES],
  imports: [CommonModule],
  exports: [ArtistsPipePipe],
})
export class SharedModule {}
