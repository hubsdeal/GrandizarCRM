import { Component, Inject, OnInit } from '@angular/core';
import { ResponseModel } from './models/gpt-response';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Configuration, OpenAIApi } from 'openai';
import { environment } from 'environments/environment';

@Component({
  selector: 'app-chat-gpt-response-modal',
  templateUrl: './chat-gpt-response-modal.component.html',
  styleUrls: ['./chat-gpt-response-modal.component.scss']
})
export class ChatGptResponseModalComponent implements OnInit {

  response!: ResponseModel | undefined;
  showSpinner = false;
  promptText: any;
  responseText: any;
  feildName: any;
  modalTitle: any;
  constructor(
    public dialogRef: MatDialogRef<ChatGptResponseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { promtFromAnotherComponent: string, feildName: string, modalTitle: string }) {
    if (this.data.promtFromAnotherComponent) {
      this.promptText = this.data.promtFromAnotherComponent;
    }
    if (this.data.feildName) {
      this.feildName = this.data.feildName
    }
    if (this.data.modalTitle) {
      this.modalTitle = this.data.modalTitle
    }
  }

  ngOnInit(): void {
    // this.editor = new Editor();

  }

  checkResponse() {
    const promptText: string = this.promptText;
    this.invokeGPT(promptText);
  }

  getText(data: string) {
    return data.split('\n').filter(f => f.length > 0);
  }

  async invokeGPT(promptText: string) {
    if (promptText.length < 2)
      return;
    try {

      // this.response = undefined;
      // const configuration = new Configuration({
      //   apiKey: environment.openAIapiKey,
      //   organization: environment.OpenAIOrgId,
      // });
      // configuration.baseOptions.headers = {
      //   Authorization: "Bearer " + configuration.apiKey,
      // };
      // let openai = new OpenAIApi(configuration);


      this.response = undefined;
      let configuration: Configuration;
      configuration = new Configuration({ apiKey: environment.openAIapiKey });
      delete configuration.baseOptions.headers['User-Agent'];
      let openai = new OpenAIApi(configuration);

      let requestData = {
        model: 'text-davinci-003',
        prompt: promptText,
        temperature: 0.95,
        max_tokens: 1500,
        top_p: 1.0,
        frequency_penalty: 0.0,
        presence_penalty: 0.0,
      };
      this.showSpinner = true;
      let apiResponse = await openai.createCompletion(requestData);
      this.response = apiResponse.data as ResponseModel;
      // const lines = this.response.choices[0].text.split('\n');
      // const recordFormat = lines.map(line => `<br> ${line}`).join('\n');
      const lines = this.response.choices[0].text.trim().split('\n');
      const recordFormat = lines.map(line => `<br> ${line}`).join('\n').trim();
      this.responseText = recordFormat;


      this.responseText = recordFormat
      this.showSpinner = false;
    } catch (error: any) {
      this.showSpinner = false;
      // Consider adjusting the error handling logic for your use case
      if (error.response) {
        console.error(error.response.status, error.response.data);
      } else {
        console.error(`Error with OpenAI API request: ${error.message}`);

      }
    }
  }

  onInsert(): void {
    this.dialogRef.close({ data: this.responseText });
  }

}
