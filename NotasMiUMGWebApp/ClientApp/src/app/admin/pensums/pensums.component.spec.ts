import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PensumsComponent } from './pensums.component';

describe('PensumsComponent', () => {
  let component: PensumsComponent;
  let fixture: ComponentFixture<PensumsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PensumsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PensumsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
