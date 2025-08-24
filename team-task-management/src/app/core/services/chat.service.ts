import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject, BehaviorSubject } from 'rxjs';
import { ChatMessage } from '../models/chat-message.model';
import { environment } from '../../../environments/environment.development';
import { CurrentUserService } from './current-user.service';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;

  private readonly _connected$ = new BehaviorSubject<boolean>(false);
  connected$ = this._connected$.asObservable();

  private readonly _messages$ = new Subject<ChatMessage>();
  messages$ = this._messages$.asObservable();

  constructor(private currentUserService: CurrentUserService, private http: HttpClient) {}

  get currentUser() {
    const user = this.currentUserService.currentUser;
    if (!user) throw new Error('No current user set');
    return user;
  }

  start(): void {
    if (this.hub) return;

    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubUrl, { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ReceiveMessage', (msg: ChatMessage) => {
      const user = this.currentUserService.currentUser;
      if (!user) return;

      if (user.role === 'Admin' || msg.senderId === user.id) {
        this._messages$.next(msg);
      }
    });

    this.hub.start()
      .then(() => 
        {
          this._connected$.next(true);
          console.log('SignalR connected');
        })
      .catch(err => console.error('SignalR start error:', err));
  }

  sendMessage(content: string): Promise<void> {
    const user = this.currentUser;
    if (!this.hub) throw new Error('Hub not started');

    return this.hub.invoke('SendMessage', user.id, content);
  }

   fetchMessages(userId: string) {
    return this.http.get<ChatMessage[]>(`${environment.apiBaseUrl}/${userId}/GetMessages`);
  }
}
