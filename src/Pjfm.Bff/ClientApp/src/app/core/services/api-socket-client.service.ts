import { Injectable } from '@angular/core';
import { WebSocketSubject } from 'rxjs/internal-compatibility';
import { webSocket } from 'rxjs/webSocket';
import { MessageType } from '../models/api-socket-message';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiSocketRequest, RequestType } from '../models/api-socket-request';
import { SpotifyTrackDto, TrackTerm } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class ApiSocketClientService {
  private socket!: WebSocketSubject<unknown>;

  private readonly _isConnected = new BehaviorSubject<boolean>(false);
  private readonly _isConnected$ = this._isConnected.asObservable();

  private readonly _playbackData = new BehaviorSubject<PlaybackUpdateMessageBody | null>(null);
  private readonly _playbackData$: Observable<PlaybackUpdateMessageBody | null> = this._playbackData.asObservable();

  initializeConnection(): void {
    // TODO: set the connection through the bff
    this.socket = webSocket('wss://localhost:5004/api/playback/ws');
    this.socket.subscribe(
      (message) => {
        this.handleIncomingMessage(message as PlaybackMessage<unknown>);
      },
      () => {
        // TODO: add a way of logging application errors
      },
      () => {
        // Todo: add onComplete
      },
    );
  }

  getIsConnected(): Observable<boolean> {
    return this._isConnected$;
  }

  private handleIncomingMessage(message: PlaybackMessage<unknown>): void {
    const messageType = message.messageType;

    if (messageType == null) {
      return;
    }

    switch (messageType) {
      case MessageType.connectionEstablished:
        this._isConnected.next(true);
        break;
      case MessageType.playbackInfo:
        this.handlePlaybackInfoUpdate(message);
        break;
    }
  }

  private handlePlaybackInfoUpdate(message: PlaybackMessage<unknown>) {
    const typedMessage = message as PlaybackMessage<PlaybackUpdateMessageBody>;
    this._playbackData.next(typedMessage.body);
  }

  getPlaybackData(): Observable<PlaybackUpdateMessageBody | null> {
    return this._playbackData$;
  }

  connectToGroup(groupId: string): void {
    const groupConnectionRequest: ApiSocketRequest<JoinPlaybackGroupSocketRequest> = {
      requestType: RequestType.ConnectToGroup,
      body: {
        groupId: groupId,
      },
    };

    this.socket.next(groupConnectionRequest);
  }
}

interface JoinPlaybackGroupSocketRequest {
  groupId: string;
}

interface PlaybackMessage<T> {
  messageType: MessageType;
  body: T;
}

export interface PlaybackUpdateMessageBody {
  groupId: string;
  groupName: string;
  currentlyPlayingTrack: SpotifyTrackDto;
  queuedTracks: SpotifyTrackDto[];
}
