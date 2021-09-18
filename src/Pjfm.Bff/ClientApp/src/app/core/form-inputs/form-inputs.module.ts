import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectComponent } from './select/select.component';
import { SelectOptionComponent } from './select/select-option/select-option.component';

@NgModule({
  declarations: [SelectComponent, SelectOptionComponent],
  exports: [SelectComponent, SelectOptionComponent],
  imports: [CommonModule, ReactiveFormsModule],
})
export class FormInputsModule {}
