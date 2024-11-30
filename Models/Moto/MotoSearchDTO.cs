public class MotoSearchDTO
{
    public List<string> MotoBrand { get; set; } 
    public string Model { get; set; }       
    public string Color { get; set; }        
    public int[] Age { get; set; }      // Intervalo de anos (ex: [Year1, Year2])
    public int[] Km { get; set; }        // Intervalo de quilometragem (ex: [Km1, Km2])
    public int[] Price { get; set; }
}