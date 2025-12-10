using System.Security.Principal;

public abstract class ParticleRules
{
    public abstract void UpdateParticle(Grid grid, int x, int y);

    protected bool CheckDensity(Grid grid, int x, int y, byte density){
        byte pd = (byte)(grid._grid[x, y].variables >> 5);
        return pd < density;
    }
}