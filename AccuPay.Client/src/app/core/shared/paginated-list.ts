export interface PaginatedList<T> {
  pageNumber: number;
  totalPages: number;
  items: T[];
  totalCount: number;
}
