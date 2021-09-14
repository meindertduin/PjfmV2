import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackProgressionBarComponent } from './track-progression-bar.component';
import { ConvertMsToTimePipe } from '../../pipes/convert-ms-to-time.pipe';

describe('TrackProgressionBarComponent', () => {
  let component: TrackProgressionBarComponent;
  let fixture: ComponentFixture<TrackProgressionBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TrackProgressionBarComponent, ConvertMsToTimePipe],
      imports: [],
    }).compileComponents();
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
