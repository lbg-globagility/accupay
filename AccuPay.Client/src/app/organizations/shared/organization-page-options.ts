import { PageOptions } from 'src/app/core/shared/page-options';

export class OrganizationPageOptions extends PageOptions {
  clientId: number;

  constructor(pageIndex?: number, pageSize?: number, clientId?: number) {
    super(pageIndex, pageSize);
    this.clientId = clientId;
  }

  toObject(): { [param: string]: string } {
    const object = super.toObject();
    if (this.clientId) {
      object.clientId = `${this.clientId}`;
    }

    return object;
  }
}
