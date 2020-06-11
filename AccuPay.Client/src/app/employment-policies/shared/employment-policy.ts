export interface EmploymentPolicy {
  id: number;
  name: string;
  items: {
    type: string;
    value: any;
  }[];
}
