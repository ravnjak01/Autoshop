import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {Router} from '@angular/router';
import {MyDialogConfirmComponent} from '../../shared/dialogs/my-dialog-confirm/my-dialog-confirm.component';
import {
  BlogsGetAllForAdministrationService,
  BlogsGetAllForAdministrationResponse
} from '../../../endpoints/blog-endpoints/blogs-get-all-for-administration-endpoint.service';
import {debounceTime, distinctUntilChanged, Subject} from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import {BlogEditComponent} from './blog-posts-editing/blog-posts-editing.component';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {BlogDeleteEndpointService} from '../../../endpoints/blog-endpoints/blogs-delete-endpoint.service';
@Component({
  selector: 'app-blog-posts',
  templateUrl: './blog-posts.component.html',
  styleUrls: ['./blog-posts.component.css'],
  standalone: false
})
export class BlogPostsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['title', 'authorName', 'publishedTime', 'isPublished', 'actions'];
  dataSource = new MatTableDataSource<BlogsGetAllForAdministrationResponse>();
  totalBlogs = 0;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  private searchSubject: Subject<string> = new Subject();

  constructor(
    private blogGetService: BlogsGetAllForAdministrationService,
    private blogDeleteService: BlogDeleteEndpointService,
    private router: Router,
    private dialog: MatDialog
  ) {
  }

  ngOnInit(): void {
    this.initSearchListener();
    this.fetchBlogs();
  }

  initSearchListener(): void {
    this.searchSubject.pipe(
      debounceTime(300), // Vrijeme čekanja (300ms)
      distinctUntilChanged(), // Emittuje samo ako je vrijednost promijenjena,
    ).subscribe((filterValue) => {
      this.fetchBlogs(filterValue, this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort; // Bind sorting to the table
    this.dataSource.paginator = this.paginator; // Bind paginator to the table

    // Listen for paginator changes
    this.paginator.page.subscribe(() => {
      const filterValue = this.dataSource.filter || '';
      this.fetchBlogs(filterValue, this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
  }

  fetchBlogs(filter: string = '', page: number = 1, pageSize: number = 5): void {
    this.blogGetService.handleAsync(
      {
        q: filter,
        pageNumber: page,
        pageSize: pageSize
      },
      true,
    ).subscribe({
      next: (data: any) => {
        // Update data source and paginator
        this.dataSource = new MatTableDataSource<BlogsGetAllForAdministrationResponse>(data.dataItems);
        this.paginator.length = data.totalCount; // Set total item count
      },
      error: (err: any) => {
        console.error('Error fetching blogs:', err);
      },
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.searchSubject.next(filterValue); // Prosljeđuje vrijednost Subject-u
  }

  openBlogPostForm(blogId?: number): void {
    const dialogRef = this.dialog.open(BlogEditComponent, {
      width: '600px',
      data: { blogId: blogId || 0 }, // Pass blogId if editing, 0 for new blog
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'saved') {
        // If the modal is saved, reload or do something (like redirect)
        this.ngOnInit();
      }
    });
  }
  // editCity(id: number): void {
  //   this.router.navigate(['/admin/cities3/edit', id]);
  // }
  //

  deactivate(id: number): void {

  }

  deleteCity(id: number): void {

    this.blogDeleteService.handleAsync(id).subscribe({
      next: () => {
        //this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste
      },
      error: (err) => console.error('Error deleting city:', err)
    });
  }

  openMyConfirmDialog(id: number) {
    const dialogRef = this.dialog.open(MyDialogConfirmComponent, {
      width: '350px',
      data: {
        title: 'Potvrda brisanja',
        message: 'Da li ste sigurni da želite obrisati ovu stavku?'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Korisnik je potvrdio brisanje');
        // Pozovite servis ili izvršite logiku za brisanje
        this.deleteCity(id);
        this.ngOnInit();
      } else {
        console.log('Korisnik je otkazao brisanje');
      }
    });
  }
}
