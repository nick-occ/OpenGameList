﻿import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms"; 
import { Router } from "@angular/router"; 
import { AuthService } from "./auth.service";
import { UserEditComponent } from "./user-edit.component";

@Component({
    selector: "login",
    template: ` 
       <div class="login-container">
            <h2 class="form-login-heading">Login</h2>
            <div class="alert alert-danger" role="alert" *ngIf="loginError">
                <strong>Warning:</strong> Username or Password mismatch
            </div> 
            <form class="form-login" [formGroup]="loginForm" (submit)="performLogin($event)">
                <input formControlName="username" type="text" class="form-control" placeholder="Your username or e-mail address" required autofocus /> 
                <input formControlName="password" type="password" class="form-control" placeholder="Your password" required /> 
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="remember-me">
                        Remember Me
                    </label>
                </div>
                <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
            </form>
            <div class="register-link">
                Don't have an account yet?
                <a (click)="onRegister()">Click here to register!</a>
            </div>
            <button class="btn btn-sm btn-default btn-block" type="submit" (click)="callExternalLogin('Google')"> 
                Login with Google 
            </button> 
        </div>
    `
})

export class LoginComponent {
    title = "Login"
    loginForm = null;
    loginError = false;
    externalProviderWindow = null; 

    constructor(private fb: FormBuilder,
        private router: Router,
        private authService: AuthService) {
        /*
        if (this.authService.isLoggedIn()) {
            this.router.navigate([""]);
        }
        */

        this.loginForm = fb.group({
            username: ["", Validators.required],
            password: ["", Validators.required]
        });
    }

    performLogin(e) {
        e.preventDefault();
        var username = this.loginForm.value.username;
        var password = this.loginForm.value.password;
        this.authService.login(username, password)
            .subscribe((data) => {
                //login successful
                this.loginError = false;
                var auth = this.authService.getAuth();
                alert("Our Token is: " + auth.access_token);
                this.router.navigate([""]);
            }, (err) => {
                console.log(err);
                //login failure
                this.loginError = true;
            });
    }

    onRegister() {
        this.router.navigate(["register"]);
    }

    callExternalLogin(providerName: string) {
        var url = "api/Accounts/ExternalLogin/" + providerName;
        //minimalistic mobile devices support
        var w = (screen.width >= 1050) ? 1050 : screen.width;
        var h = (screen.height >= 550) ? 550 : screen.height;

        var params = "toolbar=yes,scrollbars=yes,resizable=yes,width=" + w + ", height=" + h;

        //closed previously opened windows (if any)
        if (this.externalProviderWindow) {
            this.externalProviderWindow.close();
        }
        this.externalProviderWindow = window.open(url, "ExternalProvider", params, false);
    }
} 