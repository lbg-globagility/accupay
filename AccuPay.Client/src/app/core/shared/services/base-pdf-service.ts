import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

@Injectable({
  providedIn: 'root',
})
export class BasePdfService {
  constructor(protected httpClient: HttpClient) {}

  protected getPDF(pdfFileName: string, url: string): Promise<any> {
    return new Promise((resolve, reject) => {
      this.httpClient
        .get(url, {
          responseType: 'blob',
        })
        .subscribe(
          (blob) => {
            saveAs(blob, pdfFileName);
            resolve();
          },
          (error) => {
            console.log('Error downloading the file.');
            reject(error);
          }
        );
    });
  }
}
