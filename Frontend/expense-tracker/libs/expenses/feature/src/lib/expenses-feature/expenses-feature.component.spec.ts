import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExpensesFeatureComponent } from './expenses-feature.component';

describe('ExpensesFeatureComponent', () => {
  let component: ExpensesFeatureComponent;
  let fixture: ComponentFixture<ExpensesFeatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpensesFeatureComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExpensesFeatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
