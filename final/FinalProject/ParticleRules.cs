using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

public abstract class ParticleRules
{
    public abstract void UpdateParticle(Grid grid, int x, int y);

    protected bool CheckDensity(Grid grid, int x, int y, byte density){
        // Check if out of bounds
        if(x >= grid._grid.GetLength(0) || y >= grid._grid.GetLength(1) || x < 0 || y <= 0)
            return false;
        byte pd = grid._grid[x, y].GetDensity();
        return pd < density;
    }

    protected Vector2 CheckPath(Grid grid, Vector2 velocity, int x, int y, byte density)
    {
        Vector2 finalVelocity = Vector2.Zero;
        int multiY = velocity.Y > 0 ? 1 : -1;
        for(int i = 0; i < Math.Abs(velocity.Y); i++)
        {
            if(CheckDensity(grid, x, (i + 1) * multiY + y, density))
                finalVelocity.Y += multiY;
            else
                return finalVelocity;
        }
        int multix = velocity.X > 0 ? 1 : -1;
        for(int i = 0; i < Math.Abs(velocity.X); i++)
        {
            if(CheckDensity(grid, (i + 1) * multix + x, y + (int)finalVelocity.Y, density))
                finalVelocity.X += multix;
            else
                break;
        }
        return finalVelocity;
    }
}