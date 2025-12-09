import { Routes } from '@angular/router';
import { MainExpensesComponent } from './main-expenses/main-expenses.component';
import { ensureDateGuard } from '@expense-tracker/expenses/utils';

export const EXPENSES_ROUTES: Routes = [
  {
    path: '',
    component: MainExpensesComponent,
    canActivate: [ensureDateGuard()],
  },
  {
    path: ':year/:month',
    component: MainExpensesComponent,
  },
];
