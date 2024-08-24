import { inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core';
import { MonthlyExpense } from '@expense-tracker/expenses/domain';

@Pipe({
  standalone: true,
  name: 'yearMonth',
})
export class YearMonthPipe implements PipeTransform {
  private readonly localeId = inject<string>(LOCALE_ID);

  transform(monthlyExpense: MonthlyExpense): string {
    const date = new Date(monthlyExpense.year, monthlyExpense.month - 1, 10);

    return date.toLocaleString(this.localeId, {
      month: 'long',
      year: 'numeric',
    });
  }
}
