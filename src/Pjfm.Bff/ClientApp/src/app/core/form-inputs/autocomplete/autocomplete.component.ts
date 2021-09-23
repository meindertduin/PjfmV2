import { Component, OnDestroy, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'pjfm-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss'],
})
export class AutocompleteComponent implements OnInit, OnDestroy {
  @Input() autoCompleteValues: AutoCompleteValue[] = [{ text: 'this is a test', value: 'x' }];
  @Output() queryChanges = new EventEmitter<string>();

  query = new FormControl('');

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
}

export interface AutoCompleteValue {
  text: string;
  value: unknown;
}
