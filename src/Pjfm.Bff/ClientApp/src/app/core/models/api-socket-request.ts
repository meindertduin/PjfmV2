export interface ApiSocketRequest<T> {
  requestType: RequestType;
  body?: T;
}

export enum RequestType {
  ConnectToGroup = 0,
}
