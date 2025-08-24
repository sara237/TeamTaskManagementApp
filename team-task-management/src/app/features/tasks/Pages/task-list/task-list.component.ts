import { Component, OnInit } from '@angular/core';
import { Priority } from '../../../../core/enums/priority.enum';
import { TaskStatus } from '../../../../core/enums/task-status.enum';
import { TaskItem } from '../../../../core/models/task-item.model';
import { User } from '../../../../core/models/user.model';
import { TaskService } from '../../../../core/services/task.service';
import { UserService } from '../../../../core/services/user.service';
import { map, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { TaskFilterDto } from '../../../../core/dtos/task-filter-dto';
import { CurrentUserService } from '../../../../core/services/current-user.service';
import { Router } from '@angular/router';
import { ChatService } from '../../../../core/services/chat.service';

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

  currentUser?: User;
  
  private userCache: { [id: string]: string } = {};


  constructor(private taskSvc: TaskService, private userSvc: UserService, 
              private currentUserService: CurrentUserService, private router: Router,
              private chatService: ChatService) 
  { }

  ngOnInit(): void {

   this.currentUser = this.currentUserService.currentUser;

   if (!this.currentUser) {
     this.router.navigate(['/login']);
    } else {
    this.userSvc.getAll().subscribe(u => this.users = u);
    this.load();
    }
  }


  getAssignedUserName(userId: string): Observable<string> {
  if (this.userCache[userId]) {
    return of(this.userCache[userId]);
  }

  return this.userSvc.getById(userId).pipe(
    map(user => {
      const userName = user?.name ?? 'Unknown User';
      this.userCache[userId] = userName;
      return userName;
    }),
    catchError(() => {
      this.userCache[userId] = 'Unknown User';
      return of('Unknown User');
    })
  );
}
protected load() {
  this.loading = true;
  const filter: TaskFilterDto = {
    title: this.search,
    status: this.status,
    assignedUserId: this.currentUser?.role === 'Admin' ? this.assigneeId : this.currentUser?.id
  };

  this.taskSvc.filterTasks(filter).subscribe({
    next: t => { 
        this.tasks = this.currentUser?.role === 'Admin'
          ? t
          : t.filter(task => task.assignedUserId === this.currentUser?.id);
        this.loading = false;
     },
    error: err => { this.error = 'Failed to load tasks'; this.loading = false; console.error(err); }
  });
}
protected clearFilters() {
    this.search = '';
    this.status = undefined;
    this.assigneeId = '';
    this.load();
  }

protected delete(task: TaskItem) {
    if (!confirm(`Delete task "${task.title}"?`)) return;
    this.taskSvc.delete(task.id).subscribe({
      next: () => this.load()
    });
  }
}
