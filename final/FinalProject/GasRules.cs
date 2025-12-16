using System.Diagnostics;
using System.Drawing.Design;
using System.Numerics;
using System.Reflection.Metadata;

public class GasRules : ParticleRules
{
    private static readonly Random rng = new Random();
    private int dispersionRate = 3;
    const int DIR_BIT = 1; 
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        int above = y - 1;
        ref Particle p = ref grid._grid[x,y];
        byte density = p.GetDensity();
        // Determine if density is lower
        if(CheckDensity(grid, x, above, density))
        {
            p.velocity = new Vector2(0, -rng.Next(1,3));
            p.velocity = CheckPath(grid, p.velocity, x, y, density);
            return;
        }
        bool dir = p.GetEndBit();

        int dx = dir ? 1 : -1;

        int nx = x + dx;

        // Check if against wall if so flip direction
        if (!CheckDensity(grid, nx, y, density))
        {
            p.SetEndBit();
            dx *= -1;
            nx = x + dx;
        }
        if (CheckDensity(grid, nx, y, density))
        {
            p.velocity = new Vector2(dx * rng.Next(dispersionRate/2 ,dispersionRate), 0);
            p.velocity = CheckPath(grid, p.velocity, x, y, density);
        }
    }
}