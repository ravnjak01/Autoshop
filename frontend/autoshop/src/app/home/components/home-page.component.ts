
import { CommonModule } from '@angular/common';
import {Component} from '@angular/core';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
  standalone: true,
  imports: [CommonModule, RouterModule],
})
export class HomePageComponent{

  title = 'Home page ';
  constructor(
  ) {
  }
}

