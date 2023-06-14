import { Speaker } from "src/app/adminpanel/speaker/speaker.model";

export interface GetEvent{
    id : number;
    name : string;
    description : string;
    date : Date;
    time : Date;
    aboutEvent : string;
    placeEvent : string;
    speakers : Speaker[];
}