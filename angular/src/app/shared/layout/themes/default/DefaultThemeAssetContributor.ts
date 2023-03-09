import { NameValuePair } from '@shared/utils/name-value-pair';
import { IThemeAssetContributor } from '../ThemeAssetContributor';
import { ThemeHelper } from '../ThemeHelper';

export class DefaultThemeAssetContributor implements IThemeAssetContributor {
    public getAssetUrls(): string[] {
        return [
        ];
    }

    public getMenuWrapperStyle(): string {
        return 'header-menu-wrapper header-menu-wrapper-left';
    }

    public getSubheaderStyle(): string {
        return 'text-dark fw-bold my-1 me-5';
    }

    public getFooterStyle(): string {
        return 'footer py-4 d-flex flex-lg-column';
    }

    getBodyAttributes(): NameValuePair[] {
        const skin = ThemeHelper.getAsideSkin();
        return [{
            name: 'data-kt-app-layout',
            value: skin + '-sidebar'
        }, {
            name: 'data-kt-app-header-fixed',
            value: ThemeHelper.getDesktopFixedHeader()
        },
        {
            name: 'data-kt-app-header-fixed-mobile',
            value: ThemeHelper.getMobileFixedHeader()
        },
        {
            name: 'data-kt-app-sidebar-enabled',
            value: 'true'
        },
        {
            name: 'data-kt-app-sidebar-fixed',
            value: ThemeHelper.getFixedAside()
        },
        {
            name: 'data-kt-app-sidebar-hoverable',
            value: ThemeHelper.getHoverableAside()
        },
        {
            name: 'data-kt-app-sidebar-push-header',
            value: 'true'
        },
        {
            name: 'data-kt-app-toolbar-enabled',
            value: 'true'
        },
        {
            name: 'data-kt-app-sidebar-push-toolbar',
            value: 'true'
        },
        {
            name: 'data-kt-app-sidebar-push-footer',
            value: 'true'
        },
        {
            name: 'data-kt-app-toolbar-enabled',
            value: 'true'
        },
        {
            name: 'data-kt-app-sidebar-minimize',
            value: ThemeHelper.getDefaultMinimizedAside() === 'true' ? 'on' : 'off'
        }
        ];
    }

    getAppModuleBodyClass(): string {
        return 'app-default';
    }
}
