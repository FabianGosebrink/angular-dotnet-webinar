import { Pipe, PipeTransform } from '@angular/core';
import { MonthlyExpense } from '@expense-tracker/expenses/domain';

@Pipe({
  standalone: true,
  name: 'yearMonth',
})
export class YearMonthPipe implements PipeTransform {
  transform(monthlyExpense: MonthlyExpense): string {
    const date = new Date(monthlyExpense.year, monthlyExpense.month - 1, 10);

    return date.toLocaleString('default', {
      month: 'long',
      year: 'numeric',
    });
  }
}
