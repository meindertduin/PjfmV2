import { Component, Input, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'pjfm-select-option',
  templateUrl: './select-option.component.html',
  styleUrls: ['./select-option.component.scss'],
})
export class SelectOptionComponent implements AfterViewInit {
  @Input() value: unknown;
  clickEvent = new EventEmitter<SelectClickEvent>();

  private textValue!: string;

  constructor(private readonly _elementRef: ElementRef) {}

  onClick(): void {
    this.clickEvent.emit({
      value: this.value,
      textValue: this.textValue,
    });
  }

  ngAfterViewInit(): void {
    const innerText = this._elementRef.nativeElement.innerText as string | undefined;
    if (innerText == null) {
      this.textValue = '';
    } else {
      this.textValue = innerText;
    }
  }
}

export interface SelectClickEvent {
  value: unknown;
  textValue: string;
}
