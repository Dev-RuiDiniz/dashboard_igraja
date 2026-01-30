using IgrejaSocial.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IFamiliaRepository
    {
        Task<Familia?> ObterPorIdAsync(Guid id);
        Task<Familia?> ObterPorCpfAsync(string cpf);
        Task<IEnumerable<Familia>> ListarTodasAsync(); // O repositório deve implementar exatamente este nome
        Task<IEnumerable<Familia>> ListarVulneraveisAsync();
        Task<IEnumerable<Familia>> ListarPorBairroAsync(string bairro);
        Task<bool> JaRecebeuBeneficioNoMesAtualAsync(Guid familiaId);
        Task AdicionarAsync(Familia familia);
        void Atualizar(Familia familia);
        Task SalvarAlteracoesAsync();
    }
}