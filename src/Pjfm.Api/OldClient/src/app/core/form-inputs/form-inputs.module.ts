import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectComponent } from './select/select.component';
import { SelectOptionComponent } from './select/select-option/select-option.component';
import { AutocompleteComponent } from './autocomplete/autocomplete.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [SelectComponent, SelectOptionComponent, AutocompleteComponent],
  exports: [SelectComponent, SelectOptionComponent, AutocompleteComponent],
  imports: [CommonModule, ReactiveFormsModule, SharedModule],
})
export class FormInputsModule {}
