import { UserRole } from '../services/api-client.service';

export interface PermissionConfig {
  [key: string]: UserRole[];
}

export const PERMISSION_CONFIG: PermissionConfig = {};
