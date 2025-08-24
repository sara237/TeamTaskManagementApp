import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Priority } from '../../../../core/enums/priority.enum';
import { TaskStatus } from '../../../../core/enums/task-status.enum';
import { User } from '../../../../core/models/user.model';
import { TaskService } from '../../../../core/services/task.service';
import { UserService } from '../../../../core/services/user.service';
import { TaskItem } from '../../../../core/models/task-item.model';
import { firstValueFrom, tap } from 'rxjs';


@Component({
  selector: 'app-task-edit',
  standalone: false,
  templateUrl: './task-edit.component.html',
  styleUrls: ['./task-edit.component.scss']
})
export class TaskEditComponent implements OnInit {
  id?: string;
  users: User[] = [];
  TaskStatus = TaskStatus;
  Priority = Priority;
  form! : FormGroup;
  tasks: TaskItem[] = [];
  error: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private taskSvc: TaskService,
    private userSvc: UserService
  ) {}

  ngOnInit(): void {

    this.form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]],
    status: [TaskStatus.Todo, [Validators.required]],
    priority: [Priority.Medium, [Validators.required]],
    assignedUserId: ['', [Validators.required]],
    dueDate: ['']
  });
    this.userSvc.getAll().subscribe(u => this.users = u);

    this.id = this.route.snapshot.paramMap.get('id') || undefined;

    if (this.id) {
      this.taskSvc.getById(this.id).subscribe(task => 
        this.form.patchValue({
                              title: task.title,
                              description: task.description,
                              dueDate: [
                                        task?.dueDate ? this.formatDateForInput(task.dueDate) : ''
                                       ],
                              status: task.status,
                              priority: task.priority,
                              assignedUserId: task.assignedUserId
                             }));
     }
  }
private formatDateForInput(date: string | Date): string {
  const d = new Date(date);
  const pad = (n: number) => n.toString().padStart(2, '0');
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

private async loadTasks() {
  try {
    const tasks = await firstValueFrom(this.taskSvc.getAll());
    this.tasks = tasks;  // ✅ tasks assigned here
  } catch (err) {
    console.error('Failed to load tasks', err);
  }
}
 private async checkDuplicateTask(task: Partial<TaskItem>)
 {
    await this.loadTasks();
    if (this.tasks.some(t =>
      t.title === task.title &&
      t.description === task.description &&
      t.status === task.status &&
      t.priority === task.priority &&
      t.assignedUserId === task.assignedUserId
    )) {
      this.error = 'A task with the same title, status, priority, description, and assigned user already exists.';
      return true;
    }
    return false;
 }
 protected async save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const payload: Partial<TaskItem> = {
                  ...this.form.value,
                  priority: Number(this.form.value.priority),
                  status: Number(this.form.value.status),
                  dueDate: this.form.value.dueDate ? new Date(this.form.value.dueDate) : null
    };

  try {
   const isDuplicate = await this.checkDuplicateTask(payload);
    if (isDuplicate) {
      return; // 🚫 stop if duplicate found
    }

    if (this.id) {
       this.taskSvc.update(this.id, payload).subscribe({
    next: () => {   
       this.router.navigate(['/tasks'])
    },
    error: err => console.error('Update failed', err)
  });
    } else {
       this.taskSvc.create(payload).subscribe({
    next: () => this.router.navigate(['/tasks']),
    error: err => console.error('Create failed', err)
  });
    }
    this.router.navigate(['/tasks']);
  } catch (err) {
    console.error('Save failed', err);
  }

  }
}
