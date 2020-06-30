export interface CreateClient {
  name: string;
  tradeName: string;
  address: string;
  phoneNumber: string;
  contactPerson: string;
  user: {
    email: string;
    firstName: string;
    lastName: string;
  };
}
