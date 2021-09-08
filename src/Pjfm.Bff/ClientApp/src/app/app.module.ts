import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultLayoutModule } from './core/layouts/default-layout/default-layout.module';
import { HttpClientModule } from '@angular/common/http';
import { PlaybackClient, PlaybackGroupClient } from './core/services/api-client.service';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, AppRoutingModule, DefaultLayoutModule, HttpClientModule],
  providers: [PlaybackClient, PlaybackGroupClient],
  bootstrap: [AppComponent],
})
export class AppModule {}
