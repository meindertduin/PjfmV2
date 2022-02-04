import { Component, OnInit } from '@angular/core';
import { PlaybackClient, PlaybackGroupDto } from '../../../core/services/api-client.service';

@Component({
  selector: 'pjfm-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  playbackGroups: PlaybackGroupDto[] = [];

  constructor(private readonly _playbackClient: PlaybackClient) {}

  ngOnInit(): void {
    this._playbackClient.groups().subscribe((result) => {
      this.playbackGroups = result;
    });
  }
}
