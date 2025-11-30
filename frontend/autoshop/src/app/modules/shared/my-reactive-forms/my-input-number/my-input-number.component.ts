import {Component, Input, OnInit} from '@angular/core';
import {MyBaseFormControlComponent} from '../my-base-form-control-component';
import {ControlContainer} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
export enum MyInputNumberType {
  Text = 'text',
  Password = 'password',
  Email = 'email',
  Number = 'number',
  Tel = 'tel',
  Url = 'url'
}

@Component({
  selector: 'app-my-input-number',
  standalone: false,
  templateUrl: './my-input-number.component.html',
  styleUrl: './my-input-number.component.css'
})
export class MyInputNumberComponent extends MyBaseFormControlComponent implements OnInit {
  @Input() myLabel!: string; // Labela za input
  @Input() myId: string = ''; // ID za input (koristi se u <label> for atributu)
  @Input() myPlaceholder: string = ''; // Placeholder tekst
  @Input() myType: MyInputNumberType = MyInputNumberType.Number; // Tip inputa koristi enumeraciju

  @Input() override customMessages: Record<string, string> = {}; // Dodano!
  @Input() override myControlName: string = "";

  constructor(protected override controlContainer: ControlContainer) {
    super(controlContainer);
  }

  ngOnInit(): void {
    // Ako nije eksplicitno postavljen id, koristi naziv kontrola
    if (!this.myId && this.myId === '' && this.formControl) {
      this.myId = this.getControlName();
    }
  }

}
