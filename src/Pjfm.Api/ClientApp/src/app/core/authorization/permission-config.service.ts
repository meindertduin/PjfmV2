import { Injectable } from '@angular/core';
import { NgxPermissionsService } from 'ngx-permissions';
import { UserRole } from '../services/api-client.service';
import { PERMISSION_CONFIG } from './PERMISSION_CONFIG';

@Injectable({
  providedIn: 'root',
})
export class PermissionConfigService {
  constructor(private readonly _permissionsService: NgxPermissionsService) {}

  initialize(roles: UserRole[]): void {
    const rolePermissions = [];
    for (const permission in PERMISSION_CONFIG) {
      if (PERMISSION_CONFIG[permission].some((x) => roles.includes(x))) {
        rolePermissions.push(permission);
      }
    }

    this._permissionsService.loadPermissions(rolePermissions);
  }
}
