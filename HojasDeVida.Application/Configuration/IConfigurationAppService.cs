using System.Threading.Tasks;
using Abp.Application.Services;
using HojasDeVida.Configuration.Dto;

namespace HojasDeVida.Configuration
{
    public interface IConfigurationAppService: IApplicationService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}