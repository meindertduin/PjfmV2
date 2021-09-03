import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'pjfm-track-progression-bar',
  templateUrl: './track-progression-bar.component.html',
  styleUrls: ['./track-progression-bar.component.scss'],
})
export class TrackProgressionBarComponent implements OnInit {
  @Input() trackDurationMs!: number;
  @Input() startTimeMs = 0;

  currentTimeMs!: number;
  trackPercentage = 0;

  ngOnInit(): void {
    this.currentTimeMs = this.startTimeMs;
    this.setTimeout();
  }

  private setTimeout(): void {
    setInterval(() => {
      if (this.currentTimeMs < this.trackDurationMs) {
        this.currentTimeMs += 1000;
        this.trackPercentage = Math.round((this.currentTimeMs / this.trackDurationMs) * 100);
      }
    }, 1000);
  }
}
