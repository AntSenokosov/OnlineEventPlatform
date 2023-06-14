import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MailtemplateComponent } from './mailtemplate.component';

describe('MailtemplateComponent', () => {
  let component: MailtemplateComponent;
  let fixture: ComponentFixture<MailtemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MailtemplateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MailtemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
