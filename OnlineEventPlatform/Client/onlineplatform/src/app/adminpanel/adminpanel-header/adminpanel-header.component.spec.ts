import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminpanelHeaderComponent } from './adminpanel-header.component';

describe('AdminpanelHeaderComponent', () => {
  let component: AdminpanelHeaderComponent;
  let fixture: ComponentFixture<AdminpanelHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminpanelHeaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminpanelHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
