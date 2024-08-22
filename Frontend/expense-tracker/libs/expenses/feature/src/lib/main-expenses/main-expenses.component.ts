import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpensesFormComponent } from '@expense-tracker/expenses/ui';
import { Expense } from '@expense-tracker/expenses/domain';

@Component({
  selector: 'lib-main-expenses',
  standalone: true,
  imports: [CommonModule, ExpensesFormComponent],
  templateUrl: './main-expenses.component.html',
  styleUrl: './main-expenses.component.scss',
})
export class MainExpensesComponent {
  formSubmitted(expense: Expense): void {
    console.log(expense);
  }
}