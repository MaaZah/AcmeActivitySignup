import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivityDetails } from '../models/activity-details';
import { API_URL } from '../../../constants';
import { AttendeeInformation } from '../models/attendee-information';

@Injectable({
  providedIn: 'root'
})
//Call all API endpoints from here
export class ApiService {

  constructor(private _http: HttpClient) { }


  GetActivitiesList(){
    return this._http.get<ActivityDetails[]>(`${API_URL}/activities`);
  }

  GetActivityDetails(activityId: number){
    return this._http.get<ActivityDetails>(`${API_URL}/activities/${activityId}`);
  }

  AttendActivity(activityId: number, attendee: AttendeeInformation){
    return this._http.post<boolean>(`${API_URL}/activities/attend?activityId=${activityId}`, attendee);
  }
}
