import { Injectable } from '@angular/core';
import { WebSocketSubject } from 'rxjs/internal-compatibility';
import { webSocket } from 'rxjs/webSocket';
import { MessageType } from '../models/api-socket-message';
import { Subject } from 'rxjs';
import { ApiSocketRequest, RequestType } from '../models/api-socket-request';

@Injectable({
  providedIn: 'root',
})
export class ApiSocketClientService {
  static socket: WebSocketSubject<unknown>;
  onConnectionEstablished = new Subject();

  initializeConnection(): void {
    // TODO: set the connection through the bff
    ApiSocketClientService.socket = webSocket('wss://localhost:5004/api/playback/ws');
    ApiSocketClientService.socket.subscribe(
      (message) => this.handleIncomingMessage(message),
      (error) => {
        console.log(error);
      },
      () => this.onComplete(),
    );
  }

  handleIncomingMessage(message: any): void {
    const messageType = message.messageType as MessageType;

    if (messageType == null) {
      return;
    }

    switch (messageType) {
      case MessageType.connectionEstablished:
        this.onConnectionEstablished.next();
    }
  }

  connectToGroup(groupId: string): void {
    const groupConnectionRequest: ApiSocketRequest<JoinPlaybackGroupRequest> = {
      requestType: RequestType.ConnectToGroup,
      body: {
        groupId: groupId,
      },
    };

    ApiSocketClientService.socket.next(groupConnectionRequest);
  }

  onComplete(): void {}
}

interface JoinPlaybackGroupRequest {
  groupId: string;
}
