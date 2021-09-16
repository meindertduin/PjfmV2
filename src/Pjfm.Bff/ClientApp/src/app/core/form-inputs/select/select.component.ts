import { Component } from '@angular/core';

@Component({
  selector: 'pjfm-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
})
export class SelectComponent {
  showOptions = false;

  onSelectClick(): void {
    this.showOptions = !this.showOptions;
  }
}
