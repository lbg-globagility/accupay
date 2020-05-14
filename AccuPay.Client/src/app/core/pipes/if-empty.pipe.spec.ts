import { IfEmptyPipe } from './if-empty.pipe';

describe('IfEmptyPipe', () => {
  it('create an instance', () => {
    const pipe = new IfEmptyPipe();
    expect(pipe).toBeTruthy();
  });
});
