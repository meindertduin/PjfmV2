import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { RouteData } from './shared/models/route-data';
import { RouteDataService } from './shared/services/route-data.service';
import { ApiSocketClientService } from './core/services/api-socket-client.service';

@Component({
  selector: 'pjfm-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly _destroyed$ = new Subject();

  constructor(
    private readonly _router: Router,
    private readonly _activatedRoute: ActivatedRoute,
    private readonly _routeDataService: RouteDataService,
    private readonly _apiSocketClient: ApiSocketClientService,
  ) {}

  ngOnInit(): void {
    this._apiSocketClient.initializeConnection();
    this._router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        takeUntil(this._destroyed$),
      )
      .subscribe(() => {
        const currentRoute = this.getChild(this._activatedRoute);

        const isDetailPage = (currentRoute.snapshot.data as RouteData).isDetailPage ?? false;
        this._routeDataService.setIsDetailPage(isDetailPage);
      });
  }

  private getChild(activatedRoute: ActivatedRoute): ActivatedRoute {
    if (activatedRoute.firstChild) {
      return this.getChild(activatedRoute.firstChild);
    } else {
      return activatedRoute;
    }
  }

  ngOnDestroy(): void {
    this._destroyed$.next();
    this._destroyed$.complete();
  }
}
