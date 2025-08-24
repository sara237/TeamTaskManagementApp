import { Injectable } from '@angular/core';
import { User } from '../models/user.model';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private _currentUserSubject: BehaviorSubject<User | undefined> = new BehaviorSubject<User | undefined>(undefined);

  constructor() {
    this.loadFromStorage();
  }

  get currentUser(): User | undefined {
    return this._currentUserSubject.value;
  }

  get currentUser$(): Observable<User | undefined> {
    return this._currentUserSubject.asObservable();
  }

  isAdmin(): boolean {
    return this._currentUserSubject.value?.role === 'Admin';
  }

  setCurrentUser(user: User) {
    this._currentUserSubject.next(user);
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.setItem('currentUser', JSON.stringify(user));
    }
  }

  loadFromStorage() {
    if (typeof window !== 'undefined' && window.localStorage) {
      const stored = localStorage.getItem('currentUser');
      if (stored) this._currentUserSubject.next(JSON.parse(stored));
    }
  }

  logout() {
    this._currentUserSubject.next(undefined);
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.removeItem('currentUser');
    }
}
}