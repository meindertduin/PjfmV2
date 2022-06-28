import {writable} from "svelte/store";
import type {Writable} from "svelte/store";
import type {SpotifyTrackDto} from "../services/apiClient";

export let isConnected: Writable<boolean> = writable(false);
export let playbackData: Writable<PlaybackUpdateMessageBody | null> = writable(null);

let ws: WebSocket;

export function initializeWebsocket() {
    ws = new WebSocket('wss://' + 'localhost:5004' + '/api/playback/ws');

    ws.onopen = (event) => {
        console.log(event);
    }

    ws.onmessage = (message) => {
        const playbackMessage = JSON.parse(message.data) as PlaybackMessage<unknown>;

        const messageType = playbackMessage.messageType;

        if (messageType == null) {
            return;
        }

        switch (messageType) {
            case MessageType.connectionEstablished:
                isConnected.update(() => true);
                break;
            case MessageType.playbackInfo:
                console.log(playbackMessage.body);
                break;
        }
    }
}

export function connectToGroup(groupId: string): void {
    const groupConnectionRequest: ApiSocketRequest<JoinPlaybackGroupSocketRequest> = {
        requestType: RequestType.ConnectToGroup,
        body: {
            groupId: groupId,
        },
    };

    ws.send(JSON.stringify(groupConnectionRequest));
    console.log('connecting...');
}


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
