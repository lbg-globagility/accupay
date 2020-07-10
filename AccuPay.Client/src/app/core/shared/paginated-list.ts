export class PaginatedList<T> {
  public pageNumber: number;
  public totalPages: number;
  public items: T[];
  public totalCount: number;
}
