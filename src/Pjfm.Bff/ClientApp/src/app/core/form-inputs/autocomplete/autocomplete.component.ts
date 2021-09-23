import { Component, OnDestroy, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ControlValueAccessor, FormControl } from '@angular/forms';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'pjfm-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss'],
})

/* eslint-disable @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-explicit-any, @typescript-eslint/no-empty-function, @typescript-eslint/explicit-module-boundary-types */
export class AutocompleteComponent implements OnInit, OnDestroy, ControlValueAccessor {
  @Input() autoCompleteValues: AutoCompleteValue[] = [{ text: 'this is a test', value: 'x' }];
  @Output() queryChanges = new EventEmitter<string>();

  query = new FormControl('');
  value: any;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onChange = (_: any) => {};
  onTouched = () => {};

  private readonly _destroyed$ = new Subject();

  ngOnInit(): void {
    this.setupOnQueryChanges();
  }

  private setupOnQueryChanges() {
    this.query.valueChanges.pipe(debounceTime(500), takeUntil(this._destroyed$)).subscribe((value) => {
      if (value != null) {
        const typedValue = value as string;
        if (typedValue.trim().length > 2) {
          this.queryChanges.next(typedValue);
        }
      }
    });
  }

  ngOnDestroy(): void {
    this._destroyed$.complete();
    this._destroyed$.next();
  }

  onAutoCompleteValueClick(autoCompleteValue: AutoCompleteValue): void {
    this.setValue(autoCompleteValue.value);
    this.query.setValue(autoCompleteValue.text, { emitEvent: false });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  writeValue(value: any): void {
    this.setValue(value);
  }

  setValue(value: any) {
    this.value = value;
    this.onChange(this.value);
  }
}

export interface AutoCompleteValue {
  text: string;
  value: unknown;
}
