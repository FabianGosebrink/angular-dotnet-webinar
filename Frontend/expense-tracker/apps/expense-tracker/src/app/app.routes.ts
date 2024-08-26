import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: 'expenses',
    loadChildren: () =>
      import('@expense-tracker/expenses/feature').then(
        (m) => m.EXPENSES_ROUTES
      ),
  },

  { path: '', redirectTo: '/expenses', pathMatch: 'full' },
];
