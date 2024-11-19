namespace Models
{
    public class SastojakKolicina
    {
        public int IngredientId { get; set; }
        public double Kolicina { get; set; }

        public SastojakKolicina() { }

        public SastojakKolicina(int ingredientId, double kolicina)
        {
            IngredientId = ingredientId;
            Kolicina = kolicina;
        }
    }
}
