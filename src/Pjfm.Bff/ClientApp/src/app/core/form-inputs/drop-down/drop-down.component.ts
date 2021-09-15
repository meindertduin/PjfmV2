import { Component, Input } from '@angular/core';

@Component({
  selector: 'pjfm-drop-down',
  templateUrl: './drop-down.component.html',
  styleUrls: ['./drop-down.component.scss'],
})
export class DropDownComponent {
  @Input() binding!: string;
}
