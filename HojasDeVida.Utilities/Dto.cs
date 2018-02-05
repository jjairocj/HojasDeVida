using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Web.Models;

namespace HojasDeVida
{
    public class RespuestaDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public List<string> Errores { get; set; }
        public List<string> Advertencias { get; set; }

        public RespuestaDto()
        {
            Exitoso = false;
            Mensaje = null;
            Errores = new List<string>();
            Advertencias = new List<string>();
        }

        public RespuestaDto(bool exitoso)
        {
            Exitoso = exitoso;
            Mensaje = null;
            Errores = new List<string>();
            Advertencias = new List<string>();
        }

        public RespuestaDto(bool exitoso, string mensaje)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Errores = new List<string>();
            Advertencias = new List<string>();
        }

        public RespuestaDto(bool exitoso, string mensaje, List<string> errores)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Errores = errores;
            Advertencias = new List<string>();
        }

        public RespuestaDto(bool exitoso, string mensaje, List<string> errores, List<string> advertencias)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Errores = errores;
            Advertencias = advertencias;
        }

        public AjaxResponse ObtenerAjaxResponse()
        {
            AjaxResponse ajaxResponse = new AjaxResponse(Exitoso);

            if (!Exitoso)
            {
                if (Errores == null)
                    Errores = new List<string>();

                ajaxResponse.Error = new ErrorInfo
                {
                    Code = 0,
                    Message = null,
                    Details = null
                };

                if (Errores.Count == 0 && !Mensaje.IsNullOrWhiteSpace())
                    Errores.Add(Mensaje);

                List<ValidationErrorInfo> temp = new List<ValidationErrorInfo>();
                foreach (var item in Errores)
                {
                    temp.Add(new ValidationErrorInfo { Message = item });
                }

                ajaxResponse.Error.ValidationErrors = temp.ToArray();
            }
            else
            {
                if (!Mensaje.IsNullOrWhiteSpace())
                {
                    ajaxResponse.Result = Mensaje;
                }
            }

            return ajaxResponse;
        }
    }

    public class PagedAndSortedResultRequest : IPagedAndSortedResultRequest
    {
        public int MaxResultCount { get; set; }
        public int Page { get; set; }
        public string Sorting { get; set; }

        private int _skipCount;
        public int SkipCount
        {
            get
            {
                _skipCount = (Page > 0 && MaxResultCount >= 0) ? (Page - 1) * MaxResultCount : 0;
                return _skipCount;
            }
            set { _skipCount = value; }
        }

        public PagedAndSortedResultRequest()
        {
            MaxResultCount = 50;
            SkipCount = 0;
            Page = 1;
        }
    }
}
