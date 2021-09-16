import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropDownComponent } from './drop-down/drop-down.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectComponent } from './select/select.component';
import { SelectOptionComponent } from './select/select-option/select-option.component';

@NgModule({
  declarations: [DropDownComponent, SelectComponent, SelectOptionComponent],
  exports: [DropDownComponent, SelectComponent, SelectOptionComponent],
  imports: [CommonModule, ReactiveFormsModule],
})
export class FormInputsModule {}
