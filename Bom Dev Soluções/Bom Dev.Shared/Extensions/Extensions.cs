using System;

namespace Data.Extensions
{
    public static class Extensions
    {
        public static bool? ToNullableBool(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            if (s.Equals("1") || string.Equals(s, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (s.Equals("0") || string.Equals(s, bool.FalseString, StringComparison.OrdinalIgnoreCase))
                return false;

            return null;
        }

        public static bool? ToNullableBool(this object s)
        {
            if (s == null)
                return null;
            string str = s.ToString();
            if (str.Equals("1") || string.Equals(str, bool.TrueString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (str.Equals("0") || string.Equals(str, bool.FalseString, StringComparison.OrdinalIgnoreCase))
                return false;

            return null;
        }

        public static string ToNullableStringOrDefault(this object s, string defaultValue = null)
        {
            if (s == null)
                return defaultValue;

            return s.ToString();
        }

        public static string ToNullableStringOrDefault(this string s, string defaultValue = null)
        {
            if (string.IsNullOrEmpty(s))
                return defaultValue;

            return s;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static decimal? ToNullableDecimal(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            if (decimal.TryParse(s, out decimal i)) return i;
            return null;
        }

        public static decimal? ToNullableDecimal(this object s)
        {
            if (s == null)
                return null;
            if (decimal.TryParse(s.ToString(), out decimal i)) return i;
            return null;
        }

        public static int? ToNullableInt(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            if (int.TryParse(s, out int i)) return i;
            return null;
        }

        public static int? ToNullableInt(this object s, bool greaterThanZeroOrDefault = false)
        {
            if (s == null)
                return null;
            if (int.TryParse(s.ToString(), out int i))
            {
                if (greaterThanZeroOrDefault)
                {
                    if (i <= 0)
                        return null;
                }

                return i;
            }
            return null;
        }

        public static double? ToNullableDouble(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            if (double.TryParse(s, out double i)) return i;
            return null;
        }

        public static long? ToNullableLong(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            if (long.TryParse(s, out long i)) return i;
            return null;
        }

        public static DateTime? ToNullableDateTime(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            if (DateTime.TryParse(s, out DateTime i)) return i;
            return null;
        }

        public static DateTime? ToNullableDateTime(this object s)
        {
            if (s == null)
                return null;

            if (DateTime.TryParse(s.ToString(), out DateTime i)) return i;
            return null;
        }
        public static bool isCPF(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            if (cpf.Equals("00000000000") || cpf.Equals("11111111111") || cpf.Equals("22222222222") || cpf.Equals("33333333333") || cpf.Equals("44444444444")
                || cpf.Equals("55555555555") || cpf.Equals("66666666666") || cpf.Equals("77777777777") || cpf.Equals("88888888888") || cpf.Equals("99999999999"))
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }


        public static bool isCNPJ(this string cnpj)
        {
            string CNPJ = cnpj.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (
                         resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }
    }
}
