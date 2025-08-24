import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject, BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { ChatMessage } from '../models/chat-message.model';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;
  private readonly _connected$ = new BehaviorSubject<boolean>(false);
  connected$ = this._connected$.asObservable();

  private readonly _messages$ = new Subject<ChatMessage>();
  messages$ = this._messages$.asObservable();

  start(): void {
    if (this.hub) return;

    this.hub = new signalR.HubConnectionBuilder()
    .withUrl(environment.hubUrl, { withCredentials: true })
    .withAutomaticReconnect()
    .build();


    this.hub.on('ReceiveMessage', (senderId: string, message: string, senderName: string, timestamp: string) => {
      const msg: ChatMessage = {
        id: crypto.randomUUID(),
        senderId,  
        senderName, 
        content: message,
        timestamp
      };
      this._messages$.next(msg);
    });

    this.hub
      .start()
      .then(() => this._connected$.next(true))
      .catch(err => console.error('SignalR start error', err));
  }

  sendMessage(sender: string, message: string): Promise<void> {
    if (!this.hub) throw new Error('Hub not started');
    return this.hub.invoke('SendMessage', sender, message);
  }
}
