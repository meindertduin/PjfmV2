import { Directive, EventEmitter, ElementRef, Output, HostListener } from '@angular/core';

@Directive({
  selector: '[pjfmClickOutside]',
})
export class ClickOutsideDirective {
  @Output() pjfmClickOutside = new EventEmitter();
  constructor(private readonly _elementRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
  onClick(event: MouseEvent) {
    const targetElement = this._elementRef.nativeElement as HTMLElement;
    const width = targetElement.offsetWidth;
    const height = targetElement.offsetHeight;

    const divX = this.findPosX(targetElement);
    const divY = this.findPosY(targetElement);

    const clickX = Math.abs(event.x - divX) * 2;
    const clickY = event.y - divY;

    const clickInside: boolean = clickX >= 0 && clickX <= width && clickY >= 0 && clickY <= height;
    if (!clickInside) {
      this.pjfmClickOutside.emit();
    }
  }

  /* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/explicit-module-boundary-types, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-explicit-any */
  findPosX(obj: any): number {
    let curleft = 0;
    if (obj.offsetParent) {
      while (obj.offsetParent) {
        curleft += obj.offsetLeft;
        obj = obj.offsetParent;
      }
    } else if (obj.x) {
      curleft += obj.x;
    }
    return curleft;
  }

  findPosY(obj: any): number {
    let curtop = 0;
    if (obj.offsetParent) {
      while (obj.offsetParent) {
        curtop += obj.offsetTop;
        obj = obj.offsetParent;
      }
    } else if (obj.y) {
      curtop += obj.y;
    }

    return curtop;
  }

  /* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/explicit-module-boundary-types, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-explicit-any  */
}
