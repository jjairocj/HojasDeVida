using Abp.Web.Mvc.Views;

namespace HojasDeVida.Web.Views
{
    public abstract class HojasDeVidaWebViewPageBase : HojasDeVidaWebViewPageBase<dynamic>
    {

    }

    public abstract class HojasDeVidaWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected HojasDeVidaWebViewPageBase()
        {
            LocalizationSourceName = HojasDeVidaConsts.LocalizationSourceName;
        }
    }
}