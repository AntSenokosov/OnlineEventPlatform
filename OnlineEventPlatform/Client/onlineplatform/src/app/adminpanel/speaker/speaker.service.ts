import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ApiService } from 'src/app/core/services/api.service';
import { Speaker } from './speaker.model';

@Injectable()
export class SpeakerService {
  private readonly endpoint = 'speakers';

  constructor(private apiService: ApiService) {}

  getSpeakers(): Observable<Speaker[]> {
    return this.apiService.get(this.endpoint).pipe(
      map((response: any) => response.items as Speaker[])
    );
  }

  getSpeaker(id: number): Observable<Speaker> {
    return this.apiService.get(`${this.endpoint}/${id}`);
  }

  addSpeaker(speaker: Speaker): Observable<Speaker> {
    return this.apiService.post(`${this.endpoint}/create`, speaker);
  }

  updateSpeaker(id:number, speaker: Speaker): Observable<Speaker> {
    return this.apiService.put(`${this.endpoint}/${id}/update`, speaker);
  }

  deleteSpeaker(id: number): Observable<any> {
    return this.apiService.delete(`${this.endpoint}/${id}/remove`);
  }
}
