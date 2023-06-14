export interface OnlineEvent{
    id : number;
    type: number;
    name: string;
    description: string;
    date: Date;
    time: string;
    aboutEvent: string;
    photo: File | null;
    speakers: number[] | null;
    platform: number;
    link: string | null;
    meetingId: string | null;
    password: string | null;
}