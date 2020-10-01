import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { _isNumberValue } from '@angular/cdk/coercion';
import { find } from 'lodash';

interface ParseOutputModel {
  columnHeader: string;
  columnDef: string;
}

const EXEMPTED_PROPERTIES = [
  'tin',
  'sssNo',
  'pagIbigNo',
  'philHealthNo',
  'atmNo',
];

@Component({
  selector: 'app-post-import-parser-output-dialog',
  templateUrl: './post-import-parser-output-dialog.component.html',
  styleUrls: ['./post-import-parser-output-dialog.component.scss'],
})
export class PostImportParserOutputDialogComponent implements OnInit {
  columnHeaders: string[];
  validRecords: any[];
  invalidRecords: any[];
  propertyNames: string[];

  models: ParseOutputModel[];

  constructor(
    private dialog: MatDialogRef<PostImportParserOutputDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.columnHeaders = this.data.columnHeaders;
    this.validRecords = this.data.validRecords;
    this.invalidRecords = this.data.invalidRecords;
    this.propertyNames = this.data.propertyNames;
  }

  ngOnInit(): void {
    this.models = new Array<ParseOutputModel>();

    this.assignModel();
  }

  isDate(date: any) {
    if (_isNumberValue(date)) {
      return false;
    }

    return moment.utc(date).isValid();
  }

  getValue(obj: any, propertyName: string) {
    const value = obj[propertyName];

    if (
      find(EXEMPTED_PROPERTIES, (propName) => propName == propertyName) != null
    ) {
      return value;
    }

    if (propertyName.includes('Time') || propertyName.includes('time')) {
      return moment(value).toDate().toLocaleTimeString();
    }

    if (this.isDate(value)) {
      return moment(value).toDate().toLocaleDateString();
    }

    return value;
  }

  assignModel() {
    for (let i = 0; i < this.columnHeaders.length; i++) {
      this.models.push({
        columnHeader: this.columnHeaders[i],
        columnDef: this.propertyNames[i],
      });
    }
  }

  onClickOk(): void {
    this.dialog.close();
  }
}
