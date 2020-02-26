import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UserInfo } from '../product';
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
          localStorage.setItem('username', user.username);
          localStorage.setItem('user', user.firstName+' '+user.lastName);
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
    var user= this.http.get<User>(this.baseUrl+'/'+id)
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
    localStorage.removeItem('username')
    localStorage.removeItem('roles')
    localStorage.removeItem('id')
  }
  register(firstname:string,lastname:string, username:string,password:string){
    return this.http.post<any>(this.baseUrl+ '/register',{ firstname,lastname, username, password })
    }
  update(user:UserInfo){
    localStorage.removeItem('user')
    localStorage.setItem('user', user.firstname+' '+user.lastname);
    return this.http.put(this.baseUrl+'/'+user.Id,user);
  }
}
