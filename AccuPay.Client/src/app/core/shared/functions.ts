export function isEmpty(value: string) {
  return value == null || value.length <= 0;
}

export function isInfo(status: number): boolean {
  return 1 <= status && status < 100;
}

export function isOk(status: number): boolean {
  return 200 <= status && status < 300;
}

export function isRedirect(status: number): boolean {
  return 300 <= status && status < 400;
}

export function isClientError(status: number): boolean {
  return 400 <= status && status < 500;
}

export function isServerError(status: number): boolean {
  return 500 <= status && status < 600;
}
