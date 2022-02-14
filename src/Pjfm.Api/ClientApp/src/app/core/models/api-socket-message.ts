export interface ApiSocketMessage<T> {
  messageType: MessageType;
  body?: T;
}

export enum MessageType {
  playbackInfo = 0,
  connectionEstablished = 100,
}
