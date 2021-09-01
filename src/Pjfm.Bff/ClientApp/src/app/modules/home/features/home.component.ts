import { Component, OnInit } from '@angular/core';
import { pjfmclient } from '../../../core/services/api-client.service';
import PlaybackGroupDto = pjfmclient.PlaybackGroupDto;
import PlaybackClient = pjfmclient.PlaybackClient;

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
      this.playbackGroups = result as PlaybackGroupDto[];
    });
  }
}
