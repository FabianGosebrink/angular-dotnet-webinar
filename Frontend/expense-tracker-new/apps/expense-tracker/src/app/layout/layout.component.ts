import { Component, inject, OnInit } from '@angular/core';
import {
  FooterComponent,
  NavigationComponent,
} from '@expense-tracker/shared/ui-common';
import { RouterModule } from '@angular/router';
import { ExpensesStore } from '@expense-tracker/expenses/domain';

@Component({
  selector: 'app-layout',
  imports: [NavigationComponent, RouterModule, FooterComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent implements OnInit {
  title = 'Expense Tracker';
  expensesStore = inject(ExpensesStore);

  ngOnInit() {
    this.expensesStore.loadAllExpensesPerMonth();
  }
}
