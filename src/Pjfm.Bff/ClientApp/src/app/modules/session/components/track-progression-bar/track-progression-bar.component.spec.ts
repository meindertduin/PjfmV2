import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackProgressionBarComponent } from './track-progression-bar.component';

describe('TrackProgressionBarComponent', () => {
  let component: TrackProgressionBarComponent;
  let fixture: ComponentFixture<TrackProgressionBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrackProgressionBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrackProgressionBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
