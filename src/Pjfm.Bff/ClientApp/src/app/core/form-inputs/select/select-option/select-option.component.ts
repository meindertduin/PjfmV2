import { Component, Input, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'pjfm-select-option',
  templateUrl: './select-option.component.html',
  styleUrls: ['./select-option.component.scss'],
})
export class SelectOptionComponent implements AfterViewInit {
  @Input() value: unknown;
  clickEvent = new EventEmitter<SelectOptionClickEvent>();

  private textValue!: string;

  constructor(private readonly _elementRef: ElementRef) {}

  onClick(): void {
    this.clickEvent.emit({
      value: this.value,
      textValue: this.textValue,
    });
  }

  ngAfterViewInit(): void {
    this.setInnerText();
  }

  private setInnerText() {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
    const innerText = this._elementRef.nativeElement.innerText as string | undefined;
    if (innerText == null) {
      this.textValue = '';
    } else {
      this.textValue = innerText;
    }
  }
}

export interface SelectOptionClickEvent {
  value: unknown;
  textValue: string;
}
