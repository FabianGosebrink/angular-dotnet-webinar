import { Component, output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'lib-expenses-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './expenses-form.component.html',
  styleUrl: './expenses-form.component.scss',
})
export class ExpensesFormComponent {
  formSubmitted = output<{ title: string; expenseDate: Date }>();

  form = new FormGroup({
    title: new FormControl('', Validators.required),
    date: new FormControl('', Validators.required),
  });

  submitForm() {
    const { title, date } = this.form.value;
    const toSend = {
      title: title as string,
      expenseDate: new Date(date as string),
    };

    this.formSubmitted.emit(toSend);
  }
}
