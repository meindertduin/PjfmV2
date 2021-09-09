import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseBottomSheetComponent } from './base-bottom-sheet.component';

describe('BottomSheetComponent', () => {
  let component: BaseBottomSheetComponent;
  let fixture: ComponentFixture<BaseBottomSheetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BaseBottomSheetComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseBottomSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
