import { HasParenthesisPipe } from './has-parenthesis.pipe';

describe('HasParenthesisPipe', () => {
  it('create an instance', () => {
    const pipe = new HasParenthesisPipe();
    expect(pipe).toBeTruthy();
  });
});
