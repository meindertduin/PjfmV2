import { Component, Inject } from '@angular/core';
import { DialogRef, PJFM_DIALOG_REF } from '../../../../shared/services/dialog.service';
import { SpotifyTrackClient } from '../../../../core/services/api-client.service';
import { AutoCompleteValue } from '../../../../core/form-inputs/autocomplete/autocomplete.component';

@Component({
  selector: 'pjfm-select-track-dialog',
  templateUrl: './select-track-dialog.component.html',
  styleUrls: ['./select-track-dialog.component.scss'],
})
export class SelectTrackDialogComponent {
  autoCompleteValues: AutoCompleteValue[] = [];

  constructor(@Inject(PJFM_DIALOG_REF) private readonly _dialogRef: DialogRef, private readonly _spotifyTrackClient: SpotifyTrackClient) {}

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
}
