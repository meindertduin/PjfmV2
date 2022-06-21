import { ComponentFactoryResolver, ComponentRef, Injectable, InjectionToken, Injector, ViewContainerRef } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { SnackbarComponentComponent, SnackBarData } from '../components/snackbar-component/snackbar-component.component';

export const PJFM_SNACKBAR_REF = new InjectionToken<SnackBarRef>('');
export const PJFM_SNACKBAR_DATA = new InjectionToken<SnackBarData>('');

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  rootViewContainer!: ViewContainerRef;

  constructor(private readonly _componentFactoryResolver: ComponentFactoryResolver) {}

  setRootViewContainer(viewContainer: ViewContainerRef): void {
    this.rootViewContainer = viewContainer;
  }

  openSackBar(dialogData: SnackBarData): Observable<void> {
    const snackBarRef = new SnackBarRef();
    const factory = this._componentFactoryResolver.resolveComponentFactory(SnackbarComponentComponent);

    const dialogInjector = Injector.create({
      providers: [
        {
          provide: PJFM_SNACKBAR_DATA,
          useValue: dialogData,
        },
        {
          provide: PJFM_SNACKBAR_REF,
          useValue: snackBarRef,
        },
      ],
      parent: this.rootViewContainer.injector,
    });

    const factoredComponent = factory.create(dialogInjector);

    const afterClosedObservable = snackBarRef.afterClosed();
    this.deleteDialogAfterClose(factoredComponent, afterClosedObservable);

    return afterClosedObservable;
  }

  private deleteDialogAfterClose(factoredComponent: ComponentRef<SnackbarComponentComponent>, afterClosedObservable: Observable<void>) {
    const viewRef = this.rootViewContainer.insert(factoredComponent.hostView);
    afterClosedObservable.subscribe(() => {
      viewRef.destroy();
    });
  }
}

export class SnackBarRef {
  private readonly _closed$ = new Subject<void>();

  closeSnackBar(): void {
    this._closed$.next();
    this._closed$.complete();
  }

  afterClosed(): Observable<void> {
    return this._closed$.asObservable();
  }
}
