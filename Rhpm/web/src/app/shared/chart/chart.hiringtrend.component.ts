import { Component, OnInit, ViewChild, ElementRef, AfterContentInit, OnDestroy, OnChanges, Input } from '@angular/core';
import { chart } from 'highcharts';
import { ChartModel } from '../customModels/chart-view-model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-hiringtrendchart',
  templateUrl: './chart.hiringtrend.component.html'
})

export class HiringTrendChartComponent implements OnInit, AfterContentInit, OnChanges, OnDestroy {

  @Input() chartViewModel: ChartModel = {} as ChartModel;
  @ViewChild('chartTarget') chartTarget: ElementRef;
  chart: Highcharts.ChartObject;
  year: any;

  constructor(private translate: TranslateService) { }

  ngOnInit() {
    this.year = (new Date()).getFullYear();
  }

  ngAfterContentInit() {
    this.createChart();
  }

  ngOnChanges() {
    this.createChart();
  }

  createChart() {
    const options: Highcharts.Options = {
      chart: {
        type: this.chartViewModel.chartType
      },
      title: {
        text: this.translate.instant('CHARTBOARD.TITLE')
      },
      subtitle: {
        text: this.translate.instant('CHARTBOARD.SUBTITLE')
      },
      xAxis: {
        categories: this.chartViewModel.xCategories,
        crosshair: true
      },
      yAxis: {
        min: 0,
        title: {
          text: this.translate.instant('CHARTBOARD.YTITLE')
        }
      },
      credits: {
        text: this.translate.instant('CHARTBOARD.CREDITS')
      },
      exporting: {
        enabled: true,
      },
      tooltip: {
        headerFormat: '<span style="font-size:12px">{point.key} ' + this.year + '</span><table>',
        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
          '<td style="padding:0"><b>{point.y:f}</b></td></tr>',
        footerFormat: '</table>',
        shared: true,
        useHTML: true
      },
      plotOptions: {
        column: {
          pointPadding: 0.2,
          borderWidth: 0
        }
      },
      series: this.chartViewModel.series
    };
    this.chart = chart(this.chartTarget.nativeElement, options);
  }

  ngOnDestroy() {
    this.chart = null;
  }
}
