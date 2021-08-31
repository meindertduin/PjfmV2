import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaybackgroupComponent } from './playbackgroup.component';

describe('PlaybackgroupComponent', () => {
  let component: PlaybackgroupComponent;
  let fixture: ComponentFixture<PlaybackgroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlaybackgroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlaybackgroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
