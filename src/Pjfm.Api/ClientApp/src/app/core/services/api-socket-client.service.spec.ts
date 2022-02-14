import { TestBed } from '@angular/core/testing';

import { ApiSocketClientService } from './api-socket-client.service';

describe('ApiSocketClientService', () => {
  let service: ApiSocketClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApiSocketClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
