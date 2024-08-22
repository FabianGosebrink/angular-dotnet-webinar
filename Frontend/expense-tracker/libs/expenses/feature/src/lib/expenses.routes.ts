import { Routes } from '@angular/router';
import { MainExpensesComponent } from './main-expenses/main-expenses.component';

export const EXPENSES_ROUTES: Routes = [
  {
    path: '',
    component: MainExpensesComponent,
  },
];
