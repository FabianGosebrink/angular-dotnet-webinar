import { Component, input, output } from '@angular/core';
import { ExpensesModel } from '@expense-tracker/expenses/domain';
import { CurrencyPipe, DatePipe, JsonPipe } from '@angular/common';

@Component({
  selector: 'lib-expenses-list',
  standalone: true,
  imports: [JsonPipe, DatePipe, CurrencyPipe],
  templateUrl: './expenses-list.component.html',
  styleUrl: './expenses-list.component.scss',
})
export class ExpensesListComponent {
  expenseDeleted = output<ExpensesModel>();

  expenses = input<ExpensesModel[]>([]);

  deleteExpense(expenseModel: ExpensesModel) {
    this.expenseDeleted.emit(expenseModel);
  }
}
