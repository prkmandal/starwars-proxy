import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalCommunicationService {

  constructor() { }


  private starShipIds = new BehaviorSubject<any>('');
  private filmIds = new BehaviorSubject<any>('');

  setStarShipIds(data: any): void {
    this.starShipIds.next(data);
  }

  getStarShipIds(): BehaviorSubject<any> {
    return this.starShipIds;
  }

  // private modalDataSubject = new BehaviorSubject<any>(null);
  // modalData$ = this.modalDataSubject.asObservable();

  // openModalWithData(data: any) {
  //   console.log("service call data");
  //   this.modalDataSubject.next(data);
  // }
}
