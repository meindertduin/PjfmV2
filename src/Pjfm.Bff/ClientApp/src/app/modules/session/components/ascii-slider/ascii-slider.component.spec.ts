import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsciiSliderComponent } from './ascii-slider.component';

describe('AsciiSliderComponent', () => {
  let component: AsciiSliderComponent;
  let fixture: ComponentFixture<AsciiSliderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsciiSliderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsciiSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
