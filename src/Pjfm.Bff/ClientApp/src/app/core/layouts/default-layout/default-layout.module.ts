import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultLayoutComponent } from './default-layout.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [DefaultLayoutComponent],
  imports: [CommonModule, RouterModule],
})
export class DefaultLayoutModule {}
