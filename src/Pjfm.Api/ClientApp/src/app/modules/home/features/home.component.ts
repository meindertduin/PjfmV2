import { Component, OnInit } from '@angular/core';
import { PlaybackClient, PlaybackGroupDto } from '../../../core/services/api-client.service';
import { SnackbarService } from '../../../shared/services/snackbar.service';

@Component({
  selector: 'pjfm-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  playbackGroups: PlaybackGroupDto[] = [];

  constructor(private readonly _playbackClient: PlaybackClient, private readonly _snackBarService: SnackbarService) {}

  ngOnInit(): void {
    this._playbackClient.groups().subscribe((result) => {
      this.playbackGroups = result;
    });

    this._snackBarService.openSackBar({ message: 'This works' });
  }
}
