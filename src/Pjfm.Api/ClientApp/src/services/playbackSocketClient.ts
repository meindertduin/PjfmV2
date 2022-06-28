import {isConnected, playbackData} from "../store/playbackSocketClientStore";
import type {SpotifyTrackDto} from "./apiClient";

export interface ApiSocketRequest<T> {
    requestType: RequestType;
    body?: T;
}

export enum RequestType {
    ConnectToGroup = 0,
}

export interface ApiSocketMessage<T> {
    messageType: MessageType;
    body?: T;
}

export enum MessageType {
    playbackInfo = 0,
    connectionEstablished = 100,
}

export class PlaybackSocketClient {
    private socket!: globalThis.WebSocket;
    
    initializeConnection(): void {
        this.socket = new WebSocket('wss://' + window.location.host + '/api/playback/ws');
        
        this.socket.onmessage = (event) => {
            this.handleIncomingMessage(JSON.parse(event.data) as PlaybackMessage<unknown>);
        }
    }

    private handleIncomingMessage(message: PlaybackMessage<unknown>): void {
        const messageType = message.messageType;

        if (messageType == null) {
            return;
        }

        switch (messageType) {
            case MessageType.connectionEstablished:
                isConnected.update(() => true);
                break;
            case MessageType.playbackInfo:
                this.handlePlaybackInfoUpdate(message);
                break;
        }
    }

    private handlePlaybackInfoUpdate(message: PlaybackMessage<unknown>) {
        const typedMessage = message as PlaybackMessage<PlaybackUpdateMessageBody>;
        playbackData.update(() => typedMessage.body);
    }

    connectToGroup(groupId: string): void {
        const groupConnectionRequest: ApiSocketRequest<JoinPlaybackGroupSocketRequest> = {
            requestType: RequestType.ConnectToGroup,
            body: {
                groupId: groupId,
            },
        };

        console.log(this.socket);
        this.socket.send(JSON.stringify(groupConnectionRequest));
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
