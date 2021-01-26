import { PageOptions } from 'src/app/core/shared/page-options';

export class TimeLogsByEmployeePageOptions extends PageOptions {
  constructor(
    pageIndex: number,
    pageSize: number,
    private dateFrom: Date,
    private dateTo: Date,
    private searchTerm: string,
    private status: string
  ) {
    super(pageIndex, pageSize);
  }

  toObject(): { [param: string]: string } {
    const object = super.toObject();
    if (this.dateFrom != null) {
      object.dateFrom = this.dateFrom.toISOString();
    }
    if (this.dateTo != null) {
      object.dateTo = this.dateTo.toISOString();
    }
    if (this.searchTerm != null) {
      object.searchTerm = this.searchTerm;
    }
    if (this.status != null) {
      object.status = this.status;
    }

    return object;
  }
}
