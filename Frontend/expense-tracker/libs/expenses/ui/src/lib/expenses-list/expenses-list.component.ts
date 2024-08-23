import { Component, input } from '@angular/core';
import { Expense } from '@expense-tracker/expenses/domain';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'lib-expenses-list',
  standalone: true,
  imports: [JsonPipe],
  templateUrl: './expenses-list.component.html',
  styleUrl: './expenses-list.component.scss',
})
export class ExpensesListComponent {
  expenses = input<Expense[]>([]);
}
