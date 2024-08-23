import { inject, Injectable } from '@angular/core';
import { HttpService } from '@expense-tracker/shared/util-common';
import { environment } from '@expense-tracker/shared/util-environments';
import { Observable } from 'rxjs';
import { Expense } from '../models/expense';

@Injectable({
  providedIn: 'root',
})
export class ExpensesApiService {
  private readonly http = inject(HttpService);

  getExpensesForMonth(year: number, month: number): Observable<Expense[]> {
    return this.http.get<Expense[]>(
      `${environment.server}api/expenses/${year}/${month}`
    );
  }

  getExpensesForCurrentMonth(): Observable<Expense[]> {
    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;

    return this.http.get<Expense[]>(
      `${environment.server}api/expenses/${year}/${month}`
    );
  }

  getAllExpenses(): Observable<Expense[]> {
    return this.http.get<Expense[]>(`${environment.server}api/expenses/`);
  }

  addExpense(expense: Expense): Observable<Expense> {
    expense.categories = expense.categories || [];

    return this.http.post<Expense>(
      `${environment.server}api/expenses/`,
      expense
    );
  }
}
