import { TestBed, waitForAsync } from '@angular/core/testing';
import { NgxPermissionsModule, NgxPermissionsService } from 'ngx-permissions';

import { PermissionConfigService } from './permission-config.service';

describe('PermissionConfigService', () => {
  let service: PermissionConfigService;
  let mockNgxPermissionsService: jasmine.SpyObj<NgxPermissionsService>;

  beforeEach(
    waitForAsync(() => {
      mockNgxPermissionsService = jasmine.createSpyObj<NgxPermissionsService>(['loadPermissions']);

      TestBed.configureTestingModule({
        imports: [NgxPermissionsModule.forRoot()],
        providers: [PermissionConfigService, { provide: NgxPermissionsService, useValue: mockNgxPermissionsService }],
      });
      service = TestBed.inject(PermissionConfigService);
    }),
  );

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
