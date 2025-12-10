using System.Numerics;

public class SandRules : ParticleRules
{
    //int inertia;
    private static readonly Random rng = new Random();
    private static readonly Vector2 down = new Vector2(0, 1);
    private static readonly Vector2 still = new Vector2(0,0);
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        int below = y + 1;
        ref Particle p = ref grid._grid[x,y];
        byte density = (byte)(p.variables >> 5);
        if (below >= grid._grid.GetLength(1))
        {
            //p.velocity = still;
            return;            
        }
            // Determine if density is lower
        if(CheckDensity(grid, x, below, density))
        {
            p.velocity = down;
            return;
        }
        
        // Determine to fall left or right first
        bool dir = rng.Next(0, 2) == 0;

        int dx1 = dir ? -1 : 1;
        int dx2 = dir ? 1 : -1;

        int nx1 = x + dx1;
        int nx2 = x + dx2;

        // Try first direction
        if (nx1 >= 0 && nx1 < grid._grid.GetLength(0))
        {
            if (CheckDensity(grid, nx1, below, density))
            {
                p.velocity = new Vector2(dx1, 1);
                return;
            }
        }

        // Try other direction
        if (nx2 >= 0 && nx2 < grid._grid.GetLength(0))
        {
            if (CheckDensity(grid, nx2, below, density))
            {
                p.velocity = new Vector2(dx1, 1);
                return;
            }
        }
    }
}