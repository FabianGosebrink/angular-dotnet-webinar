import { Component, input, output } from '@angular/core';
import { ExpensesModel } from '@expense-tracker/expenses/domain';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ExpensesChartDirective } from '../expenses-chart/expenses-chart.directive';

@Component({
  selector: 'lib-expenses-list',
  imports: [DatePipe, CurrencyPipe, ExpensesChartDirective],
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
