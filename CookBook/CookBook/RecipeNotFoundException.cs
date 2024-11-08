public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(string naziv)
        : base($"Recept sa nazivom '{naziv}' nije pronađen.")
    {
    }
}
