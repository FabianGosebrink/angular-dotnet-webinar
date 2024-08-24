import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ExpensesFormComponent,
  ExpensesListComponent,
} from '@expense-tracker/expenses/ui';
import { ExpensesModel, ExpensesStore } from '@expense-tracker/expenses/domain';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lib-main-expenses',
  standalone: true,
  imports: [CommonModule, ExpensesFormComponent, ExpensesListComponent],
  templateUrl: './main-expenses.component.html',
  styleUrl: './main-expenses.component.scss',
})
export class MainExpensesComponent implements OnInit {
  expensesStore = inject(ExpensesStore);
  date = signal<Date | null>(null);

  private route = inject(ActivatedRoute);

  ngOnInit() {
    this.route.params.subscribe(({ year, month }) => {
      this.date.set(new Date(year, month - 1));
      this.expensesStore.loadAllExpensesByYearAndMonth({ year, month });
    });
  }

  formSubmitted(expense: ExpensesModel): void {
    this.expensesStore.addExpense(expense);
  }

  expenseDeleted(expense: ExpensesModel) {
    this.expensesStore.deleteExpense(expense);
  }
}
