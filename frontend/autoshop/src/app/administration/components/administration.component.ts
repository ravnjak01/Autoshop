import { CommonModule } from '@angular/common';
import {Component} from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-administration-root',
  templateUrl: './administration.component.html',
  styleUrl: './administration.component.css',
  standalone: true,
  imports: [
   RouterOutlet,CommonModule,RouterModule
  ]
})
export class AdministrationComponent {
  title = 'Administration ';
}
