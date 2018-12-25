import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToasterService, Toast } from 'angular5-toaster';

const toast: Toast = {
  animation: 'fade',
  type: '',
  title: '',
  body: '',
  mouseoverTimerStop: false,
  showCloseButton: true,
  tapToDismiss: false,
  timeout: 2000,
  newestOnTop: true,
  positionClass: 'toast-bottom-right'
};

@Injectable()
export class DisplayMessageService {

  constructor(private toasterService: ToasterService, private translate: TranslateService) {

  }

  showSuccess(messageKey) {
    toast.type = 'success';
    toast.body = this.translate.instant(messageKey);
    this.toasterService.pop(toast);
  }

  showError(messageKey) {
    toast.type = 'error';
    toast.body = this.translate.instant(messageKey);
    this.toasterService.pop(toast);
  }

  showWarning(messageKey) {
    toast.type = 'warning';
    toast.body = this.translate.instant(messageKey);
    this.toasterService.pop(toast);
  }

  showInfo(messageKey) {
    toast.type = 'info';
    toast.body = this.translate.instant(messageKey);
    this.toasterService.pop(toast);
  }


}
