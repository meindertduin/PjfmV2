import {
  AfterContentInit,
  AfterViewInit,
  Component,
  ContentChildren,
  ElementRef,
  forwardRef,
  HostListener,
  Input,
  OnDestroy,
  QueryList,
  ViewChild,
} from '@angular/core';
import { SelectOptionClickEvent, SelectOptionComponent } from './select-option/select-option.component';
import { Subject, Subscription } from 'rxjs';
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
export class SelectComponent implements OnDestroy, ControlValueAccessor, AfterContentInit, AfterViewInit {
  showOptions = false;
  textValue?: string;
  optionsWidth = 0;
  @Input() default?: any;

  value: any;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onChange = (_: any) => {};
  onTouched = () => {};

  private clickSubscriptions: Subscription[] = [];
  private _componentViewInitialized = false;

  setValue(value: any) {
    this.value = value;
    this.onChange(this.value);
  }

  @Input() placeHolder = '';
  @ContentChildren(SelectOptionComponent) options!: QueryList<SelectOptionComponent>;
  @ViewChild('select') select!: ElementRef;

  @HostListener('window:resize')
  onResize(): void {
    if (this._componentViewInitialized) {
      this.setOptionsWidth();
    }
  }

  private readonly _destroyed$ = new Subject();

  onSelectClick(): void {
    this.showOptions = !this.showOptions;
  }

  ngAfterViewInit(): void {
    this._componentViewInitialized = true;
    this.setOptionsWidth();
  }

  private setOptionsWidth(): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
    this.optionsWidth = this.select.nativeElement.offsetWidth;
  }

  ngAfterContentInit(): void {
    this.setOptionsOnClickEvent();
  }

  private setOptionsOnClickEvent() {
    this.options.changes.pipe(takeUntil(this._destroyed$)).subscribe(() => {
      this.unsubscribeFromCurrentClickEvents();
      this.subscribeToUpdatedContentClickEvents();
    });
  }

  private unsubscribeFromCurrentClickEvents() {
    this.clickSubscriptions.forEach((subscription) => {
      subscription.unsubscribe();
    });
  }

  private subscribeToUpdatedContentClickEvents() {
    this.options.toArray().forEach((option) => {
      const subscription = option.clickEvent.pipe(takeUntil(this._destroyed$)).subscribe((event) => {
        this.onOptionClick(event);
      });
      this.clickSubscriptions.push(subscription);
    });
  }

  private onOptionClick(event: SelectOptionClickEvent): void {
    this.textValue = event.textValue;
    this.showOptions = false;
    this.setValue(event.value);
  }

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
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
