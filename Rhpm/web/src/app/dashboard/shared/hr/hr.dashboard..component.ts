import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApprovalServiceApp } from '../../../approval/shared/approval.serviceApp';
import { Status, ShowType } from '../../../app.enum';
import { DisplayMessageService } from '../../../shared/toastr/display.message.service';
import { DashboardModel } from '../../../shared/customModels/dashboard-view-model';
import { ChartModel } from '../../../shared/customModels/chart-view-model';

@Component({
  selector: 'app-hr',
  templateUrl: './hr.dashboard.component.html',
  styleUrls: ['hr.dashboard.scss']
})

export class HrDashboardComponent implements OnInit {

  dashboardModel: DashboardModel = {} as DashboardModel;
  chartViewModel: ChartModel = {} as ChartModel;
  showType = ShowType;
  exShowType: any;
  selectedType: any;

  constructor(private router: Router, private approvalService: ApprovalServiceApp,
    private msgService: DisplayMessageService) {
  }

  ngOnInit() {
    this.chartViewModel.type = this.showType.All;
    this.selectedType = this.showType.All;
    this.exShowType = this.chartViewModel.type;
    this.getDashboardDetails();
    this.getChartDetails(this.chartViewModel.type);
  }

  setChartDetails() {
    this.chartViewModel.chartType = 'column';
    this.chartViewModel.xCategories = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];
  }

  getDashboardDetails() {
    this.approvalService.getDashboardDetails().subscribe(
      (data) => {
        if (data.status === Status.Success) {
          this.dashboardModel = data.body;
        } else {
          this.msgService.showError('Error');
        }
      }
    );
  }

  getChartDetails(showType) {
    this.approvalService.getChartDetails(showType).subscribe(
      (data) => {
        if (data.status === Status.Success) {
          this.chartViewModel = data.body;
          this.setChartDetails();
        } else {
          this.msgService.showError('Error');
        }
      }
    );
  }

  showData(showType) {
    if (this.exShowType !== showType) {
      this.selectedType = showType;
      this.getChartDetails(showType);
      this.exShowType = showType;
    }
  }

}
