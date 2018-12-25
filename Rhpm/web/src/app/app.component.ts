import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from './shared/constant/constant.variable';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(translate: TranslateService) {
    translate.addLangs(['nb', 'en']);
    translate.setDefaultLang('en');
    translate.use('en');
  }

  ngOnInit() {
    this.isTokenExist();
  }

  isTokenExist() {
    const token = localStorage.getItem(AppConstants.AuthToken);
    if (!isNullOrUndefined(token)) {
      return true;
    }
    return false;
  }
}

