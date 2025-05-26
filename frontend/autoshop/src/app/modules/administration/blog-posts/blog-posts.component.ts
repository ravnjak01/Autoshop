
import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
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
import {BlogDeleteEndpointService} from '../../../endpoints/blog-endpoints/blogs-delete-endpoint.service';
import {BlogDeactivateEndpointService} from '../../../endpoints/blog-endpoints/blog-deactivate-endpoint.service';
import {MyDialogSimpleComponent} from '../../shared/dialogs/my-dialog-simple/my-dialog-simple.component';
import {BlogPublishEndpointService} from '../../../endpoints/blog-endpoints/blog-publish-endpoint.service';
import {BlogPostComponent} from './blog-post/blog-post.component';
@Component({
  selector: 'app-blog-posts',
  templateUrl: './blog-posts.component.html',
  styleUrls: ['./blog-posts.component.css'],
  standalone: false
})
export class BlogPostsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['title', 'authorName', 'publishedTime', 'isPublished', 'active', 'actions'];
  dataSource = new MatTableDataSource<BlogsGetAllForAdministrationResponse>();
  totalBlogs = 0;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  private searchSubject: Subject<string> = new Subject();

  constructor(
    private blogGetService: BlogsGetAllForAdministrationService,
    private blogDeleteService: BlogDeleteEndpointService,
    private blogDeactivateService: BlogDeactivateEndpointService,
    private blogPublishService: BlogPublishEndpointService,
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

  ngAfterViewInit(): void { // Bind sorting to the table
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
        this.dataSource.paginator = data.paginator;
        this.paginator.pageIndex = data.currentPage - 1; // Backend page is 1-based, paginator is 0-based
        this.paginator.pageSize = data.pageSize;
        this.paginator.length = data.totalCount;
      },
      error: (err: any) => {
        console.error('Error fetching blogs:', err);
      },
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.paginator.pageIndex = 0; // Reset to the first page
    this.searchSubject.next(filterValue); // Send the new filter value
  }

  openBlogPostForm(event: MouseEvent, blogId?: number): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(BlogEditComponent, {
      width: '800px', // Povećana širina dijaloga
      maxHeight: '80vh', // Maksimalna visina dijaloga
      data: { blogId: blogId || 0 }, // Pass blogId if editing, 0 for new blog
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'saved') {
        // If the modal is saved, reload or do something (like redirect)
        this.refreshPage();
      }
    });
  }

  deactivate(id: number, event: MouseEvent): void {
    event.stopPropagation();
    this.blogDeactivateService.handleAsync(id).subscribe({
      next: () => {
        //this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste
        this.openMySimpleDialog("Uspješno ste izvršili akciju");
      },
      error: (err) => console.error('Error deactivate blog:', err)
    });
  }

  deleteBlog(id: number): void {

    this.blogDeleteService.handleAsync(id).subscribe({
      next: () => {
        //this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste

      },
      error: (err) => console.error('Error deleting blog:', err)
    });
  }

  openMySimpleDialog(text: string) {
    const dialogRef = this.dialog.open(MyDialogSimpleComponent, {
      width: '350px',
      data: {
        title: 'Uspjeh',
        message: text
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshPage();
      }
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
        this.deleteBlog(id);
        this.refreshPage();
      } else {
        console.log('Korisnik je otkazao brisanje');
      }
    });
  }

  publish(id: number, event: MouseEvent) {
    event.stopPropagation();
    this.blogPublishService.handleAsync(id).subscribe({
      next: () => {
        //this.cities = this.cities.filter(city => city.id !== id); // Uklanjanje iz lokalne liste
        this.openMySimpleDialog("Uspješno ste izvršili akciju");
      },
      error: (err) => console.error('Error deactivate blog:', err)
    });
  }

  openBlogModal(id: number) {
    const dialogRef = this.dialog.open(BlogPostComponent, {
      width: '600px',
      data: { blogId: id}, // Pass blogId if editing, 0 for new blog
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'saved') {
        // If the modal is saved, reload or do something (like redirect)
        this.refreshPage();
      }
    });
  }

  refreshPage(): void {
    window.location.reload();
  }
}
