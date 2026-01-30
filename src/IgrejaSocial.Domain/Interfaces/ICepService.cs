using IgrejaSocial.Domain.Models;
using System.Threading.Tasks;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface ICepService
    {
        Task<CepResponse> BuscarEnderecoPorCepAsync(string cep);
    }
}