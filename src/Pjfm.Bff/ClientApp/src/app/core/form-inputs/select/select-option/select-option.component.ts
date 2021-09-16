import { Component, Input } from '@angular/core';

@Component({
  selector: 'pjfm-select-option',
  templateUrl: './select-option.component.html',
  styleUrls: ['./select-option.component.scss'],
})
export class SelectOptionComponent {
  @Input() value: unknown;
}
