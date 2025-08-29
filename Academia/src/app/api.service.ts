import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Student { id?: number; name: string; age: number; document: string; licenseType: string; enrollments?: any[] }

@Injectable()
export class ApiService {
  private base = 'https://localhost:7080/api'; // Adjust if backend runs on different port

  constructor(private http: HttpClient) {}

  getStudents(): Observable<Student[]> { return this.http.get<Student[]>(`${this.base}/students`); }
  createStudent(s: Student) { return this.http.post<Student>(`${this.base}/students`, s); }
  getModules() { return this.http.get<any[]>(`${this.base}/modules`); }
  getClasses() { return this.http.get<any[]>(`${this.base}/classes`); }
  enroll(payload: { studentId:number, classId:number }) { return this.http.post(`${this.base}/enrollments`, payload); }
}
