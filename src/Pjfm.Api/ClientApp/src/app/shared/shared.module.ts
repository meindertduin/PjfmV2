import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArtistsPipe } from './pipes/artists-pipe.pipe';
import { BaseBottomSheetComponent } from './components/base-bottom-sheet/base-bottom-sheet.component';
import { SnackbarComponentComponent } from './components/snackbar-component/snackbar-component.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { ClickOutsideDirective } from './directives/click-outside.directive';

const COMPONENTS = [BaseBottomSheetComponent, SnackbarComponentComponent];
const PIPES = [ArtistsPipe];

@NgModule({
  declarations: [PIPES, COMPONENTS, AutofocusDirective, ClickOutsideDirective],
  imports: [CommonModule],
  exports: [PIPES, COMPONENTS, AutofocusDirective],
})
export class SharedModule {}
