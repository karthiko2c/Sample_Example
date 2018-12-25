import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { SpinnerService } from '../spinner/spinner.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  token: String;
  constructor(private spinnerService: SpinnerService, private injector: Injector) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.getToken('auth_token');
    const clonedRequest = req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) });

    this.spinnerService.startRequest();
    return next
      .handle(clonedRequest)
      .catch(response => {
        if (response instanceof HttpErrorResponse) {
          this.handleHTTPStatus(response.status, response.error);
        }
        return Observable.throw(response);
      }).finally(() => {
        this.spinnerService.endRequest();
      });
  }

  private handleHTTPStatus(status: number, error: any) {
    if (status === 500 && error) {
      // this.alert.showResponseDescription(error, null, 'error');
    }
    // if status code 400 show bad request message for the user.
    if (status === 400 && error) {
      // this.alert.showBadRequestError(error);
    }
    if ((status === 401 || error.status === 403) && (window.location.href.match(/\?/g) || []).length < 2) {
      // this.router.navigate(['login']);
    }
  }

  getToken(key: string) {
    return localStorage.getItem(key);
  }
}
