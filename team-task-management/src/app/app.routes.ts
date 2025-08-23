import { Routes } from '@angular/router';
import { ChatPanelComponent } from './features/chat/components/chat-panel/chat-panel.component';
import { AppComponent } from './app.component';

export const routes: Routes = [
  { path: '', 
    component: AppComponent },
  {
    path: 'chat',
    component: ChatPanelComponent,
  },
  {
    path: '**',
    redirectTo: ''
  }
];
