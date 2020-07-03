import { PageOptions } from 'src/app/core/shared/page-options';

export class EmployeePageOptions extends PageOptions {
  searchTerm: string;

  filter: string;

  constructor(
    pageIndex: number,
    pageSize: number,
    searchTerm?: string,
    filter?: string
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

    return object;
  }
}
