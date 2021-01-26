import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostImportParserOutputDialogComponent } from './post-import-parser-output-dialog.component';

describe('PostImportParserOutputDialogComponent', () => {
  let component: PostImportParserOutputDialogComponent;
  let fixture: ComponentFixture<PostImportParserOutputDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostImportParserOutputDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostImportParserOutputDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
