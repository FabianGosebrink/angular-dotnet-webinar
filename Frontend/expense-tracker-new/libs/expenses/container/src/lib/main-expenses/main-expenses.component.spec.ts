import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MainExpensesComponent } from './main-expenses.component';

describe('MainExpensesComponent', () => {
  let component: MainExpensesComponent;
  let fixture: ComponentFixture<MainExpensesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainExpensesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(MainExpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
