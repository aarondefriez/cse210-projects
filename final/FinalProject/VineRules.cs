using System.Numerics;
public class VineRules : ParticleRules
{
    private static readonly Random rng = new Random();
    public readonly Particle vine = new Particle(5);
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        ref Particle p = ref grid._grid[x,y];

        // If at rest and water above it turn water into vine (chooses one of three random positions)
        int check = rng.Next(-1,2);
        if(grid._grid[x + check, y-1].GetMaterial() == 2 && !p.GetEndBit())
        {
            grid._grid[x + check, y-1] = vine;
            p.SetEndBit(); // Only allow to grow once to avoid blocks
        }
    }
    
}