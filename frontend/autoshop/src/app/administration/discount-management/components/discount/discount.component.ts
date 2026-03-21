import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {debounceTime, distinctUntilChanged, Subject} from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {
  DiscountGetAllResponse,
  DiscountGetAllService
} from '../../services/discount-get-all-endpoint.service';
import {MatDialog} from '@angular/material/dialog';
import {DiscountPostComponent} from '../discount-post/discount-post.component';
import {MyDialogConfirmComponent} from '../../../../modules/shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {DiscountDeleteEndpointService} from '../../services/discount-delete-endpoint.service';
import {DiscountEditComponent} from '../discount-post-editing/discount-posts-editing.component';
import {DiscountCodesComponent} from '../discount-code/discount-codes.component';
import {DiscountCategoryDialogComponent} from '../discount-categories/discount-category.component';
import {DiscountProductDialogComponent} from '../discount-products/discount-product.component';
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
  readonly DEFAULT_PAGE_SIZE = 5;

  constructor(
    private discountGetService: DiscountGetAllService,
    private dialog: MatDialog,
    private discountDeleteService: DiscountDeleteEndpointService) {}
  ngOnInit(): void {
    this.initSearchListener();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;

    this.fetchDiscounts('', 1, this.DEFAULT_PAGE_SIZE);

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
      this.paginator.pageIndex = 0;

      this.fetchDiscounts(filterValue, 1, this.paginator.pageSize);
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();

    this.dataSource.filter = filterValue;
    this.searchSubject.next(filterValue);
  }

  fetchDiscounts(filter: string = '', page: number = 1, pageSize: number = 5): void {
    const resolvedPageSize =
      pageSize ??
      this.paginator?.pageSize ??
      this.DEFAULT_PAGE_SIZE;

    this.discountGetService.handleAsync(
      { q: filter, pageNumber: page, pageSize: resolvedPageSize }
    ).subscribe({
      next: (data) => {
        this.dataSource.data = data.dataItems;
        this.paginator.pageIndex = data.currentPage - 1;
        this.paginator.length = data.totalCount;
        this.paginator.pageSize = data.pageSize;
      },
      error: (err) => {
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
      }
    });
  }

  openMyConfirmDialog(id: number, event: MouseEvent) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '450px',
      data: {
        title: 'Confirm deleting',
        message: 'Are you sure you want to delete this'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Pozovite servis ili izvršite logiku za brisanje
        this.deleteDiscount(id);
      } 
    });
  }

  openDiscountModal(id: number) {
    const dialogRef = this.dialog.open(DiscountPostComponent, {
      width: '500px',
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
      }
    });
  }

  refreshPage(): void {
    const filterValue = this.dataSource.filter || '';
    const page = this.paginator.pageIndex + 1;
    const pageSize = this.paginator.pageSize;

    this.fetchDiscounts(filterValue, page, pageSize);
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
