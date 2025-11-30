import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { MyAuthService } from '../../services/auth/my-auth.service';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<MyAuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('MyAuthService', ['isLoggedIn', 'isAdmin', 'isManager']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: MyAuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(MyAuthService) as jasmine.SpyObj<MyAuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should redirect to login if user is not logged in', () => {
    authService.isLoggedIn.and.returnValue(false);
    const route = { data: {} } as any;

    const result = guard.canActivate(route);

    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should allow access if user is logged in', () => {
    authService.isLoggedIn.and.returnValue(true);
    const route = { data: {} } as any;

    const result = guard.canActivate(route);

    expect(result).toBe(true);
  });

  it('should redirect to unauthorized if admin access required but user is not admin', () => {
    authService.isLoggedIn.and.returnValue(true);
    authService.isAdmin.and.returnValue(false);
    const route = { data: { isAdmin: true } } as any;

    const result = guard.canActivate(route);

    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/unauthorized']);
  });

  it('should allow access if admin access required and user is admin', () => {
    authService.isLoggedIn.and.returnValue(true);
    authService.isAdmin.and.returnValue(true);
    const route = { data: { isAdmin: true } } as any;

    const result = guard.canActivate(route);

    expect(result).toBe(true);
  });
});