import { IfZeroPipe } from './if-zero.pipe';

describe('IfZeroPipe', () => {
  it('create an instance', () => {
    const pipe = new IfZeroPipe();
    expect(pipe).toBeTruthy();
  });
});
