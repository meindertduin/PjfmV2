import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionComponent } from './session.component';
import { ActivatedRoute } from '@angular/router';
import { PlaybackService } from '../../../shared/services/playback.service';
import { PlaybackClient, UserClient } from '../../../core/services/api-client.service';
import { HttpClientModule } from '@angular/common/http';
import { of } from 'rxjs';

describe('SessionComponent', () => {
  let component: SessionComponent;
  let fixture: ComponentFixture<SessionComponent>;

  beforeEach(async () => {
    const mockPlaybackService = jasmine.createSpyObj<PlaybackService>(['getPlaybackIsActive']);
    mockPlaybackService.getPlaybackIsActive.and.returnValue(of(false));
    await TestBed.configureTestingModule({
      declarations: [SessionComponent],
      imports: [HttpClientModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { params: { id: '1' } },
          },
        },
        {
          provide: PlaybackService,
          useValue: mockPlaybackService,
        },
        PlaybackClient,
        UserClient,
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SessionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
