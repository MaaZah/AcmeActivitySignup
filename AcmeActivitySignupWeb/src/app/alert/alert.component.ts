import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertService } from '../_shared/services/alert.service';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
//Alert component to go along with alertService
export class AlertComponent implements OnInit {

  private subscription: Subscription;
  message: any;

  constructor(private _alertService: AlertService) { }

  ngOnInit() {
    this.subscription = this._alertService.getMessage().subscribe(message => {
      this.message = message;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
