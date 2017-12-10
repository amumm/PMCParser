import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    public http: Http;

    public baseUrl: string;

    public exportToExcel: boolean;

    public results: Array<JournalPaper>;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;
        this.exportToExcel = true;
        
    }

    analyzeArticles() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/AnalyzeArticles').subscribe(result => {
            this.results = result.json() as Array<JournalPaper>;
        }, error => console.error(error));
    }

    exportData() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/ExportData').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }


}

export class DataType {

        Name: string;

        keywords: Array<String>;

        constructor(Name: string){
            this.Name = Name;
        }
}

export class JournalPaper {

    JournalTitle: string;

    PaperTitle: string;

    Issue: string;

    Volume: string;

    Date: string;

    PMCID: string;

    Authors: Array<String>;

    CorrespondingAuthors: Array<String>;

    dataTypes: Array<DataType>;

    public JournalPaper() {

    }
}

