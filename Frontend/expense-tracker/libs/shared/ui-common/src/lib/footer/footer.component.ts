import { Component } from '@angular/core';
import { environment } from '@expense-tracker/shared/util-environments';

@Component({
  selector: 'lib-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {
  backendUrl = environment.server;
}
