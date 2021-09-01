import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouteDataService } from '../../../shared/services/route-data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'pjfm-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent implements OnInit, OnDestroy {
  private readonly _destroyed = new Subject();

  isDetailPage = false;

  constructor(private readonly _routeDataService: RouteDataService) {}

  ngOnInit(): void {
    this._routeDataService
      .getIsDetailPage()
      .pipe(takeUntil(this._destroyed))
      .subscribe((isDetailPage) => {
        this.isDetailPage = isDetailPage;
      });
  }

  ngOnDestroy(): void {
    this._destroyed.complete();
    this._destroyed.next();
  }
}
