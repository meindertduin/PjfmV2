import { UserRole } from '../services/api-client.service';

export interface PermissionConfig {
  [key: string]: UserRole[];
}

export const PERMISSION_CONFIG: PermissionConfig = {
  // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
  spotifyAuthenticated: [UserRole.User, UserRole.SpotifyAuth],
  mod: [UserRole.User, UserRole.Mod],
};
