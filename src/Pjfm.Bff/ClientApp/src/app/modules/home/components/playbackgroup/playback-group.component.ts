import { Component, Input } from '@angular/core';
import { PlaybackGroupDto } from '../../../../core/services/api-client.service';

@Component({
  selector: 'pjfm-playback-group',
  templateUrl: './playback-group.component.html',
  styleUrls: ['./playback-group.component.scss'],
})
export class PlaybackGroupComponent {
  @Input() playbackGroup!: PlaybackGroupDto;
}
