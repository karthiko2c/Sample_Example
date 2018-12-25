import { NgModule } from '@angular/core';
import {
    MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule,
    MatTableModule
} from '@angular/material';

@NgModule({
    imports: [MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatInputModule,
        MatSelectModule, MatOptionModule, MatTableModule],
    exports: [MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatInputModule,
        MatSelectModule, MatOptionModule, MatTableModule],
})
export class MaterialModule { }
