import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultLayoutModule } from './core/layouts/default-layout/default-layout.module';
import { HttpClientModule } from '@angular/common/http';
import { API_BASE_URL, PlaybackClient, SpotifyClient } from './core/services/api-client.service';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, DefaultLayoutModule, HttpClientModule],
  providers: [PlaybackClient, SpotifyClient, { provide: API_BASE_URL, useValue: '' }],
  bootstrap: [AppComponent],
})
export class AppModule {}
