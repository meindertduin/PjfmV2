import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultLayoutComponent } from './default-layout.component';
import { RouterModule } from '@angular/router';
import { InlineSVGModule } from 'ng-inline-svg';

@NgModule({
  declarations: [DefaultLayoutComponent],
  imports: [CommonModule, RouterModule, InlineSVGModule],
})
export class DefaultLayoutModule {}
