export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  title: string;
  role: string;
  organizationName: string;
  phoneNumber: string;
  userName?: string;
  supplierId?: number;
  providerId?: number;
  mobilePhone?: string;
  image?: string;
  imageLocation?: string;
  status?: string;
  recallCategories?: string[];
  location: string;
  employeeId?: number;
}
