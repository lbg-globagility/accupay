const DEFAULT_PAGE_INDEX = 0;
const DEFAULT_PAGE_SIZE = 20;

export class PageOptions {
  pageIndex: number;
  pageSize: number;
  sort: string;
  direction: string;

  constructor(
    pageIndex: number = DEFAULT_PAGE_INDEX,
    pageSize: number = DEFAULT_PAGE_SIZE,
    sort: string = '',
    direction: string = ''
  ) {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.sort = sort;
    this.direction = direction;
  }

  /**
   * @description Convert to object literal to ready for serialization
   */
  toObject(): { [param: string]: string } {
    return {
      pageIndex: String(this.pageIndex || DEFAULT_PAGE_INDEX),
      pageSize: String(this.pageSize || DEFAULT_PAGE_SIZE),
      sort: this.sort || '',
      direction: this.direction || ''
    };
  }
}
