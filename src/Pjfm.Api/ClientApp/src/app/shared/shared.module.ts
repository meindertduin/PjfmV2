import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArtistsPipe } from './pipes/artists-pipe.pipe';
import { BaseBottomSheetComponent } from './components/base-bottom-sheet/base-bottom-sheet.component';
import { SnackbarComponentComponent } from './components/snackbar-component/snackbar-component.component';
import { AutofocusDirective } from './directives/autofocus.directive';

const COMPONENTS = [BaseBottomSheetComponent, SnackbarComponentComponent];
const PIPES = [ArtistsPipe];

@NgModule({
  declarations: [PIPES, COMPONENTS, AutofocusDirective],
  imports: [CommonModule],
  exports: [PIPES, COMPONENTS, AutofocusDirective],
})
export class SharedModule {}
