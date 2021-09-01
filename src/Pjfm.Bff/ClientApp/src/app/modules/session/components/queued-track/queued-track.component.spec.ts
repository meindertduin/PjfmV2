import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueuedTrackComponent } from './queued-track.component';

describe('QueuedTrackComponent', () => {
  let component: QueuedTrackComponent;
  let fixture: ComponentFixture<QueuedTrackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QueuedTrackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QueuedTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
