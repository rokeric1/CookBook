public class Recipe
{
    public int id { get; set; }
    public string naziv { get; set; }
    public List<string> sastojci { get; set; }
    public string kategorija { get; set; }
    public int vrijemePripreme { get; set; }
    public string upute { get; set; }
    public int popularnost { get; set; }

    public Recipe()
    {
        sastojci = new List<string>();
    }

    public Recipe(int id, string naziv, List<string> sastojci, string kategorija, int vrijemePripreme, string upute, int popularnost)
    {
        this.id = id;
        this.naziv = naziv;
        this.sastojci = sastojci;
        this.kategorija = kategorija;
        this.vrijemePripreme = vrijemePripreme;
        this.upute = upute;
        this.popularnost = popularnost;
    }

    public int GetId() => id;
    public string GetNaziv() => naziv;
    public List<string> GetSastojci() => sastojci;
    public string GetKategorija() => kategorija;
    public int GetVrijemePripreme() => vrijemePripreme;
    public string GetUpute() => upute;
    public int GetPopularnost() => popularnost;

    public void SetNaziv(string naziv) => this.naziv = naziv;
    public void SetSastojci(List<string> sastojci) => this.sastojci = sastojci;
    public void SetKategorija(string kategorija) => this.kategorija = kategorija;
    public void SetVrijemePripreme(int vrijeme) => this.vrijemePripreme = vrijeme;
    public void SetUpute(string upute) => this.upute = upute;
    public void SetPopularnost(int popularnost) => this.popularnost = popularnost;

    public override string ToString()
    {
        string sastojciString = string.Join(", ", sastojci);
        return $"Naziv: {naziv}\nSastojci: {sastojciString}\nKategorija: {kategorija}\nVrijeme pripreme: {vrijemePripreme} min\nUpute: {upute}\nPopularnost: {popularnost}";
    }
}
