using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using HojasDeVida.Configuration.Dto;

namespace HojasDeVida.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : HojasDeVidaAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
