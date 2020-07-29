import { PageOptions } from 'src/app/core/shared/page-options';

export class LoanPageOptions extends PageOptions {
  constructor(
    pageIndex: number,
    pageSize: number,
    private searchTerm?: string,
    private loanTypeId?: number,
    private status?: string
  ) {
    super(pageIndex, pageSize);
  }

  toObject(): { [param: string]: string } {
    const object = super.toObject();

    if (this.searchTerm) {
      object.searchTerm = this.searchTerm;
    }

    if (this.loanTypeId) {
      object.loanTypeId = String(this.loanTypeId);
    }

    if (this.status) {
      object.status = this.status;
    }

    return object;
  }
}
