import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, Input, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    ProductUpsellRelatedProductsServiceProxy,
    ProductUpsellRelatedProductDto,
    GetPublicProductForViewDto,
    PublicPagesCommonServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProductUpsellRelatedProductModalComponent } from './create-or-edit-productUpsellRelatedProduct-modal.component';

import { ViewProductUpsellRelatedProductModalComponent } from './view-productUpsellRelatedProduct-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import SwiperCore, {
    Navigation,
    Pagination,
    Scrollbar,
    A11y,
    Virtual,
    Zoom,
    Autoplay,
    Thumbs,
    Controller,
} from 'swiper';
import { BehaviorSubject } from "rxjs";
import Swiper from "swiper";

// install Swiper components
SwiperCore.use([
    Navigation,
    Pagination,
    Scrollbar,
    A11y,
    Virtual,
    Zoom,
    Autoplay,
    Thumbs,
    Controller
]);
@Component({
    selector: 'app-RelatedProducts',
    templateUrl: './productUpsellRelatedProducts.component.html',
})
export class ProductUpsellRelatedProductsComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @ViewChild('createOrEditProductUpsellRelatedProductModal', { static: true })
    createOrEditProductUpsellRelatedProductModal: CreateOrEditProductUpsellRelatedProductModalComponent;
    @ViewChild('viewProductUpsellRelatedProductModal', { static: true })
    viewProductUpsellRelatedProductModal: ViewProductUpsellRelatedProductModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxRelatedProductIdFilter: number;
    maxRelatedProductIdFilterEmpty: number;
    minRelatedProductIdFilter: number;
    minRelatedProductIdFilterEmpty: number;
    maxDisplaySequenceFilter: number;
    maxDisplaySequenceFilterEmpty: number;
    minDisplaySequenceFilter: number;
    minDisplaySequenceFilterEmpty: number;
    productNameFilter = '';
    @Input() productId: number;

    thumbs: any;
    slides$ = new BehaviorSubject<string[]>(['']);
    breakpoints = {
        640: { slidesPerView: 2, spaceBetween: 20 },
        768: { slidesPerView: 4, spaceBetween: 20 },
        1024: { slidesPerView: 5, spaceBetween: 20 },
    };
    controlledSwiper: any;
    responsiveOptions: any;

    relatedProducts: GetPublicProductForViewDto[] = [];
    constructor(
        injector: Injector,
        private _productUpsellRelatedProductsServiceProxy: ProductUpsellRelatedProductsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _publicPagesCommonServiceProxy: PublicPagesCommonServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.getRelatedProduct(this.productId);
    }

    ngOnInit(): void {
        this.initializeSwiper();
    }
    ngAfterViewInit(): void {
        this.initializeSwiper();
    }

    initializeSwiper(): void {
        this.controlledSwiper = new Swiper('.swiper-container', {
            observer: true,
            observeParents: true,
            slidesPerView: 5,
            spaceBetween: 20,
            navigation: {
                prevEl: '.swiper-navigation-prev',
                nextEl: '.swiper-navigation-next',
            },
            breakpoints: {
                1920: { slidesPerView: 5, spaceBetween: 20 },
                1366: { slidesPerView: 5, spaceBetween: 20 },
                1024: { slidesPerView: 4, spaceBetween: 20 },
                768: { slidesPerView: 2, spaceBetween: 20 },
                560: { slidesPerView: 1, spaceBetween: 20 },
            },
        });
    }

    getRelatedProduct(productId: number) {
        this._publicPagesCommonServiceProxy.getRelatedProduct(productId).subscribe(result => {
            this.relatedProducts = result;
        });
    }
    onDeleteRelatedProduct(primaryProductId: number, relatedProductId: number) {
        this._productUpsellRelatedProductsServiceProxy.deleteByProductId(primaryProductId, relatedProductId).subscribe(result => {
            this.notify.success(this.l('DeletedSuccessfully'));
            this.getRelatedProduct(this.productId);
        })
    }
    getProductUpsellRelatedProducts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._productUpsellRelatedProductsServiceProxy
            .getAll(
                this.filterText,
                this.maxRelatedProductIdFilter == null
                    ? this.maxRelatedProductIdFilterEmpty
                    : this.maxRelatedProductIdFilter,
                this.minRelatedProductIdFilter == null
                    ? this.minRelatedProductIdFilterEmpty
                    : this.minRelatedProductIdFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.productNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createProductUpsellRelatedProduct(): void {
        this.createOrEditProductUpsellRelatedProductModal.productId = this.productId;
        this.createOrEditProductUpsellRelatedProductModal.show();
    }

    deleteProductUpsellRelatedProduct(productUpsellRelatedProduct: ProductUpsellRelatedProductDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._productUpsellRelatedProductsServiceProxy.delete(productUpsellRelatedProduct.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._productUpsellRelatedProductsServiceProxy
            .getProductUpsellRelatedProductsToExcel(
                this.filterText,
                this.maxRelatedProductIdFilter == null
                    ? this.maxRelatedProductIdFilterEmpty
                    : this.maxRelatedProductIdFilter,
                this.minRelatedProductIdFilter == null
                    ? this.minRelatedProductIdFilterEmpty
                    : this.minRelatedProductIdFilter,
                this.maxDisplaySequenceFilter == null
                    ? this.maxDisplaySequenceFilterEmpty
                    : this.maxDisplaySequenceFilter,
                this.minDisplaySequenceFilter == null
                    ? this.minDisplaySequenceFilterEmpty
                    : this.minDisplaySequenceFilter,
                this.productNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    resetFilters(): void {
        this.filterText = '';
        this.maxRelatedProductIdFilter = this.maxRelatedProductIdFilterEmpty;
        this.minRelatedProductIdFilter = this.maxRelatedProductIdFilterEmpty;
        this.maxDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.minDisplaySequenceFilter = this.maxDisplaySequenceFilterEmpty;
        this.productNameFilter = '';

        this.getProductUpsellRelatedProducts();
    }

    setControlledSwiper(swiper) {
        this.controlledSwiper = swiper;
    }

}
