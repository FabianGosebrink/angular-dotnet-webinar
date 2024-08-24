import { inject, Injectable } from '@angular/core';
import { HttpService } from '@expense-tracker/shared/util-common';
import { environment } from '@expense-tracker/shared/util-environments';
import { Observable } from 'rxjs';
import { ExpensesModel, MonthlyExpense } from '../models/expenses.models';

@Injectable({
  providedIn: 'root',
})
export class ExpensesApiService {
  private readonly http = inject(HttpService);

  getExpensesForMonth(
    year?: number,
    month?: number
  ): Observable<ExpensesModel[]> {
    const date = new Date();
    const yearToSend = year ?? date.getFullYear();
    const monthToSend = month ?? date.getMonth() + 1;

    return this.http.get<ExpensesModel[]>(
      `${environment.server}api/expenses/${yearToSend}/${monthToSend}`
    );
  }

  getAllExpenses(): Observable<ExpensesModel[]> {
    return this.http.get<ExpensesModel[]>(`${environment.server}api/expenses/`);
  }

  getAllMonths(): Observable<MonthlyExpense[]> {
    return this.http.get<MonthlyExpense[]>(
      `${environment.server}api/expenses/get-all-months`
    );
  }

  addExpense(expense: ExpensesModel): Observable<ExpensesModel> {
    expense.categories = expense.categories || [];

    return this.http.post<ExpensesModel>(
      `${environment.server}api/expenses/`,
      expense
    );
  }
}
