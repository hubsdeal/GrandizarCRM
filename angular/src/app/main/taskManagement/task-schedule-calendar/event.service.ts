import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()
export class EventService {
    public getEvents(): Observable<any> {
        const dateObj = new Date();
        const yearMonth = dateObj.getUTCFullYear() + '-' + (dateObj.getUTCMonth() + 1);
        const data: any = [
            {
                title: 'All Day Event',
                start: yearMonth + '-01',
                DoctorsCount: 5,
                TotalAppointments: 20,
                Booked: 10,
                Cancelled: 2
            },
            {
                title: 'All Day Event',
                start: yearMonth + '-02',
                DoctorsCount: 8,
                TotalAppointments: 20,
                Booked: 10,
                Cancelled: 2
            },
            {
                title: 'All Day Event',
                start: yearMonth + '-03',
                DoctorsCount: 40,
                TotalAppointments: 20,
                Booked: 10,
                Cancelled: 2
            },
            {
                title: 'All Day Event',
                start: yearMonth + '-05',
                DoctorsCount: 40,
                TotalAppointments: 20,
                Booked: 10,
                Cancelled: 2
            }
        ];
        return of(data);
    }
}
