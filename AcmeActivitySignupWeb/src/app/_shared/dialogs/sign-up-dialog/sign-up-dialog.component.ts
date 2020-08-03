import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService } from '../../services/api.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivityDetails } from '../../models/activity-details';
import { AttendeeInformation } from '../../models/attendee-information';
import { AlertService } from '../../services/alert.service';

@Component({
  selector: 'app-sign-up-dialog',
  templateUrl: './sign-up-dialog.component.html',
  styleUrls: ['./sign-up-dialog.component.scss']
})
export class SignUpDialogComponent implements OnInit {

  signupForm: FormGroup;
  selectedActivity: ActivityDetails;
  activities: ActivityDetails[];

  //track state of form
  submitted = false;
  success = false;
  disableSubmit = false;


  constructor(public dialogRef: MatDialogRef<SignUpDialogComponent>, public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any, private _formBuilder: FormBuilder, private _apiService: ApiService, private _alertService: AlertService) { }

  ngOnInit(): void {
    //get activities list for form's select
    this._apiService.GetActivitiesList().subscribe(res => {
      this.activities = res;

      //set pre-selected activity 
      if (this.data.activityId != undefined) {
        this.selectedActivity = this.activities.find(x => x.activityId == this.data.activityId);
      }
      this.buildForm();
    })
  }


  //build the form, set defaults/validators
  buildForm() {
    this.signupForm = this._formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      comments: [''],
      activity: [this.selectedActivity, Validators.required]
    });
  }

  //Easy access to form controls
  get f() {
    return this.signupForm.controls;
  }

  OnSubmit() {
    this.submitted = true;


    //check errors
    if (this.signupForm.invalid) {
      this.success = false;
      return;
    }


    this.disableSubmit = true;

    //build request params
    let attendee = new AttendeeInformation();
    attendee.firstName = this.f.firstName.value;
    attendee.lastName = this.f.lastName.value;
    attendee.email = this.f.email.value;
    attendee.comments = this.f.comments.value;

    this.selectedActivity = this.f.activity.value;
    let activityId = this.selectedActivity.activityId;

    //call API
    this._apiService.AttendActivity(activityId, attendee).subscribe(res => {
      //Success!
      this.success = true;
      this._alertService.success("Thanks for signing up!")
    },
    err => {
      //Uh oh!
      this.success = false;
      this.submitted = false;
      this.disableSubmit = false;
      this._alertService.error(err.error.error);
    });
  }
}
