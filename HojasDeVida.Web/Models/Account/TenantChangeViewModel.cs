using Abp.AutoMapper;
using HojasDeVida.Sessions.Dto;

namespace HojasDeVida.Web.Models.Account
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}