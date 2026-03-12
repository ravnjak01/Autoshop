import {Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {MyConfig} from '../../../my-config';
import {LoginTokenDto} from '../../../modules/shared/dtos/login-token-dto';

@Injectable({providedIn: 'root'})
export class MySignalRService {
  private hubConnection!: signalR.HubConnection;

  // Pokretanje SignalR konekcije
 startConnection() {
    const authToken = localStorage.getItem('jwtToken');
    
    if (!authToken) {
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${MyConfig.api_address}/mysignalr-hub-path?access_token=${authToken}`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
}
  // Zaustavljanje SignalR konekcije
  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
    }
  }

  // Dodavanje listenera za primanje poruka
  myClientMethod1(callback: (message: string) => void) {
    this.hubConnection.on('myClientMethod1', (data: string) => {
      callback(data);
    });
  }

  // Slanje poruke serveru
  myServerHubMethod1(toUser: string, message: string) {
    this.hubConnection
      .invoke('MyServerHubMethod1', toUser, message)
  }
}
