import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { Router, NavigationStart } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
//generic alert service, typically used for api errors/success messages
export class AlertService {

  private _subject = new Subject<any>();

  private _keepAfterNavigationChange = false;

  constructor(private _router: Router) { 
    //clear alert message on route change
    _router.events.subscribe(event => {
      if(event instanceof NavigationStart){
        if(this._keepAfterNavigationChange){
          //only keep for a single navigation change
          this._keepAfterNavigationChange = false;
        }
        else{
          //clear alert
          this._subject.next();
        }
      }
    });
  }

  success(message: string, keepAfterNavigationChange = false){
    this._keepAfterNavigationChange = keepAfterNavigationChange;
    this._subject.next({type: 'success', text: message});
  }

  error(message: string, keepAfterNavigationChange = false){
    this._keepAfterNavigationChange = keepAfterNavigationChange;
    this._subject.next({type: 'error', text: message});
  }

  clear(){
    this._subject.next();
  }

  getMessage(): Observable<any>{
    return this._subject.asObservable();
  }
}
