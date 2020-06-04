export interface Role {
  id: string;
  name: string;
  rolePermissions: {
    permissionId: number;
    read: boolean;
    create: boolean;
    update: boolean;
    delete: boolean;
  }[];
}
