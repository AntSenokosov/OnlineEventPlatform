import { Component, OnInit } from '@angular/core';
import { OnlineEvent } from '../adminpanel/onlineevent/onlineevent.model';
import { CatalogService } from './catalog.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent implements OnInit {
  onlineEvents: OnlineEvent[] = [];

  constructor(private catalogService: CatalogService, private router : Router) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.catalogService.getCatalog().subscribe(
      (events: OnlineEvent[]) => {
        this.onlineEvents = events;
      },
      (error: any) => {
        console.error("Failed to load events", error);
      }
    );
  }

  onEventClicked(eventId: number): void {
    this.router.navigate(['/event', eventId]);
  }

  splitText(text: string, maxLength: number): string {
    if (text.length > maxLength) {
      const words = text.split(' ');
      let result = '';
      let currentLineLength = 0;
  
      for (let i = 0; i < words.length; i++) {
        const word = words[i];
        currentLineLength += word.length;
  
        if (currentLineLength <= maxLength) {
          result += word + ' ';
          currentLineLength++; // +1 for the space between words
        } else {
          result += '<br>' + word + ' ';
          currentLineLength = word.length + 1; // +1 for the space before the next word
        }
      }
  
      return result.trim(); // Remove trailing space
    }
  
    return text;
  }
}
