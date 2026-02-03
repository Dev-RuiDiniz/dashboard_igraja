using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IRegistroAtendimentoRepository
    {
        Task AdicionarAsync(RegistroAtendimento registro);
        Task<RegistroAtendimento?> ObterPorIdAsync(Guid id);
        Task<RegistroAtendimento?> ObterAtivoPorEquipamentoAsync(Guid equipamentoId);
        Task<IEnumerable<RegistroAtendimento>> ListarPorFamiliaAsync(Guid familiaId);
        Task<bool> ExisteEmprestimoAtivoPorFamiliaETipoAsync(Guid familiaId, TipoEquipamento tipo);
        void Atualizar(RegistroAtendimento registro);
        Task SalvarAlteracoesAsync();
    }
}
