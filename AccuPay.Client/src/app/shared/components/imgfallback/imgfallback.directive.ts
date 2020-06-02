import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: 'img[appImgFallback]',
})
export class ImgFallbackDirective {
  /**
   * Url to the fallback image if the img src is broken
   */
  private readonly fallbackUrl: string = 'assets/no-profile.jpg';

  constructor(private eRef: ElementRef) {}

  @HostListener('error')
  fallbackOnError() {
    const element = this.eRef.nativeElement as HTMLImageElement;
    element.src = this.fallbackUrl;
  }
}
