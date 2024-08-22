import { Component } from '@angular/core';
import {
  FooterComponent,
  NavigationComponent,
} from '@expense-tracker/shared/ui-common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [NavigationComponent, RouterModule, FooterComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {
  title = 'expense-tracker';
}
