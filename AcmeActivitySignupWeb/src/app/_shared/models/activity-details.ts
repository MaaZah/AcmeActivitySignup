import {AttendeeInformation} from './attendee-information';

//api response model for activity details
export class ActivityDetails {
    activityId: number;
    title: string;
    description: string;
    date: Date;
    attendees: AttendeeInformation[];
    imageUrl: string;

    constructor(activityId: number,
        title: string,
        description: string,
        date: Date,
        imageUrl: string){

        this.activityId = activityId;
        this.title = title;
        this.description = description;
        this.date = date;
        this.imageUrl = imageUrl

    }
}

