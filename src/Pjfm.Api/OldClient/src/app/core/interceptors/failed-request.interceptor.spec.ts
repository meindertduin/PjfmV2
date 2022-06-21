import { TestBed } from '@angular/core/testing';

import { FailedRequestInterceptor } from './failed-request.interceptor';

describe('FailedRequestInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      FailedRequestInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: FailedRequestInterceptor = TestBed.inject(FailedRequestInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
