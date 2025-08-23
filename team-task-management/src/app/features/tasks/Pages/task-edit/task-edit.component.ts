import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Priority } from '../../../../core/enums/priority.enum';
import { TaskStatus } from '../../../../core/enums/task-status.enum';
import { User } from '../../../../core/models/user.model';
import { TaskService } from '../../../../core/services/task.service';
import { UserService } from '../../../../core/services/user.service';
import { TaskItem } from '../../../../core/models/task-item.model';


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
  debugger;
    this.userSvc.getAll().subscribe(u => this.users = u);
    this.id = this.route.snapshot.paramMap.get('id') || undefined;
    if (this.id) {
      this.taskSvc.getById(this.id).subscribe(task => this.form.patchValue({
         title: task.title,
    description: task.description,
    dueDate: this.formatDateTimeForInput(task.dueDate),
    status: task.status,
    priority: task.priority,
    assignedUserId: task.assignedUserId
      }));
    }
  }
private formatDateTimeForInput(dateStr?: string | Date): string {
  if (!dateStr) return '';
  const date = new Date(dateStr);
  return date.toISOString().slice(0, 16); // YYYY-MM-DDTHH:mm
}
 save() {
    debugger;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    //const payload = this.form.value as Partial<TaskItem>;
const payload: Partial<TaskItem> = {
  ...this.form.value,
  priority: Number(this.form.value.priority),
  status: Number(this.form.value.status),
   dueDate: this.form.value.dueDate ? new Date(this.form.value.dueDate).toISOString() : null
};
  try {
    if (this.id) {
       this.taskSvc.update(this.id, payload).subscribe({
    next: () => this.router.navigate(['/tasks']),
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
