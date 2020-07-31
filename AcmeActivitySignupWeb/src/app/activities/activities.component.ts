import { Component, OnInit } from '@angular/core';
import { ActivityDetails } from '../_shared/models/activity-details';
import { ApiService } from '../_shared/services/api.service';
import { MatDialog } from '@angular/material/dialog';
import { SignUpDialogComponent } from '../_shared/dialogs/sign-up-dialog/sign-up-dialog.component';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
})
export class ActivitiesComponent implements OnInit {

  activities: ActivityDetails[];

  constructor(private _apiService: ApiService, public dialog: MatDialog) { }

  ngOnInit(): void {
    //get list of activities
    this._apiService.GetActivitiesList().subscribe(res => {
      this.activities = res;
    });
  }

  //open the sign up dialog, and pass activityId
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
