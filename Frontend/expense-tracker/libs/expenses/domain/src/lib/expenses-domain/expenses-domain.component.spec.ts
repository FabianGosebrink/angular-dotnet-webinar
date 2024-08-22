import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpensesDomainComponent } from './expenses-domain.component';

describe('ExpensesDomainComponent', () => {
  let component: ExpensesDomainComponent;
  let fixture: ComponentFixture<ExpensesDomainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpensesDomainComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExpensesDomainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
