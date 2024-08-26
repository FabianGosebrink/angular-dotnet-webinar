import { Component } from '@angular/core';
import { LayoutComponent } from './layout/layout.component';

@Component({
  standalone: true,
  imports: [LayoutComponent],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {}
