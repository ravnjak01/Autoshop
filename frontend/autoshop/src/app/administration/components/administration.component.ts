import {Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-administration-root',
  templateUrl: './administration.component.html',
  styleUrl: './administration.component.css',
  standalone: true,
  imports: [
   RouterOutlet
  ]
})
export class AdministrationComponent {
  title = 'Administration ';
}
