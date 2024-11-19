namespace Services;

public interface IAuthenticationService
{
	bool RegistrujKorisnika(string korisnickoIme, string lozinka);
	bool AutentifikujKorisnika(string korisnickoIme, string lozinka);
	string CitajLozinku();
}
