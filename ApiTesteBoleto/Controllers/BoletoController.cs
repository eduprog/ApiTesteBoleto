using BoletoNetCore;
using BoletoNetCore.Pdf.BoletoImpressao;
using Microsoft.AspNetCore.Mvc;
using ApiTesteBoleto.Utils;

namespace ApiTesteBoleto.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BoletoController : ControllerBase
    {
        public BoletoController()
        {

        }

        [HttpPost("CriarBoleto/{quantidade}")]
        [Consumes("Application/json")]
        [Produces("Application/json")]
        public async Task<IActionResult> CriarBoleto(int quantidade)
        {
            try
            {
                var currentDir = $@"{Directory.GetCurrentDirectory()}\Boleto";
                if (!Directory.Exists(currentDir))
                {
                    Directory.CreateDirectory(currentDir);
                }
                var contaBancaria = new ContaBancaria
                {
                    Agencia = "0156",
                    Conta = "85305",
                    DigitoConta = "4",
                    CarteiraPadrao = "1",
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    VariacaoCarteiraPadrao = "A",
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa,
                    OperacaoConta = "05"

                };

                var banco = Banco.Instancia(Bancos.Sicredi);
                banco.Beneficiario = HelperUtils.GerarBeneficiario("85305", "", "", contaBancaria);
                banco.FormataBeneficiario();
                Boletos boletos = HelperUtils.GerarBoletos(banco, quantidade, "N", 10);
                //var bytes = new 
                //var fileName = Path.Combine(currentDir, "boleto.html");
                //System.IO.File.WriteAllBytes(fileName,bytes);
                return await Task.FromResult(Ok(boletos));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(BadRequest(e.Message));
            }
        }
    }
}


