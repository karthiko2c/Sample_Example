import { Component, OnInit } from '@angular/core';
import { UtilityService } from '../utility/utility.service';


@Component({
    selector: 'app-leftsidebar',
    templateUrl: './leftSideBar.component.html',
    styleUrls: ['leftSideBar.scss']
})

export class LeftSideBarComponent implements OnInit {

    isToggle: boolean;
    listOfBar: any[];

    constructor(private utilitySevice: UtilityService) { }

    ngOnInit(): void {
        this.isToggle = true;
        this.initializeMethod();
    }

    initializeMethod() {
        this.listOfBar = this.utilitySevice.getLeftSideBar();
    }

}

