import { Component, OnInit } from '@angular/core';
import { QualificationsServiceApp } from './shared/qualifications.serviceApp';
import { QualificationViewModel } from '../../webapi/models/qualification-view-model';
import { DisplayMessageService } from '../../shared/toastr/display.message.service';
import { SearchAndSortModel } from '../../webapi/models/search-and-sort-model';
import { TranslateService } from '@ngx-translate/core';
import { isNullOrUndefined } from 'util';
import { Status } from '../../app.enum';

@Component({
  selector: 'app-qualifications',
  templateUrl: 'qualifications.component.html'
})

export class QualificationsComponent implements OnInit {
  qualificationsModel: QualificationViewModel = {} as QualificationViewModel;
  qualifications: QualificationViewModel[] = [] as QualificationViewModel[];
  searchAndSortModel: SearchAndSortModel = {} as SearchAndSortModel;
  submitted = false;
  isQualificationExists: boolean = false;
  listFilter: string;
  qualificationId: any;
  isDesc = false;


  constructor(private qualificationsServiceApp: QualificationsServiceApp, private displayMessage: DisplayMessageService,
    private translateService: TranslateService) {
  }
  ngOnInit() {
    this.listQualification();
  }

  onSubmit(qualificationsform) {
    this.submitted = true;
    if (qualificationsform.valid) {
      if (this.qualificationsModel['qualificationId'] === undefined) {
        this.qualificationsServiceApp.addQualification(this.qualificationsModel).subscribe(
          (data) => {
            if (data.status === 0) {
            } else {
            }
            this.listQualification();
          }
        );
      } else {
        this.qualificationsServiceApp.updateQualification(this.qualificationsModel).subscribe(
          (data) => {
            this.listQualification();
          }
        );
      }
    }
  }
  checkQualificationExits() {
    let $this = this;
    let qualificationname = $this.qualificationsModel.name;
    this.qualifications.every(function (qualification) {
      if (qualification['name'] === qualificationname) {
        $this.isQualificationExists = true;
        return false;
      } else {
        $this.isQualificationExists = false;
        return true;
      }
    });
  }
  listQualification() {
    this.qualificationsServiceApp.getAllQualification()
      .subscribe((data) => {
        if (data) {
          this.qualifications = data.body;
        }
      });
  }

  deleteQualifications(qualification, i) {
    let isDelete = confirm("Are you sure you want to delete Qualification?");
    if (isDelete) {
      this.qualifications.splice(i, 1);
      this.qualificationsServiceApp.deleteQualification(qualification).subscribe(
        (data) => {
          if (data.status === Status.Success) {
            this.qualificationsModel = data.body;
            this.displayMessage.showSuccess('QUALIFICATIONS.DELETEDSUCCESSFULLY');
          }
          else if (data.status === Status.Fail) {
            this.displayMessage.showWarning('QUALIFICATIONS.FAILEDTODELETE');
          }
          else if (data.status === Status.Error) {
            this.displayMessage.showError('QUALIFICATIONS.DELETEERROR');
          }
        },
        (error) => {
          this.displayMessage.showError('QUALIFICATIONS.DELETEERROR');
        });
    }
  }

  getQualificationById(qualificationId) {
    if (!isNullOrUndefined(qualificationId)) {
      this.qualificationsServiceApp.getQualificationById(qualificationId).subscribe(
        (data) => {
          if (data.status === Status.Success) {
            this.qualificationsModel = data.body;
          }
        });
    }
  }
  editQualifications(qualificationId) {
    this.qualificationsModel.qualificationId = qualificationId;
    this.getQualificationById(qualificationId);
  }


  setDefaultSortOption() {
    this.searchAndSortModel.direction = -1;
    this.translateService.get('QUALIFICATIONS.DEFAULTSORTPROPERTY').subscribe(
      (data) => {
        this.searchAndSortModel.property = data;
        this.getAllQualifications();
      }
    );
  }

  getAllQualifications() {
    this.getQualificationResults();

  }
  sort(property) {
    this.isDesc = !this.isDesc;
    this.searchAndSortModel.direction = this.isDesc ? 1 : -1;
    this.searchAndSortModel.property = property;
    this.getQualificationResults();
  }

  clear() {
    if (this.listFilter === "") {
      this.search()
    }
    return;
  }
  onKeydown(event) {
    if (event.key === 'Enter') {
      this.search();
    }
  }
  search() {
    this.searchAndSortModel.searchString = this.listFilter.trim();
    this.getQualificationResults();
  }

  getQualificationResults() {
    this.qualificationsServiceApp.GetQualificationsResults(this.searchAndSortModel).subscribe(
      (data) => {
        if (data.status === Status.Success) {
          this.qualifications = data.body;
        }
        else if (data.status === Status.Error) {
          this.displayMessage.showError('QUALIFICATIONS.ERROR');
        }
      },
      (error) => {
        this.displayMessage.showError('QUALIFICATIONS.ERROR');
      }
    );
  }

}


