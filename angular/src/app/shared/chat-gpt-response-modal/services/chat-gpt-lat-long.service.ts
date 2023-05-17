import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ResponseModel } from '../models/gpt-response';
import { Configuration, OpenAIApi } from 'openai';

@Injectable({
    providedIn: 'root'
})
export class GeocodingService {
    response!: ResponseModel | undefined;

    constructor(private http: HttpClient) { }

    async invokeGPT(promptText: string) {
        if (promptText.length < 2)
          return;
        try {
          this.response = undefined;
          let configuration: Configuration;
          configuration = new Configuration({ apiKey: environment.openAIapiKey });
          let openai = new OpenAIApi(configuration);
    
          let requestData = {
            model: 'text-davinci-003',//'text-davinci-003',//"text-curie-001",
            prompt: promptText,//this.generatePrompt(animal),
            temperature: 0.95,
            max_tokens: 150,
            top_p: 1.0,
            frequency_penalty: 0.0,
            presence_penalty: 0.0,
          };
          let apiResponse = await openai.createCompletion(requestData);
          this.response = apiResponse.data as ResponseModel;
          const lines = this.response.choices[0].text.split('\n');
          const recordFormat = lines.map(line => `<br> ${line}`).join('\n');
          console.log(recordFormat);
          const responseText = this.extractCoordinates(recordFormat);
          console.log(responseText);
          return responseText;
        } catch (error: any) {
          if (error.response) {
            console.error(error.response.status, error.response.data);
          } else {
            console.error(`Error with OpenAI API request: ${error.message}`);
    
          }
        }
      }


    private extractCoordinates(responseText: string): { latitude: number, longitude: number } {
        // Remove HTML tags and leading/trailing white spaces from the response text
        const cleanText = responseText.replace(/<[^>]+>/g, '').trim();
      
        // Extract the latitude and longitude using regular expressions
        const regex = /(\d+\.\d+)° [NSEW], (\d+\.\d+)° [NSEW]/;
        const matches = cleanText.match(regex);
      
        if (matches && matches.length >= 3) {
          const latitude = +matches[1]; // Convert string to number
          const longitude = +matches[2]; // Convert string to number
          return { latitude, longitude };
        }
      
        // If the response text cannot be parsed or does not contain the coordinates, return null or throw an error
        throw new Error('Unable to extract coordinates from the response');
      }
      
}
