using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Dictionary<string, string> korisnici = new Dictionary<string, string>();

        public string CitajLozinku()
        {
            string lozinka = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && lozinka.Length > 0)
                {
                    Console.Write("\b \b");
                    lozinka = lozinka[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    lozinka += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return lozinka;
        }

        public bool RegistrujKorisnika(string korisnickoIme, string lozinka)
        {
            if (korisnici.ContainsKey(korisnickoIme))
                return false;

            korisnici[korisnickoIme] = HashLozinka(lozinka);
            return true;
        }

        public bool AutentifikujKorisnika(string korisnickoIme, string lozinka)
        {
            return korisnici.TryGetValue(korisnickoIme, out string hash) && hash == HashLozinka(lozinka);
        }

        private string HashLozinka(string lozinka)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
