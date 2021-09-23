import { Component, Input } from '@angular/core';
import { SpotifyTrackDto } from '../../../../core/services/api-client.service';

@Component({
  selector: 'pjfm-queued-track',
  templateUrl: './queued-track.component.html',
  styleUrls: ['./queued-track.component.scss'],
})
export class QueuedTrackComponent {
  @Input() track!: SpotifyTrackDto;
}
