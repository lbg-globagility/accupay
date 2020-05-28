enum LoadingStateType {
  Nothing,
  Loading,
  Error,
  Success
}

export class LoadingState {
  state: LoadingStateType;

  constructor() {
    this.state = LoadingStateType.Nothing;
  }

  get isNothing(): boolean {
    return this.state === LoadingStateType.Nothing;
  }

  get isLoading(): boolean {
    return this.state === LoadingStateType.Loading;
  }

  get isSuccess(): boolean {
    return this.state === LoadingStateType.Success;
  }

  get isError(): boolean {
    return this.state === LoadingStateType.Error;
  }

  changeToNothing(): void {
    this.changeState(LoadingStateType.Nothing);
  }

  changeToSuccess(): void {
    this.changeState(LoadingStateType.Success);
  }

  changeToLoading(): void {
    this.changeState(LoadingStateType.Loading);
  }

  changeToError(): void {
    this.changeState(LoadingStateType.Error);
  }

  private changeState(state: LoadingStateType): void {
    this.state = state;
  }
}
