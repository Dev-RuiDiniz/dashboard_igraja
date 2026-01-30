using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Validations
{
    public class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? cpf = value?.ToString()?.Replace(".", "").Replace("-", "");

            if (string.IsNullOrEmpty(cpf)) return ValidationResult.Success; // [Required] cuida do nulo

            if (cpf.Length != 11 || !IsCpfValido(cpf))
            {
                return new ValidationResult("O CPF informado é inválido.");
            }

            return ValidationResult.Success;
        }

        private bool IsCpfValido(string cpf)
        {
            // Elimina CPFs conhecidos inválidos (11111111111, etc)
            if (cpf.Distinct().Count() == 1) return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}