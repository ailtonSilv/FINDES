using Microsoft.AspNetCore.Mvc;
using Findes.WebApi.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Findes.Standard.Core.Util;
using Newtonsoft.Json.Linq;
using System;
using Findes.WebApi.Model;

namespace Findes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NacionalController : Controller
    {
        public IOptions<AppSettings> AppSettings;
        StringHelper stringHelper;
        public NacionalController(IOptions<AppSettings> _appSettings)
        {
            AppSettings = _appSettings;
            this.stringHelper = new StringHelper(AppSettings);
        }

        [HttpGet]
        [Route("contas")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroContas([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "contas");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("contatos")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroContatos([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "contatos");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("oportunidades")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroOportunidades([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "oportunidades");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("produtooportunidades")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroProdutoOportunidades([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "produtoOportunidades");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("propostas")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroPropostas([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "propostas");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("produtopropostas")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroProdutoPropostas([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "produtoPropostas");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("contratos")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroContratos([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "contratos");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("produtocontratos")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroProdutoContratos([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "produtoContratos");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("ocorrencias")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroOcorrencias([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "ocorrencias");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }

        [Route("produtos")]
        [ApiExceptionFilter]
        [BasicAuthFilter]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult ListarFiltroProdutos([FromQuery] PostModel value)
        {
            try
            {
                Process p = new Process(AppSettings);
                var result = p.ProcessPostModel(value, "produtos");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Retorno retorno = new Retorno
                {
                    mensagem = ex.Message,
                    sucesso = false
                };
                JObject jRetorno = JObject.FromObject(retorno);
                return BadRequest(jRetorno);
            }
        }
    }
}