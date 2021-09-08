import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';

@Component({
  selector: 'pjfm-track-progression-bar',
  templateUrl: './track-progression-bar.component.html',
  styleUrls: ['./track-progression-bar.component.scss'],
})
export class TrackProgressionBarComponent implements OnInit, OnChanges {
  @Input() trackDurationMs!: number;
  @Input() startTimeMs = 0;

  private _currentInterval!: any;

  currentTimeMs!: number;
  trackPercentage = 0;

  ngOnInit(): void {
    this.reset();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.trackDurationMs && !changes.trackDurationMs.isFirstChange()) {
      this.reset();
    }
  }

  private reset(): void {
    this.currentTimeMs = this.startTimeMs;
    console.log(this.currentTimeMs);

    if (this._currentInterval != null) {
      clearInterval(this._currentInterval);
    }

    this.setTimeout();
  }

  private setTimeout(): void {
    this._currentInterval = setInterval(() => {
      if (this.currentTimeMs < this.trackDurationMs) {
        this.currentTimeMs += 1000;
        this.trackPercentage = Math.round((this.currentTimeMs / this.trackDurationMs) * 100);
      }
    }, 1000);
  }
}
