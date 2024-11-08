using System.Security.Cryptography;
using System.Text;

public class AuthenticationService
{
    Dictionary<string, string> korisnici = new();

    public bool RegistrujKorisnika(string korisnickoIme, string lozinka)
    {
        if (korisnici.ContainsKey(korisnickoIme)) return false;
        korisnici[korisnickoIme] = HashLozinka(lozinka);
        return true;
    }

    public bool AutentifikujKorisnika(string korisnickoIme, string lozinka)
    {
        if (!korisnici.TryGetValue(korisnickoIme, out string hashedLozinka)) return false;
        return HashLozinka(lozinka) == hashedLozinka;
    }

    private string HashLozinka(string lozinka)
    {
        using var sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}
