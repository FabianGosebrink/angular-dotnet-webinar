import {
  patchState,
  signalStore,
  withComputed,
  withHooks,
  withMethods,
  withState,
} from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { tapResponse } from '@ngrx/operators';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { switchMap } from 'rxjs';
import { ExpensesModel } from '../models/expenses.models';
import { ExpensesApiService } from '../services/expenses-api.service';
import { MonthlyExpense } from '@expense-tracker/shared/util-common';

export interface ExpensesState {
  expenses: ExpensesModel[];
  overviewMonthlyExpenses: MonthlyExpense[];
  loading: boolean;
}

export const initialState: ExpensesState = {
  expenses: [],
  overviewMonthlyExpenses: [],
  loading: false,
};

export const ExpensesStore = signalStore(
  { providedIn: 'root' },
  withState<ExpensesState>(initialState),
  withComputed((store) => ({
    getSumForMonth: computed(() => {
      return store.expenses().reduce((a, b) => a + b.value, 0);
    }),
    expensesOrderedByDate: computed(() => {
      return store.expenses().sort((a, b) => {
        const dateA = new Date(a.expenseDate);
        const dateB = new Date(b.expenseDate);

        return dateA.getTime() - dateB.getTime();
      });
    }),
    // getSelectedDoggoIndex: computed(() => {
    //   return store
    //     .doggos()
    //     .findIndex((doggo) => doggo.id === store.selectedDoggo().id);
    // }),
    // getNextDoggoIndex: computed(() => {
    //   const currentDoggoIndex = store
    //     .doggos()
    //     .findIndex((doggo) => doggo.id === store.selectedDoggo().id);
    //
    //   return (currentDoggoIndex + 1) % store.doggos().length;
    // }),
    // getAllDoggosButSelected: computed(() => {
    //   if (store.doggos().length === 0) {
    //     return [];
    //   }
    //
    //   if (!store.selectedDoggo()) {
    //     return store.doggos();
    //   }
    //
    //   return store
    //     .doggos()
    //     .filter((doggo) => doggo.id !== store.selectedDoggo().id);
    // }),
    // getUserSub: computed(() => authStore.userSub()),
  })),
  withMethods((store, expensesApiService = inject(ExpensesApiService)) => ({
    loadAllExpenses: rxMethod<void>(
      switchMap(() => {
        patchState(store, { loading: true });

        return expensesApiService.getAllExpenses().pipe(
          tapResponse({
            next: (expenses: ExpensesModel[]) => {
              patchState(store, { expenses });
            },
            error: (error: unknown) => {
              console.error(error);
            },
            finalize: () => patchState(store, { loading: false }),
          })
        );
      })
    ),

    loadAllExpensesByYearAndMonth: rxMethod<{ year?: number; month?: number }>(
      switchMap(({ year, month }) => {
        patchState(store, { loading: true });

        return expensesApiService.getExpensesForMonth(year, month).pipe(
          tapResponse({
            next: (expenses: ExpensesModel[]) => {
              patchState(store, { expenses });
            },
            error: (error: unknown) => {
              console.error(error);
            },
            finalize: () => patchState(store, { loading: false }),
          })
        );
      })
    ),

    loadAllExpensesPerMonth: rxMethod<void>(
      switchMap(() => {
        patchState(store, { loading: true });

        return expensesApiService.getAllMonths().pipe(
          tapResponse({
            next: (overviewMonthlyExpenses: MonthlyExpense[]) => {
              patchState(store, { overviewMonthlyExpenses });
            },
            error: (error: unknown) => {
              console.error(error);
            },
            finalize: () => patchState(store, { loading: false }),
          })
        );
      })
    ),

    addExpense: rxMethod<ExpensesModel>(
      switchMap((expense) => {
        patchState(store, { loading: true });

        return expensesApiService.addExpense(expense).pipe(
          tapResponse({
            next: (expenseResponse) => {
              patchState(store, {
                expenses: [...store.expenses(), expenseResponse],
              });
            },
            error: (error: unknown) => {
              console.error(error);
            },
            finalize: () => patchState(store, { loading: false }),
          })
        );
      })
    ),

    deleteExpense: rxMethod<ExpensesModel>(
      switchMap((expenseToDelete) => {
        patchState(store, { loading: true });

        return expensesApiService.deleteExpense(expenseToDelete).pipe(
          tapResponse({
            next: () => {
              const currentExpenses = store.expenses();
              const expenses = currentExpenses.filter(
                (expense) => expense.id !== expenseToDelete.id
              );

              patchState(store, {
                expenses: [...expenses],
              });
            },
            error: (error: unknown) => {
              console.error(error);
            },
            finalize: () => patchState(store, { loading: false }),
          })
        );
      })
    ),
  })),
  withHooks({
    onInit() {
      console.log('state is there');
    },
  })
);
