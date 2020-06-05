export interface Role {
  id: string;
  name: string;
  rolePermissions: {
    permissionId: number;
    permissionName: string;
    read: boolean;
    create: boolean;
    update: boolean;
    delete: boolean;
  }[];
}
