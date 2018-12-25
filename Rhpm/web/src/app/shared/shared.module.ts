import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Http } from '@angular/http';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { ChartModule, HIGHCHARTS_MODULES } from 'angular-highcharts';
import { ImageCropperModule } from 'ngx-image-cropper';
import exporting from 'highcharts/modules/exporting.src.js';

// Services
import {
    SpinnerService, RoleGuardService, AuthService, SharedService,
    HeaderComponent, FooterComponent, LeftSideBarComponent, HiringTrendChartComponent
} from './index.shared';

// Constant
import { AppConstants } from './constant/constant.variable';
import { HttpInterceptorHandler } from '@angular/common/http/src/interceptor';
import { SideBarPipe } from './pipes/sideBar.pipe';

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

export function highchartsModules() {
    return [exporting];
}

@NgModule({
    imports: [
        HttpClientModule,
        CommonModule, FormsModule, ReactiveFormsModule, RouterModule, ChartModule, ImageCropperModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
    ],
    declarations: [HeaderComponent, FooterComponent, LeftSideBarComponent, SideBarPipe, HiringTrendChartComponent],
    exports: [HeaderComponent, FooterComponent, LeftSideBarComponent, SideBarPipe, HiringTrendChartComponent, ImageCropperModule],
    providers: [{ provide: HIGHCHARTS_MODULES, useFactory: highchartsModules }]
})

export class SharedModule {
    static forRoot() {
        return {
            ngModule: SharedModule,
            providers: []
        };
    }

}
