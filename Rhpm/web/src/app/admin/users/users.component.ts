import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserViewModel } from '../../webapi/models/user-view-model';
import { UserServiceApp } from './shared/user.serviceApp';
import { debuglog, isNullOrUndefined } from 'util';
import { Status } from '../../app.enum';
import { SearchAndSortModel } from '../../webapi/models/search-and-sort-model';
import { DisplayMessageService } from '../../shared/toastr/display.message.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-users',
  templateUrl: 'users.component.html',
  styleUrls: ['shared/user.scss']
})

export class UsersComponent implements OnInit {

  users: UserViewModel[] = [] as UserViewModel[];
  searchAndSortModel: SearchAndSortModel = {} as SearchAndSortModel;
  displayedColumns = ['firstName', 'lastName', 'email', 'role', 'actions'];
  listFilter: string;
  isDesc = false;

  constructor(private userService: UserServiceApp, private router: Router, private displayMessage: DisplayMessageService,
    private translateService: TranslateService) {
  }

  ngOnInit() {
    this.setDefaultSortOption();
  }

  getAllUsers() {
    this.getUserResults();
  }

  addUser() {
    this.router.navigate(['User']);
  }

  updateUser(user) {
    this.router.navigate(['User', user.userId]);
  }

  deleteUser(user) {

  }

  setDefaultSortOption() {
    this.searchAndSortModel.direction = -1;
    this.translateService.get('USER.DEFAULTSORTPROPERTY').subscribe(
      (data) => {
        this.searchAndSortModel.property = data;
        this.getAllUsers();
      }
    );
  }
  sort(property) {
    this.isDesc = !this.isDesc;
    this.searchAndSortModel.direction = this.isDesc ? 1 : -1;
    this.searchAndSortModel.property = property;
    this.getUserResults();
  }

  clear() {
    if (this.listFilter === '') {
      this.search();
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
    this.getUserResults();
  }

  getUserResults() {
    this.userService.GetUsersResults(this.searchAndSortModel).subscribe(
      (data) => {
        if (data.status === Status.Success) {
          this.users = data.body;
        } else if (data.status === Status.Error) {
          this.displayMessage.showError('USER.ERROR');
        }
      },
      (error) => {
        this.displayMessage.showError('USER.ERROR');
      }
    );
  }
}
