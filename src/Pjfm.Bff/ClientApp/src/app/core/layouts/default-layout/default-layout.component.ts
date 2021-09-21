import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouteDataService } from '../../../shared/services/route-data.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from '../../../shared/services/user.service';

@Component({
  selector: 'pjfm-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent implements OnInit, OnDestroy {
  private readonly _destroyed = new Subject();

  isDetailPage = false;
  userAuthenticated?: boolean;

  constructor(private readonly _routeDataService: RouteDataService, private readonly _userService: UserService) {}

  ngOnInit(): void {
    this._routeDataService
      .getIsDetailPage()
      .pipe(takeUntil(this._destroyed))
      .subscribe((isDetailPage) => {
        this.isDetailPage = isDetailPage;
      });

    this.userAuthenticated = this._userService.isAuthenticated();
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }
}
