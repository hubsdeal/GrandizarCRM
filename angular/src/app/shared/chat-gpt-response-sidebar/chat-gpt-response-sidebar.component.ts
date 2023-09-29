import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ResponseModel } from '../chat-gpt-response-modal/models/gpt-response';
import { Configuration, OpenAIApi } from 'openai';
import { environment } from 'environments/environment';

@Component({
  selector: 'app-chat-gpt-response-sidebar',
  templateUrl: './chat-gpt-response-sidebar.component.html',
  styleUrls: ['./chat-gpt-response-sidebar.component.scss']
})
export class ChatGptResponseSidebarComponent extends AppComponentBase implements OnInit {

  response!: ResponseModel | undefined;
  showSpinner = false;
  @Input() sidebarVisible: boolean;
  @Input() taskName: any;
  @Input() promt: any;
  @Input() fieldName: any;
  @Input() modalTitle: any;
  responseText: any;
  @Output() sidebarVisibleChange = new EventEmitter<boolean>();


  @Output() responseTextInserted = new EventEmitter<string>();
  constructor(
    injector: Injector,
  ) {
    super(injector);
  }
  ngOnInit(): void {
    // this.editor = new Editor();

  }
  checkResponse() {
    const promptText: string = this.promt;
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
        this.notify.error(`Error with ChatGpt Request, Please contact our customer support with this data: ${error.response.status}, ${error.response.data}`);
      } else {
        console.error(`Error with OpenAI API request: ${error.message}`);
        this.notify.error(`Error with ChatGpt Request, Please contact our customer support with this data: ${error.message}`);
      }
    }
  }

  onInsert(): void {
    this.sidebarVisible = false;
    this.sidebarVisibleChange.emit(this.sidebarVisible);
    this.responseTextInserted.emit(this.responseText);
  }
  closeSidebar() {
    this.sidebarVisible = false;
    this.sidebarVisibleChange.emit(this.sidebarVisible);
  }
}
