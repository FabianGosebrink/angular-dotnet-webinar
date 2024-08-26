import { Component, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ExpensesModel } from '@expense-tracker/expenses/domain';

@Component({
  selector: 'lib-expenses-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './expenses-form.component.html',
  styleUrl: './expenses-form.component.scss',
})
export class ExpensesFormComponent {
  formSubmitted = output<ExpensesModel>();

  form = new FormGroup({
    title: new FormControl('', Validators.required),
    value: new FormControl(0, Validators.required),
    date: new FormControl('', Validators.required),
  });

  submitForm() {
    const { title, date, value } = this.form.value;
    const toSend = {
      name: title as string,
      value: value as number,
      expenseDate: date,
    } as ExpensesModel;

    this.formSubmitted.emit(toSend);
  }
}
