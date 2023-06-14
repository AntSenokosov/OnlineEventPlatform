import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsereventsComponent } from './userevents.component';

describe('UsereventsComponent', () => {
  let component: UsereventsComponent;
  let fixture: ComponentFixture<UsereventsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsereventsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsereventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
