import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment'
import { JwtService } from './jwt.service';

/*
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.api_url;

  constructor(private http: HttpClient) { }

  public get(endpoint: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${endpoint}`);
  }

  public post(endpoint: string, data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/${endpoint}`, data);
  }

  public put(endpoint: string, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${endpoint}`, data);
  }

  public delete(endpoint: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${endpoint}`);
  }
}
*/

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.api_url;

  constructor(private http: HttpClient, private jwtService: JwtService) { }

  public get(endpoint: string): Observable<any> {
    const options = this.getRequestOptions();
    return this.http.get(`${this.baseUrl}/${endpoint}`, options);
  }

  public post(endpoint: string, data: any): Observable<any> {
    const options = this.getRequestOptions();
    return this.http.post(`${this.baseUrl}/${endpoint}`, data, options);
  }

  public put(endpoint: string, data: any): Observable<any> {
    const options = this.getRequestOptions();
    return this.http.put(`${this.baseUrl}/${endpoint}`, data, options);
  }

  public delete(endpoint: string): Observable<any> {
    const options = this.getRequestOptions();
    return this.http.delete(`${this.baseUrl}/${endpoint}`, options);
  }

  private getRequestOptions(): { headers: HttpHeaders } {
    const token = this.jwtService.getToken();
    let headers = new HttpHeaders();
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return { headers };
  }
}
