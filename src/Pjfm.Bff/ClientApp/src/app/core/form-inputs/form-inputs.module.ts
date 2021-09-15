import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropDownComponent } from './drop-down/drop-down.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [DropDownComponent],
  exports: [DropDownComponent],
  imports: [CommonModule, ReactiveFormsModule],
})
export class FormInputsModule {}
