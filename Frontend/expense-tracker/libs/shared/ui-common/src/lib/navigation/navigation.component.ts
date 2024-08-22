import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-navigation',
  standalone: true,
  imports: [],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss',
})
export class NavigationComponent {
  title = input<string>();
}
