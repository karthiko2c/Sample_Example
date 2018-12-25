import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class SharedService {

    public userDetail = new EventEmitter<any>();

    updateUserDetail(user) {
        this.userDetail.emit(user);
    }

}
