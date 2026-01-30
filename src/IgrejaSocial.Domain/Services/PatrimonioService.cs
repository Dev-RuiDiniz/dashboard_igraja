using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Services
{
    public class PatrimonioService
    {
        /// <summary>
        /// Gera um código baseado nas 3 primeiras letras do tipo + número sequencial.
        /// </summary>
        public string GerarCodigoPatrimonio(TipoEquipamento tipo, int proximoNumero)
        {
            // Pega as 3 primeiras letras em maiúsculo (ex: CAD)
            string prefixo = tipo.ToString().Substring(0, 3).ToUpper();
            
            // Retorna no formato PREFIXO-000 (ex: CAD-001)
            return $"{prefixo}-{proximoNumero:D2}";
        }
    }
}