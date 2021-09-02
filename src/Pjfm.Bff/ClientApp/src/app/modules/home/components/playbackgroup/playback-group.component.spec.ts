import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaybackGroupComponent } from './playback-group.component';

describe('PlaybackgroupComponent', () => {
  let component: PlaybackGroupComponent;
  let fixture: ComponentFixture<PlaybackGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlaybackGroupComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlaybackGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
