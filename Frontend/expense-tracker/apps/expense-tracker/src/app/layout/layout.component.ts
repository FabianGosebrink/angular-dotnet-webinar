import { Component, inject, OnInit } from '@angular/core';
import {
  FooterComponent,
  NavigationComponent,
} from '@expense-tracker/shared/ui-common';
import { RouterModule } from '@angular/router';
import { ExpensesStore } from '@expense-tracker/expenses/domain';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [NavigationComponent, RouterModule, FooterComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent implements OnInit {
  expensesStore = inject(ExpensesStore);

  title = 'expense-tracker';

  ngOnInit() {
    this.expensesStore.loadAllExpensesPerMonth();
  }
}
