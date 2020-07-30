export interface ImportParserOutput<T> {
  validRecords: T[];
  invalidRecords: T[];
  columnHeaders: string[];
}
