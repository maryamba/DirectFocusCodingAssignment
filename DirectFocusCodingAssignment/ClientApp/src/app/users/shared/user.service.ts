import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import { User } from './user.model'

@Injectable()
export class UserService {
  selectedUser: User;
  userList: User[];
  constructor(private http: Http) { }

  postUser(emp: User) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Post, headers: headerOptions });
    return this.http.post('http://localhost:28750/api/User', body, requestOptions).map(x => x.json());
  }

  putUser(id, emp) {
    var body = JSON.stringify(emp);
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put('http://localhost:28750/api/User/' + id,
      body,
      requestOptions).map(res => res.json());
  }

  getUserList() {
    this.http.get('http://localhost:28750/api/User')
      .map((data: Response) => {
        return data.json() as User[];
      }).toPromise().then(x => {
        this.userList = x;
      })
  }

  deleteUser(id: number) {
    return this.http.delete('http://localhost:28750/api/User/' + id).map(res => res.json());
  }
}
