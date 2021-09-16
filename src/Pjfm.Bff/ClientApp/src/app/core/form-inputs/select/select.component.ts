import { AfterViewInit, Component, ContentChildren, forwardRef, Input, OnDestroy, QueryList } from '@angular/core';
import { SelectOptionComponent } from './select-option/select-option.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'pjfm-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true,
    },
  ],
})
/* eslint-disable @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-explicit-any, @typescript-eslint/no-empty-function, @typescript-eslint/explicit-module-boundary-types */
export class SelectComponent implements AfterViewInit, OnDestroy, ControlValueAccessor {
  showOptions = false;
  textValue?: string;

  value: any;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onChange = (_: any) => {};
  onTouched = () => {};

  setValue(value: any) {
    this.value = value;
    this.onChange(this.value);
  }

  @Input() placeHolder = '';
  @ContentChildren(SelectOptionComponent) options!: QueryList<SelectOptionComponent>;

  private readonly _destroyed$ = new Subject();

  onSelectClick(): void {
    this.showOptions = !this.showOptions;
  }

  ngAfterViewInit(): void {
    this.options.toArray().forEach((option) => {
      option.clickEvent.pipe(takeUntil(this._destroyed$)).subscribe((event) => {
        this.textValue = event.textValue;
        this.showOptions = false;
        this.setValue(event.value);
      });
    });
  }

  ngOnDestroy(): void {
    this._destroyed$.complete();
    this._destroyed$.next();
  }

  registerOnChange(fn: (_: any) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  writeValue(value: any): void {
    this.setValue(value);
  }
}
