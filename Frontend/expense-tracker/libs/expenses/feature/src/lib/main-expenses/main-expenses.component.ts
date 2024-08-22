import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExpensesFormComponent } from '@expense-tracker/expenses/ui';

@Component({
  selector: 'lib-main-expenses',
  standalone: true,
  imports: [CommonModule, ExpensesFormComponent],
  templateUrl: './main-expenses.component.html',
  styleUrl: './main-expenses.component.scss',
})
export class MainExpensesComponent {
  formSubmitted({ title, expenseDate }: { title: string; expenseDate: Date }) {
    console.log(title, expenseDate);
  }
}
