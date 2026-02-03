using IgrejaSocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IRegistroAtendimentoRepository
    {
        Task AdicionarAsync(RegistroAtendimento registro);
        Task<RegistroAtendimento?> ObterAtivoPorEquipamentoAsync(Guid equipamentoId);
        Task<bool> ExisteEmprestimoAtivoPorFamiliaETipoAsync(Guid familiaId, TipoEquipamento tipo);
        Task<List<RegistroAtendimento>> ListarPorFamiliaAsync(Guid familiaId);
        Task SalvarAlteracoesAsync();
    }
}
