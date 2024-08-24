import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MonthlyExpense } from '@expense-tracker/expenses/domain';
import { YearMonthPipe } from './year-month.pipe';

@Component({
  selector: 'lib-navigation',
  standalone: true,
  imports: [RouterLink, YearMonthPipe],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss',
})
export class NavigationComponent {
  title = input<string>();
  monthlyExpenses = input<MonthlyExpense[]>();
}
