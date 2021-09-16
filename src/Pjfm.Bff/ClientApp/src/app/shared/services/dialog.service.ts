import { ComponentFactoryResolver, Injectable, Type, ViewContainerRef } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  rootViewContainer!: ViewContainerRef;

  constructor(private readonly _componentFactoryResolver: ComponentFactoryResolver) {}

  setRootViewContainer(viewContainer: ViewContainerRef): void {
    this.rootViewContainer = viewContainer;
  }

  openDialog<T>(component: Type<T>): void {
    this.addDynamicDialogComponent(component);
  }

  private addDynamicDialogComponent<T>(component: Type<T>): void {
    const factory = this._componentFactoryResolver.resolveComponentFactory(component);
    const factoredComponent = factory.create(this.rootViewContainer.injector);
    this.rootViewContainer.insert(factoredComponent.hostView);
  }
}
