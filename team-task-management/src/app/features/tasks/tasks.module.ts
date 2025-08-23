import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { TaskEditComponent } from './Pages/task-edit/task-edit.component';
import { TaskListComponent } from './Pages/task-list/task-list.component';


const routes: Routes = [
  { path: 'tasks', component: TaskListComponent },
  { path: 'tasks/new', component: TaskEditComponent },
  { path: 'tasks/:id', component: TaskEditComponent }
];

@NgModule({
  declarations: [TaskListComponent, TaskEditComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule.forChild(routes)],
})
export class TasksModule {}
