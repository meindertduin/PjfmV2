import { TestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { UserClient } from '../../core/services/api-client.service';
import { HttpClientModule } from '@angular/common/http';

describe('UserService', () => {
  let service: UserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserClient],
      imports: [HttpClientModule],
    });
    service = TestBed.inject(UserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
