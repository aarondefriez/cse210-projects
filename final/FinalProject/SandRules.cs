using System.Numerics;

public class SandRules : ParticleRules
{
    //int inertia;
    private static readonly Random rng = new Random();
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        int below = y + 1;
        ref Particle p = ref grid._grid[x,y];
        byte density = p.GetDensity();

        if(CheckDensity(grid, x, below, density))
        {
            p.velocity = new Vector2(0, rng.Next(2,5));
            p.velocity = CheckPath(grid, p.velocity, x, y, density);
            return;
        }
        
        // Determine to fall left or right first
        bool dir = rng.Next(0, 2) == 0;

        int dx1 = dir ? -1 : 1;
        int dx2 = dir ? 1 : -1;

        int nx1 = x + dx1;
        int nx2 = x + dx2;

        // Try first direction
        if (CheckDensity(grid, nx1, below, density))
        {
            p.velocity = new Vector2(dx1, 1);
            return;
        }

        // Try other direction
        if (CheckDensity(grid, nx2, below, density))
        {
            p.velocity = new Vector2(dx1, 1);
            return;
        }
    }
}