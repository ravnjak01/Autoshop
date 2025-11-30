import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.css'],
  standalone: true,
  imports: [CommonModule],
})
export class ConfirmationModalComponent {
  @Input() title = 'Confirmation';
  @Input() message = 'Are you sure?';
  @Input() isVisible = false;

  @Output() onConfirm = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  confirm() {
    this.onConfirm.emit();
    this.isVisible = false;
  }

  cancel() {
    this.onCancel.emit();
    this.isVisible = false;
  }
}
