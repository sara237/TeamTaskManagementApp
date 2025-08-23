import { Component, OnInit } from '@angular/core';
import { Priority } from '../../../../core/enums/priority.enum';
import { TaskStatus } from '../../../../core/enums/task-status.enum';
import { TaskItem } from '../../../../core/models/task-item.model';
import { User } from '../../../../core/models/user.model';
import { TaskService } from '../../../../core/services/task.service';
import { UserService } from '../../../../core/services/user.service';
import { map, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-task-list',
  standalone: false,
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  tasks: TaskItem[] = [];
  users: User[] = [];

  search = '';
  status?: number;
  assigneeId = '';

  TaskStatus = TaskStatus;
  Priority = Priority;

  loading = false;
  error?: string;
  private userCache: { [id: string]: string } = {};
  constructor(private taskSvc: TaskService, private userSvc: UserService) {}

  ngOnInit(): void {
    this.userSvc.getAll().subscribe(u => this.users = u);
    this.load();
  }
getAssignedUserName(userId: string): Observable<string> {
  if (this.userCache[userId]) {
    return of(this.userCache[userId]);
  }

  return this.userSvc.getById(userId).pipe(
    map(user => {
      const userName = user?.name ?? 'Unknown User';
      this.userCache[userId] = userName;
      debugger;
      return userName;
    }),
    catchError(() => {
      this.userCache[userId] = 'Unknown User';
      return of('Unknown User');
    })
  );
}
  load() {
    debugger;
    this.loading = true;
    this.taskSvc.getAll({
      search: this.search || undefined,
      status: this.status || undefined,

      assignedUserId: this.assigneeId || undefined
    }).subscribe({
      next: t => { this.tasks = t; this.loading = false; },
      error: err => { this.error = 'Failed to load tasks'; this.loading = false; console.error(err); }
    });
  }

  clearFilters() {
    this.search = '';
    this.status = undefined;
    this.assigneeId = '';
    this.load();
  }

  delete(task: TaskItem) {
    if (!confirm(`Delete task "${task.title}"?`)) return;
    this.taskSvc.delete(task.id).subscribe({
      next: () => this.load()
    });
  }
}
