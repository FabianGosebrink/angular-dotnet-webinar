import { Directive, effect, ElementRef, inject, input } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { ExpensesModel } from '@expense-tracker/expenses/domain';

Chart.register(...registerables);

@Directive({
  selector: '[libExpensesChart]',
  standalone: true,
})
export class ExpensesChartDirective {
  monthlyExpenses = input<ExpensesModel[]>([]);

  private readonly el = inject(ElementRef);
  private chart: Chart | null = null;

  private updateChart = effect(() => {
    this.destroyChart();
    this.createChart(this.monthlyExpenses());
  });

  private destroyChart() {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  private createChart(monthlyExpenses: ExpensesModel[]) {
    const date = new Date(monthlyExpenses[0]?.expenseDate);
    const year = date.getFullYear();
    const month = date.getMonth();
    const daysInMonth = this.daysInMonth(year, month);
    const chartData = this.getChartData(daysInMonth);

    this.applyChartToElement(daysInMonth, chartData);
  }

  private getChartData(daysInMonth: number[]): number[] {
    const zeroFilledDaysInMonthArray = Array(daysInMonth.length).fill(0);

    return this.calculateMonthlyData(
      zeroFilledDaysInMonthArray,
      this.monthlyExpenses()
    );
  }

  private applyChartToElement(daysInMonth: number[], data: number[]): void {
    this.chart = new Chart(this.el.nativeElement, {
      type: 'bar',
      data: {
        labels: daysInMonth,
        datasets: [
          {
            data,
            borderWidth: 1,
            label: 'Amount',
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }

  private daysInMonth(year: number, month: number) {
    const daysInMonth = new Date(year, month, 0).getDate();

    return Array.from({ length: daysInMonth }, (_, i) => i + 1);
  }

  private calculateMonthlyData(
    zeroFilledDaysInMonthArray: number[],
    monthlyExpenses: ExpensesModel[]
  ): number[] {
    return zeroFilledDaysInMonthArray.map((_, index) => {
      const dayInMonth = index + 1;

      return this.calculateTotalExpensesForDay(dayInMonth, monthlyExpenses);
    });
  }

  private calculateTotalExpensesForDay(
    dayInMonth: number,
    monthlyExpenses: ExpensesModel[]
  ): number {
    const expensesForDay = this.getAllExpensesForDay(
      dayInMonth,
      monthlyExpenses
    );

    return expensesForDay.reduce((total, expense) => total + expense.value, 0);
  }

  private getAllExpensesForDay(
    dayInMonth: number,
    monthlyExpenses: ExpensesModel[]
  ): ExpensesModel[] {
    return monthlyExpenses.filter((expense) => {
      const expenseDate = new Date(expense.expenseDate);

      return expenseDate.getDate() === dayInMonth;
    });
  }
}
