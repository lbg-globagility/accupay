export interface Role {
  id: string;
  name: string;
  isAdmin: boolean;
  rolePermissions: {
    permissionId: number;
    permissionName: string;
    read: boolean;
    create: boolean;
    update: boolean;
    delete: boolean;
  }[];
}
