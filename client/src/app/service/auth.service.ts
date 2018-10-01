import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../product';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  error=''
  baseUrl:string="http://localhost:5000/users"
  constructor(private http:HttpClient) { }
  login(username:string,password:string){
    return this.http.post<any>(this.baseUrl+ '/authenticate',{ username, password })
    .pipe(map(user => {
      if (user && user.token) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', user.firstName);
          localStorage.setItem('roles', user.roles);
          localStorage.setItem('id', user.id);
      }
      return user;
  },
      error => {
          this.error = error;
      }))
  }
  getUserById(){
    var id=this.getUserId();
    var user= this.http.get<User>(this.baseUrl+'/GetUserById/'+id)
    return user;
  }
  getToken(): string {
    return localStorage.getItem('token');
  }
  getUserId(): string {
    return localStorage.getItem('id');
  }  
  loggedIn(){
    return !!localStorage.getItem('token')
  }
  logout(){
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }
  register(firstname:string,lastname:string, username:string,password:string){
    return this.http.post<any>(this.baseUrl+ '/register',{ firstname,lastname, username, password })
    }
  }
