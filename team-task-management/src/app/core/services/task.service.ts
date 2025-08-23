import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { TaskItem } from '../models/task-item.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly baseUrl = `${environment.apiBaseUrl}/task`;

  constructor(private http: HttpClient) {}

  getAll(params?: { search?: string; status?: number; assignedUserId?: string }): Observable<TaskItem[]> {
    let httpParams = new HttpParams();
    if (params?.search) httpParams = httpParams.set('search', params.search);
    if (params?.status) httpParams = httpParams.set('status', params.status);
    if (params?.assignedUserId) httpParams = httpParams.set('assignedUserId', params.assignedUserId);
    return this.http.get<TaskItem[]>(this.baseUrl, { params: httpParams });
  }

  getById(id: string): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this.baseUrl}/${id}`);
  }

  create(task: Partial<TaskItem>): Observable<TaskItem> {
    return this.http.post<TaskItem>(this.baseUrl, task);
  }

  update(id: string, task: Partial<TaskItem>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, task);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
