import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArtistsPipe } from './pipes/artists-pipe.pipe';
import { BaseBottomSheetComponent } from './components/base-bottom-sheet/base-bottom-sheet.component';

const COMPONENTS = [BaseBottomSheetComponent];
const PIPES = [ArtistsPipe];

@NgModule({
  declarations: [PIPES, COMPONENTS],
  imports: [CommonModule],
  exports: [PIPES, COMPONENTS],
})
export class SharedModule {}
