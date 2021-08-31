import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild
} from '@angular/core';

@Component({
  selector: 'pjfm-ascii-slider',
  templateUrl: './ascii-slider.component.html',
  styleUrls: ['./ascii-slider.component.scss'],
})
export class AsciiSliderComponent implements AfterViewInit, OnChanges {
  loadingBar!: string;
  @Input() percentage = 0.0;

  private componentHasLoaded = false;

  @ViewChild('container')
  container!: ElementRef;

  @HostListener('window:resize')
  onResize(): void {
    this.setLoadingBar();
  }

  ngAfterViewInit(): void {
    this.componentHasLoaded = true;
    this.setLoadingBar();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.percentage != null) {
      this.setLoadingBar();
    }
  }

  private setLoadingBar(): void {
    if (!this.componentHasLoaded) {
      return;
    }

    const charactersAmount = this.container.nativeElement.offsetWidth / 10;

    let loadingBar = '[';
    let xAmount = (charactersAmount - 2) * this.percentage;
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
  }
}
