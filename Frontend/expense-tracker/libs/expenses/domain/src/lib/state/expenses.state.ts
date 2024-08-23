import {
  patchState,
  signalStore,
  withComputed,
  withHooks,
  withMethods,
  withState,
} from '@ngrx/signals';
import { inject } from '@angular/core';
import { tapResponse } from '@ngrx/operators';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { switchMap } from 'rxjs';
import { Expense } from '../models/expense';
import { ExpensesApiService } from '../services/expenses-api.service';

export interface ExpensesState {
  expenses: Expense[];
  loading: boolean;
}

export const initialState: ExpensesState = {
  expenses: [],
  loading: false,
};

export const ExpensesStore = signalStore(
  { providedIn: 'root' },
  withState<ExpensesState>(initialState),
  withComputed((store) => ({
    // getAllIdsOfMyDoggos: computed(() => {
    //   const myDoggos = store.myDoggos();
    //
    //   if (myDoggos.length === 0) {
    //     return [];
    //   }
    //
    //   return myDoggos.map((x) => x.id);
    // }),
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
            next: (expenses: Expense[]) => {
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

    addExpense: rxMethod<Expense>(
      switchMap((expense) => {
        patchState(store, { loading: true });

        return expensesApiService.addExpense(expense).pipe(
          tapResponse({
            next: (expense) => {
              patchState(store, ({ expenses }) => ({
                expenses: [...expenses, expense],
              }));
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
