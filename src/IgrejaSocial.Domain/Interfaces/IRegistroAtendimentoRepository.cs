using IgrejaSocial.Domain.Entities;
using System.Threading.Tasks;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IRegistroAtendimentoRepository
    {
        Task AdicionarAsync(RegistroAtendimento registro);
        Task SalvarAlteracoesAsync();
    }
}
