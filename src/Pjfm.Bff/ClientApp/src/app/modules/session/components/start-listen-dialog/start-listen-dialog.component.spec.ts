import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartListenDialogComponent } from './start-listen-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { PlaybackClient, SpotifyClient } from '../../../../core/services/api-client.service';
import { HttpClientModule } from '@angular/common/http';

describe('StartListenDialogComponent', () => {
  let component: StartListenDialogComponent;
  let fixture: ComponentFixture<StartListenDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StartListenDialogComponent],
      imports: [ReactiveFormsModule, HttpClientModule],
      providers: [SpotifyClient, PlaybackClient],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StartListenDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
