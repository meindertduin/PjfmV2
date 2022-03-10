import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { SnackbarService } from '../../shared/services/snackbar.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class FailedRequestInterceptor implements HttpInterceptor {
  constructor(private readonly _snackBarService: SnackbarService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(() => {
        this._snackBarService.openSackBar({ message: 'An error has occured while sending a request...' });

        return throwError('An exception has occured');
      }),
    );
  }
}
