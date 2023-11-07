import { Component, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ContractsServiceProxy, CreateOrEditContractDto } from '@shared/service-proxies/service-proxies';
import { TokenService } from 'abp-ng2-module';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-contract-dashboard',
  templateUrl: './contract-dashboard.component.html',
  styleUrls: ['./contract-dashboard.component.css']
})
export class ContractDashboardComponent extends AppComponentBase {
  active = false;
  saving = false;
  contractId: number;
  contract: CreateOrEditContractDto = new CreateOrEditContractDto();
  contractTypeOptions: any[];
  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private _tokenService: TokenService,
    private _contractService: ContractsServiceProxy
  ) {
    super(injector);
    this.loadAllDropdown() 
  }

  ngOnInit(): void {
    let contractId = this.route.snapshot.paramMap.get('contractId')
    this.contractId = parseInt(contractId);
  }

  loadAllDropdown() {
    this._contractService.getAllContractTypeForLookupTable('', '', 0, 1000).subscribe(result => {
      this.contractTypeOptions = result.items;
    });
  }

  saveBasicInfo(): void {
    this.saving = true;
    this._contractService.createOrEdit(this.contract)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
      });
  }
}
