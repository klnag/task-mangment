import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { GlobalVariblesService } from '../store/global-varibles.service';
import { ProjectsPageComponent } from '../pages/projects-page/projects-page.component';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loggedIn = localStorage.getItem("user") !== "" && this.cookieService.get("token") !== "";
  private isLoggedin = new BehaviorSubject<boolean>(localStorage.getItem("user") !== "" && this.cookieService.get("token") !== "")

  constructor(private location: Location,private http: HttpClient, private cookieService: CookieService, private router: Router, private store: GlobalVariblesService) {}

  getUserInfo() {
    // if (!localStorage.getItem("user")) {
    //console.log(this.cookieService.get("token"))
      this.http.get('http://localhost:5242/api/User', {
        headers: { "Authorization": "Bearer " + this.cookieService.get("token") }, responseType: "json"
      }
      ).subscribe((res: any) => {
        localStorage.setItem("user", JSON.stringify({ user: res.data }))
        this.loggedIn = true;
        this.isLoggedin.next(true)
        // this.location.go('/projects')
        // window.location.reload()
        if(this.router.url === "/login" || this.router.url === "/singup") {

        this.router.navigate(['projects'])
        }
      }, err => {
          //console.log(err)
        this.cookieService.delete("token")
        localStorage.removeItem("user")
        localStorage.removeItem("project")
        this.isLoggedin.next(false)
        this.loggedIn = false;
        this.router.navigate(["login"])     
      })
    // }
  }

  singup(username: string,email: string, password: string) {
    return this.http.post("http://localhost:5242/api/User", {
      "username": username,
      "email": email,
      "password": password 
    })
  }
  
  login(email: string, password: string) {
    //console.log(email)
    return this.http.post("http://localhost:5242/api/User/login", {
      "username": "string",
      "email": email,
      "password": password 
    })
  }

  logout() {
    this.cookieService.delete("token")
    localStorage.removeItem("user")
    localStorage.removeItem("project")
    this.isLoggedin.next(false)
    this.loggedIn = false;
    this.router.navigate(["login"])
  }

  isLoggedIn() {
    return this.loggedIn;
  }
isLoggedIns() {
    return this.isLoggedin.asObservable();
  }
}
