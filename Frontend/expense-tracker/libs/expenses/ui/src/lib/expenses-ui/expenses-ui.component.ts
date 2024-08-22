import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'lib-expenses-ui',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './expenses-ui.component.html',
  styleUrl: './expenses-ui.component.css',
})
export class ExpensesUiComponent {}
