import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/login/login-page/login.component';
import { TaskListComponent } from './features/tasks/Pages/task-list/task-list.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'tasks', component: TaskListComponent } 
  //{ path: '', redirectTo: 'tasks', pathMatch: 'full' },
  // TasksModule already declares its own child routes in forChild
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
