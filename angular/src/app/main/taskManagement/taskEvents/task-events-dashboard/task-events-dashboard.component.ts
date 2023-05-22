import { AfterViewInit, Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenService } from 'abp-ng2-module';

@Component({
  selector: 'app-task-events-dashboard',
  templateUrl: './task-events-dashboard.component.html',
  styleUrls: ['./task-events-dashboard.component.css']
})
export class TaskEventsDashboardComponent extends AppComponentBase implements OnInit, AfterViewInit {
  taskEventId: number;
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    let taskEventId = this.route.snapshot.paramMap.get('taskEventId')
    this.taskEventId = parseInt(taskEventId);
  }
  ngAfterViewInit() {

  }
}
