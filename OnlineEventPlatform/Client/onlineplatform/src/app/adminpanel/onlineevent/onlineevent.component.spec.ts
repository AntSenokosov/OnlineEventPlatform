import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnlineeventComponent } from './onlineevent.component';

describe('OnlineeventComponent', () => {
  let component: OnlineeventComponent;
  let fixture: ComponentFixture<OnlineeventComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OnlineeventComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OnlineeventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
