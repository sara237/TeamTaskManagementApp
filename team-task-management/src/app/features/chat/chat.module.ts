import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatPanelComponent } from './components/chat-panel/chat-panel.component';

@NgModule({
  declarations: [ChatPanelComponent],
  imports: [CommonModule, FormsModule],
  exports: [ChatPanelComponent]
})
export class ChatModule {}
