import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpensesUiComponent } from './expenses-ui.component';

describe('ExpensesUiComponent', () => {
  let component: ExpensesUiComponent;
  let fixture: ComponentFixture<ExpensesUiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpensesUiComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExpensesUiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
