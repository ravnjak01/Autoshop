import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {debounceTime, distinctUntilChanged, Subject} from 'rxjs';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {
  DiscountGetAllResponse,
  DiscountGetAllService
} from '../../../endpoints/discount-endpoints/discount-get-all-endpoint.service';
import {MatDialog} from '@angular/material/dialog';
import {DiscountPostComponent} from './discount-post/discount-post.component';
import {MyDialogConfirmComponent} from '../../shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {DiscountDeleteEndpointService} from '../../../endpoints/discount-endpoints/discount-delete-endpoint.service';
import {DiscountEditComponent} from './discount-post-editing/discount-posts-editing.component';
import {DiscountCodesComponent} from './discount-code/discount-codes.component';
import {DiscountCategoryDialogComponent} from './discount-categories/discount-category.component';
import {DiscountProductDialogComponent} from './discount-products/discount-product.component';
import { SharedModule } from '../../shared/shared.module';
@Component({
  selector: 'app-discounts',
  templateUrl: 'discount.component.html',
  styleUrls: ['discount.component.css'],
  standalone: false,

})
export class DiscountsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['name', 'discountPercentage', 'startDate', 'endDate', 'actions'];
  dataSource = new MatTableDataSource<DiscountGetAllResponse>();
  totalDiscounts = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private searchSubject: Subject<string> = new Subject();
  constructor(
    private discountGetService: DiscountGetAllService,
    private dialog: MatDialog,
    private discountDeleteService: DiscountDeleteEndpointService) {}

  ngOnInit(): void {
    this.initSearchListener();
    this.fetchDiscounts();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.paginator.page.subscribe(() => {
      const filterValue = this.dataSource.filter || '';
      this.fetchDiscounts(filterValue, this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  initSearchListener(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((filterValue) => {
      this.fetchDiscounts(filterValue, 1, this.paginator.pageSize);
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.paginator.pageIndex = 0;
    this.searchSubject.next(filterValue);
  }

  fetchDiscounts(filter: string = '', page: number = 1, pageSize: number = 5): void {
    this.discountGetService.handleAsync(
      { q: filter, pageNumber: page, pageSize: pageSize },
      true
    ).subscribe({
      next: (data) => {
        this.dataSource.data = data.dataItems;
        this.paginator.pageIndex = data.currentPage - 1;
        this.paginator.length = data.totalCount;
        this.paginator.pageSize = data.pageSize;
      },
      error: (err) => {
        console.error('Error fetching discounts:', err);
      }
    });
  }

  openDiscountPostForm(event: MouseEvent, discountId?: number): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(DiscountEditComponent, {
      width: '800px',
      maxHeight: '80vh',
      data: { discountId: discountId || 0 },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'saved' || result === 'updated') {
        this.refreshPage();
      }
    });
  }

  deleteDiscount(id: number): void {
    this.discountDeleteService.handleAsync(id).subscribe({
      next: () => {
        this.refreshPage();
      },
      error: (err) => console.error('Error deleting discount:', err)
    });
  }

  openMyConfirmDialog(id: number, event: MouseEvent) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Confirm deleting',
        message: 'Are you sure you want to delete this'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Korisnik je potvrdio brisanje');
        // Pozovite servis ili izvršite logiku za brisanje
        this.deleteDiscount(id);
      } else {
        console.log('Korisnik je otkazao brisanje');
      }
    });
  }

  openDiscountModal(id: number) {
    const dialogRef = this.dialog.open(DiscountPostComponent, {
      width: '600px',
      data: { discountId: id}, // Pass blogId if editing, 0 for new blog
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'saved') {
        // If the modal is saved, reload or do something (like redirect)
        this.refreshPage();
      }
    });
  }

  openDiscountCodesDialog(discountId: number, event: MouseEvent) {
    event.stopPropagation();

    const dialogRef = this.dialog.open(DiscountCodesComponent, {
      width: '600px',
      maxHeight: '80vh',
      data: { discountId: discountId },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if(result === 'updated') {
        this.refreshPage()
        console.log('Codes updated');
      }
    });
  }

  refreshPage(): void {
    window.location.reload();
  }

  openCategoryDialog(discount: any, event: MouseEvent) {
    event.stopPropagation();

    this.dialog.open(DiscountCategoryDialogComponent, {
      width: '500px',
      data: { discountId: discount.id, discountName: discount.name }
    });
  }

  openProductDialog(discount: any, event: MouseEvent) {
    event.stopPropagation();

    this.dialog.open(DiscountProductDialogComponent, {
      width: '500px',
      data: { discountId: discount.id, discountName: discount.name }
    });
  }
}
