import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { SideBarModel } from '../customModels/side-bar-model';

@Pipe({
    name: 'sort'
})

export class SideBarPipe implements PipeTransform {
    transform(arr: SideBarModel[], args: any): SideBarModel[] {
        return arr.sort((a, b) => {
            if (a.order > b.order) {
                return 1;
            }
            if (a.order < b.order) {
                return -1;
            }
            return 0;
        });
    }
}
