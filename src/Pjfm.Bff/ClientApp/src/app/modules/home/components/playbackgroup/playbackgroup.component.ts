import { Component, Input } from '@angular/core';
import { PlaybackGroupDto } from '../../../../core/services/api-client.service';

@Component({
  selector: 'pjfm-playbackgroup',
  templateUrl: './playbackgroup.component.html',
  styleUrls: ['./playbackgroup.component.scss'],
})
export class PlaybackgroupComponent {
  clickRoute = '/session/1';

  @Input() playbackGroup!: PlaybackGroupDto;
}
