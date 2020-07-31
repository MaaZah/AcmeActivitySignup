import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../_shared/services/api.service';
import { ActivityDetails } from '../_shared/models/activity-details';
import { MatTableDataSource } from '@angular/material/table';
import { AttendeeInformation } from '../_shared/models/attendee-information';
import { MatDialog } from '@angular/material/dialog';
import { SignUpDialogComponent } from '../_shared/dialogs/sign-up-dialog/sign-up-dialog.component';

@Component({
  selector: 'app-activity-details',
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.scss']
})
export class ActivityDetailsComponent implements OnInit {

  private routeSub: Subscription;
  activityDetails: ActivityDetails;
  
  //initializations for the mat-table
  dataSource: MatTableDataSource<AttendeeInformation>;
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'comments'];


  constructor(private _location: Location, private _route: ActivatedRoute, private _apiService: ApiService, public dialog: MatDialog) { }

  ngOnInit(): void {
    //get the activityId from the route
    this.routeSub = this._route.params.subscribe(params => {
      console.log(params["activityId"]);
      //get the activity details
      this._apiService.GetActivityDetails(params["activityId"]).subscribe(res => {
        this.activityDetails = res;
        this.dataSource = new MatTableDataSource<AttendeeInformation>(this.activityDetails.attendees);
        console.log(this.activityDetails);
      });
    });
  }

  //back button funciton
  BackClicked(){
    this._location.back();
  }

  //open sign up dialog
  OpenSignUp(activityId: number){
    console.log(activityId);
    const dialogRef = this.dialog.open(SignUpDialogComponent, {
      width: '60%',
      data: {
        activityId: activityId
      }
    });
  }

}
