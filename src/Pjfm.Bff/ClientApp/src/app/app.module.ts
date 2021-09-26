import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultLayoutModule } from './core/layouts/default-layout/default-layout.module';
import { HttpClientModule } from '@angular/common/http';
import { API_BASE_URL, PlaybackClient, SpotifyPlayerClient, SpotifyTrackClient, UserClient } from './core/services/api-client.service';
import { FormBuilder } from '@angular/forms';
import { UserService } from './shared/services/user.service';
import { PermissionConfigService } from './core/authorization/permission-config.service';
import { NgxPermissionsModule } from 'ngx-permissions';

export function initializeApplication(userService: UserService, permissionConfigService: PermissionConfigService) {
  return (): Promise<void> => {
    return userService.initialize().then(() => {
      const roles = userService.getUser()?.roles ?? [];
      permissionConfigService.initialize(roles);
    });
  };
}

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, DefaultLayoutModule, HttpClientModule, NgxPermissionsModule.forRoot()],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApplication,
      deps: [UserService, PermissionConfigService],
      multi: true,
    },
    PlaybackClient,
    SpotifyPlayerClient,
    SpotifyTrackClient,
    { provide: API_BASE_URL, useValue: '' },
    FormBuilder,
    UserClient,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
