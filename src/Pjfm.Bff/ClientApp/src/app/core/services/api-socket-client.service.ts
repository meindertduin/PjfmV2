import { Injectable } from '@angular/core';
import { WebSocketSubject } from 'rxjs/internal-compatibility';
import { webSocket } from 'rxjs/webSocket';
import { MessageType } from '../models/api-socket-message';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ApiSocketRequest, RequestType } from '../models/api-socket-request';
import { SpotifyTrack } from './api-client.service';

@Injectable({
  providedIn: 'root',
})
export class ApiSocketClientService {
  static socket: WebSocketSubject<unknown>;
  onConnectionEstablished = new Subject();
  private readonly _playbackData = new BehaviorSubject<PlaybackUpdateMessageBody | null>(null);
  private readonly _playbackData$: Observable<PlaybackUpdateMessageBody | null> = this._playbackData.asObservable();

  initializeConnection(): void {
    // TODO: set the connection through the bff
    ApiSocketClientService.socket = webSocket('wss://localhost:5004/api/playback/ws');
    ApiSocketClientService.socket.subscribe(
      (message) => this.handleIncomingMessage(message as PlaybackMessage<unknown>),
      (error) => {
        console.log(error);
      },
      () => this.onComplete(),
    );
  }

  handleIncomingMessage(message: PlaybackMessage<unknown>): void {
    const messageType = message.messageType;

    if (messageType == null) {
      return;
    }

    switch (messageType) {
      case MessageType.connectionEstablished:
        this.onConnectionEstablished.next();
        break;
      case MessageType.playbackInfo:
        this.handlePlaybackInfoUpdate(message);
        break;
    }
  }

  private handlePlaybackInfoUpdate(message: PlaybackMessage<unknown>) {
    const typedMessage = message.body as PlaybackMessage<PlaybackUpdateMessageBody>;
    console.log(typedMessage);
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

    ApiSocketClientService.socket.next(groupConnectionRequest);
  }

  onComplete(): void {}
}

interface JoinPlaybackGroupSocketRequest {
  groupId: string;
}

interface PlaybackMessage<T> {
  messageType: MessageType;
  body: T;
}

interface PlaybackUpdateMessageBody {
  groupId: string;
  groupName: string;
  currentlyPlayingTrack: SpotifyTrack;
}
