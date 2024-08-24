export interface ExpensesModel {
  id: number;
  name: string;
  value: number;
  categories: string[];
  expenseDate: string;
}

export interface MonthlyExpense {
  year: number;
  month: number;
  sum: number;
}
