import {
  ComponentFactoryResolver,
  ComponentRef,
  Injectable,
  InjectionToken,
  Injector,
  Type,
  ViewContainerRef,
  ViewRef,
} from '@angular/core';
import { Observable, Subject } from 'rxjs';

export const PJFM_DIALOG_DATA = new InjectionToken<unknown>('');
export const PJFM_DIALOG_REF = new InjectionToken<DialogRef>('');

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  rootViewContainer!: ViewContainerRef;
  dialogReferences: DialogData[] = [];

  constructor(private readonly _componentFactoryResolver: ComponentFactoryResolver) {}

  setRootViewContainer(viewContainer: ViewContainerRef): void {
    this.rootViewContainer = viewContainer;
  }

  openDialog<T>(component: Type<T>, dialogData: unknown): Observable<unknown> {
    const dialogRef = new DialogRef();
    const factoredComponent = this.createComponentFactory(component, dialogData, dialogRef);

    const afterClosedObservable = dialogRef.afterClosed();
    this.deleteDialogAfterClose(factoredComponent, afterClosedObservable);

    return afterClosedObservable;
  }

  private createComponentFactory<T>(component: Type<T>, dialogData: unknown, dialogRef: DialogRef) {
    const factory = this._componentFactoryResolver.resolveComponentFactory(component);
    const dialogInjector = this.createDialogInjector(dialogData, dialogRef);

    return factory.create(dialogInjector);
  }

  private createDialogInjector(dialogData: unknown, dialogRef: DialogRef) {
    return Injector.create({
      providers: [
        {
          provide: PJFM_DIALOG_DATA,
          useValue: dialogData,
        },
        {
          provide: PJFM_DIALOG_REF,
          useValue: dialogRef,
        },
      ],
      parent: this.rootViewContainer.injector,
    });
  }

  private deleteDialogAfterClose<T>(factoredComponent: ComponentRef<T>, afterClosedObservable: Observable<unknown>) {
    const viewRef = this.rootViewContainer.insert(factoredComponent.hostView);
    afterClosedObservable.subscribe(() => {
      viewRef.destroy();
    });
  }
}

export class DialogRef {
  private readonly _closed$ = new Subject<unknown>();

  closeDialog(result: unknown): void {
    this._closed$.next(result);
    this._closed$.complete();
  }

  afterClosed(): Observable<unknown> {
    return this._closed$.asObservable();
  }
}

interface DialogData {
  dialogRef: DialogRef;
  dialogViewRef: ViewRef;
}
