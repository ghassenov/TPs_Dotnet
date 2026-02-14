namespace IdentityApp.Models;

public class PanierParUser
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ProduitId { get; set; }
    public int Quantity { get; set; } = 1;

    // Navigation properties
    public ApplicationUser? User { get; set; }
    public Produit? Produit { get; set; }
}
