import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    public http: Http;

    public baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;
    }

    createList() {
        console.log("here");
        this.http.get(this.baseUrl + 'api/PMCController/CreateList').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }

    downloadArticles() {
        this.http.get(this.baseUrl + 'api/PMCController/DownloadArticles').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }

    analyzeArticles() {
        this.http.get(this.baseUrl + 'api/PMCController/AnalyzeArticles').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }

    exportData() {
        this.http.get(this.baseUrl + 'api/PMCController/ExportData').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }
}
