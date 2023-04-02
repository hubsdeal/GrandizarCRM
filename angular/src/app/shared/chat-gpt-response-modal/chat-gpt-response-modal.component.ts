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
  // jobaarFg: FormGroup = this.fb.group({

  // });
  promptText: any;
  /*editor: Editor;*/
  // toolbar: Toolbar = [
  //   ['bold', 'italic'],
  //   ['underline', 'strike'],
  //   ['code', 'blockquote'],
  //   ['ordered_list', 'bullet_list'],
  //   [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
  //   ['image'],
  //   ['text_color', 'background_color'],
  //   ['align_left', 'align_center', 'align_right', 'align_justify'],
  // ];
  // editor: Editor;
  responseText: any;

  constructor(
    public dialogRef: MatDialogRef<ChatGptResponseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { promtFromAnotherComponent: string }) { }

  ngOnInit(): void {
    // this.editor = new Editor();
    console.log(this.data.promtFromAnotherComponent)
    if (this.data.promtFromAnotherComponent) {
      this.promptText = this.data.promtFromAnotherComponent;
    }
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
      this.showSpinner = true;
      let apiResponse = await openai.createCompletion(requestData);
      this.response = apiResponse.data as ResponseModel;
      const lines = this.response.choices[0].text.split('\n');
      const recordFormat = lines.map(line => `<br> ${line}`).join('\n');
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
