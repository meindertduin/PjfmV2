import { AfterViewInit, Component, ContentChildren, Input, OnDestroy, QueryList } from '@angular/core';
import { SelectOptionComponent } from './select-option/select-option.component';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'pjfm-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
})
export class SelectComponent implements AfterViewInit, OnDestroy {
  showOptions = false;
  textValue?: string;

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
      });
    });
  }

  ngOnDestroy(): void {
    this._destroyed$.complete();
    this._destroyed$.next();
  }
}
