import { Directive, EventEmitter, ElementRef, Output, HostListener } from '@angular/core';

@Directive({
  selector: '[pjfmClickOutside]',
})
export class ClickOutsideDirective {
  private _clickCount = 0;
  @Output() pjfmClickOutside = new EventEmitter();
  constructor(private readonly _elementRef: ElementRef) {}

  @HostListener('document:click', ['$event.target'])
  // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
  onClick(target: any) {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-call,@typescript-eslint/no-unsafe-assignment,@typescript-eslint/no-unsafe-member-access
    const clickInside: boolean = this._elementRef.nativeElement.contains(target);
    if (!clickInside) {
      // One click outside is permitted for dialogs, as buttons are clicked outside the direcftive component to activate the dialog
      if (this._clickCount > 0) {
        this.pjfmClickOutside.emit();
      }
      this._clickCount++;
    }
  }
}
