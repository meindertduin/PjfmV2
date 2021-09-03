import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'pjfm-ascii-slider',
  templateUrl: './ascii-slider.component.html',
  styleUrls: ['./ascii-slider.component.scss'],
})
export class AsciiSliderComponent implements AfterViewInit, OnChanges {
  loadingBar = '';
  @Input() percentage = 0;

  private componentHasLoaded = false;

  @ViewChild('container')
  container!: ElementRef;

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  @HostListener('window:resize')
  onResize(): void {
    this.setLoadingBar();
    console.log(this.loadingBar.length);
    console.log(this.container.nativeElement.offsetWidth);
  }

  ngAfterViewInit(): void {
    this.componentHasLoaded = true;
    this.setLoadingBar();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.percentage != null && !changes.percentage.isFirstChange()) {
      this.setLoadingBar();
    }
  }

  private setLoadingBar(): void {
    if (!this.componentHasLoaded) {
      return;
    }

    if (this.percentage > 100) {
      this.percentage = 100;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
    const charactersAmount = Math.floor(this.container.nativeElement.offsetWidth / 9.7);

    let loadingBar = '[';
    let xAmount = (charactersAmount - 2) * (this.percentage / 100.0);
    for (let i = 1; i < charactersAmount - 1; i++) {
      if (xAmount > 0) {
        loadingBar += 'x';
        xAmount--;
      } else {
        loadingBar += ' ';
      }
    }
    loadingBar += ']';

    this.loadingBar = loadingBar;
    this.changeDetectorRef.detectChanges();
  }
}
