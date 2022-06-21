import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[pjfmAutofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  @Input() enabled = true;

  constructor(private readonly _elementRef: ElementRef) {}

  ngAfterViewInit(): void {
    if (this.enabled) {
      (this._elementRef.nativeElement as HTMLInputElement).focus();
    }
  }
}
