import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectTrackDialogComponent } from './select-track-dialog.component';

describe('SelectTrackDialogComponent', () => {
  let component: SelectTrackDialogComponent;
  let fixture: ComponentFixture<SelectTrackDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectTrackDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectTrackDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
