import { Component, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Expense } from '@expense-tracker/expenses/domain';

@Component({
  selector: 'lib-expenses-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './expenses-form.component.html',
  styleUrl: './expenses-form.component.scss',
})
export class ExpensesFormComponent {
  formSubmitted = output<Expense>();

  form = new FormGroup({
    title: new FormControl('', Validators.required),
    value: new FormControl(0, Validators.required),
    date: new FormControl('', Validators.required),
  });

  submitForm() {
    const { title, date, value } = this.form.value;
    const toSend = {
      title: title as string,
      value: value as number,
      expenseDate: new Date(date as string),
    };

    this.formSubmitted.emit(toSend);
  }
}
