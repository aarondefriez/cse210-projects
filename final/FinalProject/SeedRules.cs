using System.Numerics;
public class SeedRules : ParticleRules
{
    private static readonly Random rng = new Random();
    public readonly Particle vine = new Particle(5);
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        int below = y + 1;
        ref Particle p = ref grid._grid[x,y];
        byte density = p.GetDensity();

        // If nothing above its density below than swap with it
        if(CheckDensity(grid, x, below, density))
        {
            p.velocity = new Vector2(0, rng.Next(2,5));
            p.velocity = CheckPath(grid, p.velocity, x, y, density);
            return;
        }

        // If at rest and water above it turn water into vine (chooses one of three random positions)
        int check = rng.Next(-1,2);
        if(grid._grid[x + check, y-1].GetMaterial() == 2 && !p.GetEndBit())
        {
            grid._grid[x + check, y-1] = vine;
            p.SetEndBit(); // only allow one vine to grow out of it
        }
    }
    
}