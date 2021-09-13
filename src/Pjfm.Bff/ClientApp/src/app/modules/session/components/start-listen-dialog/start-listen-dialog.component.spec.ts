import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartListenDialogComponent } from './start-listen-dialog.component';

describe('StartListenDialogComponent', () => {
  let component: StartListenDialogComponent;
  let fixture: ComponentFixture<StartListenDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StartListenDialogComponent],
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
