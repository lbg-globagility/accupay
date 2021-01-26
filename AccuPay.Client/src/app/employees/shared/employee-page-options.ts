import { PageOptions } from 'src/app/core/shared/page-options';

export class EmployeePageOptions extends PageOptions {
  constructor(
    pageIndex: number,
    pageSize: number,
    private searchTerm?: string,
    private filter?: string,
    private positionId?: number
  ) {
    super(pageIndex, pageSize);
    this.searchTerm = searchTerm;
    this.filter = filter;
  }

  toObject(): { [param: string]: string } {
    const object = super.toObject();

    if (this.searchTerm != null) {
      object.searchTerm = this.searchTerm;
    }

    if (this.filter != null) {
      object.filter = this.filter;
    }

    if (this.positionId) {
      object.positionId = String(this.positionId);
    }

    return object;
  }
}
