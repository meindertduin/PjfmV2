import {
  Component,
  OnDestroy,
  OnInit,
  Output,
  EventEmitter,
  Input,
  HostListener,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { ControlValueAccessor, FormControl } from '@angular/forms';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { generateRandomString } from '../../utils/random-string';

@Component({
  selector: 'pjfm-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss'],
})

/* eslint-disable @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-explicit-any, @typescript-eslint/no-empty-function, @typescript-eslint/explicit-module-boundary-types */
export class AutocompleteComponent implements OnInit, OnDestroy, ControlValueAccessor, AfterViewInit, OnChanges {
  @Input() autoCompleteValues: AutoCompleteValue[] = [];
  @Input() inputId = generateRandomString(10);
  @Input() placeHolder = '';
  @Input() autoFocus = false;
  @Output() queryChanges = new EventEmitter<string>();
  @Output() valueSelect = new EventEmitter<unknown>();

  @ViewChild('input') input!: ElementRef;

  cursorSelectedAutocompleteIndex = 0;
  query = new FormControl('');
  showAutoCompleteValues = false;
  value: any;
  autoCompleteWidth = 0;
  showAutoCompleteOnTop = false;

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onChange = (_: any) => {};
  onTouched = () => {};

  private _componentViewInitialized = false;

  @HostListener('window:resize')
  onResize(): void {
    if (this._componentViewInitialized) {
      this.setAutoCompletePositionAndWidth();
    }
  }

  @ViewChild('input') inputComponent!: ElementRef;

  private readonly _destroyed$ = new Subject();

  get queryValue() {
    return this.query.value as string;
  }

  ngOnInit(): void {
    this.setupOnQueryChanges();
  }

  ngAfterViewInit(): void {
    this.setAutoCompletePositionAndWidth();
    this._componentViewInitialized = true;
  }

  private setupOnQueryChanges() {
    this.query.valueChanges.pipe(debounceTime(500), takeUntil(this._destroyed$)).subscribe((value) => {
      if (value != null) {
        this.queryChanges.next(this.queryValue);
      }
    });
  }

  ngOnDestroy(): void {
    this._destroyed$.complete();
    this._destroyed$.next();
  }

  selectAutocompleteValue(autoCompleteValue: AutoCompleteValue | null): void {
    if (autoCompleteValue == null) {
      return;
    }

    this.setValue(autoCompleteValue.value);
    this.query.setValue('', { emitEvent: false });
    this.queryChanges.next(this.queryValue);
    this.showAutoCompleteValues = false;
    this.valueSelect.next(autoCompleteValue.value);
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

  onInputFocusChange(focusIn: boolean) {
    this.showAutoCompleteValues = focusIn;
  }

  private setAutoCompletePositionAndWidth(): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access,@typescript-eslint/no-unsafe-call
    const { y } = this.inputComponent.nativeElement.getBoundingClientRect() as { x: number; y: number };
    const spaceToTop = Math.floor(y);
    const spaceToBottom = window.innerHeight - spaceToTop;
    if (spaceToBottom < 150 && spaceToTop > spaceToBottom) {
      this.showAutoCompleteOnTop = true;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
    this.autoCompleteWidth = this.inputComponent.nativeElement.offsetWidth;
  }

  cursorSelectValue(index: number): void {
    this.cursorSelectedAutocompleteIndex = index;
  }

  @HostListener('document:keydown', ['$event'])
  onKeypress(event: KeyboardEvent): void {
    switch (event.key) {
      case 'ArrowUp':
        event.preventDefault();
        if (this.cursorSelectedAutocompleteIndex > 0) {
          this.cursorSelectedAutocompleteIndex--;
        }
        break;
      case 'ArrowDown':
        event.preventDefault();
        if (this.cursorSelectedAutocompleteIndex < this.autoCompleteValues.length - 1) {
          this.cursorSelectedAutocompleteIndex++;
        }
        break;
      case 'Enter':
        if (this.queryValue.length > 0) {
          this.selectAutocompleteValue(this.autoCompleteValues[this.cursorSelectedAutocompleteIndex]);
          this.showAutoCompleteValues = true;
        }
        break;
      default:
        break;
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.autoCompleteValues && !changes.autoCompleteValues.firstChange) {
      this.cursorSelectedAutocompleteIndex = 0;
    }
  }
}

export interface AutoCompleteValue {
  text: string;
  value: unknown;
}
