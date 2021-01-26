export interface Organization {
  id: number;
  name: string;
  type: string;
  address1: string;
  address2: string;
  city: string;
  state: string;
  image: string;
  zip: string;
  status: string;
  domains: string[];
  acknowledgmentThresholds: Threshold[];
  inProgressThresholds: Threshold[];
}

export interface Threshold {
  type: string;
  classification: string;
  first: number;
  second: number;
}
