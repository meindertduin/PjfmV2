import { Component, HostListener, Inject } from '@angular/core';
import { DialogRef, PJFM_DIALOG_DATA, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';
import { PlaybackClient, PlaybackTrackRequest, SpotifyTrackClient } from '../../../../core/services/api-client.service';
import { AutoCompleteValue } from '../../../../core/form-inputs/autocomplete/autocomplete.component';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'pjfm-select-track-dialog',
  templateUrl: './select-track-dialog.component.html',
  styleUrls: ['./select-track-dialog.component.scss'],
})
export class SelectTrackDialogComponent {
  autoCompleteValues: AutoCompleteValue[] = [];
  selectedTracks: SelectedTrack[] = [];

  readonly selectTrackLimit = 3;

  private _isRequesting = false;

  constructor(
    @Inject(PJFM_DIALOG_DATA) readonly dialogData: SelectTrackDialogData,
    @Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef,
    private readonly _spotifyTrackClient: SpotifyTrackClient,
    private readonly _playbackClient: PlaybackClient,
  ) {}

  closeDialog(): void {
    this._dialogRef.closeDialog(undefined);
  }

  onQueryChange(query: string): void {
    this._spotifyTrackClient.search(query).subscribe((result) => {
      this.autoCompleteValues = result.tracks.map<AutoCompleteValue>((s) => {
        return {
          value: s.spotifyTrackId,
          text: `${s.title} - ${s.artists.join(', ')}`,
        };
      });
    });
  }

  onSearchValueSelect(value: unknown): void {
    if (this.selectedTracks.length + this.dialogData.userRequestedAmount < this.selectTrackLimit) {
      const trackId = value as string;
      const autoCompleteValue = this.autoCompleteValues.find((s) => s.value === trackId);
      this.selectedTracks.push({ trackId: trackId, text: autoCompleteValue?.text.split('-')[0].trim() ?? '' });
    }
  }

  removeSelectedTrack(selectedTrack: SelectedTrack): void {
    this.selectedTracks = this.selectedTracks.filter((s) => s.trackId !== selectedTrack.trackId);
  }

  confirmClicked(): void {
    if (this._isRequesting || this.selectedTracks.length <= 0) {
      return;
    }
    this._isRequesting = true;

    const trackIds = this.selectedTracks.map((s) => s.trackId);
    this._playbackClient
      .trackRequest({ trackIds: trackIds } as PlaybackTrackRequest)
      .pipe(
        finalize(() => {
          this._isRequesting = false;
        }),
      )
      .subscribe(() => {
        this.closeDialog();
      });
  }

  @HostListener('document:keydown', ['$event'])
  onKeypress(event: KeyboardEvent): void {
    switch (event.key) {
      case 'Escape':
        this.closeDialog();
        break;
      case 'Enter':
        this.confirmClicked();
        break;
      default:
        break;
    }
  }
}

export interface SelectedTrack {
  text: string;
  trackId: string;
}

export interface SelectTrackDialogData {
  userRequestedAmount: number;
}
