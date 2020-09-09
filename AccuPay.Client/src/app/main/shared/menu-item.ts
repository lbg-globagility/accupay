export interface MenuItem {
  label: string;
  route?: string;
  icon?: string;
  items?: MenuItem[];
  toggled?: boolean;
  permission?: string;
}
