namespace Services
{
    public interface IOcjenaService
    {
        void DodajOcjenu(int recipeId, int ocjena);
        double GetProsjecnaOcjena(int recipeId);
        List<int> GetSveOcjene(int recipeId);
    }
}
