using System.Reflection.Metadata;

public class LiquidRules : ParticleRules
{
    //public int dispersionRate = 1;
    const int DIR_BIT = 1; 
    public override void UpdateParticle(Grid grid, int x, int y)
    {
        // int below = y + 1;
        // byte density = (byte)(grid._grid[x,y].variables >> 5);
        // if (below >= grid._grid.GetLength(1))
        //     return;
        // // Determine if density is lower
        // if(CheckDensity(grid, x, below, density))
        // {
        //     grid.Swap(x,y,x,below);
        //     return;
        // }
        // bool dir = Random.Shared.Next(2) == 0;

        // int dx = dir ? 1 : -1;

        // int nx = x + dx;

        // // Check if against wall if so flip direction
        // if (nx >= 0 && nx < grid._grid.GetLength(0))
        //     if (!CheckDensity(grid, nx, y, density))
        //     {
        //         grid._grid[x, y].variables ^= DIR_BIT;
        //         nx = x + dx * -1;
        //     }
        //     else
        //     {
        //         // Do nothing
        //     }
        // else
        // {
        //     grid._grid[x, y].variables ^= DIR_BIT;
        //     nx = x + dx * -1;
        // }
        // if (nx >= 0 && nx < grid._grid.GetLength(0))
        // {
        //     if (CheckDensity(grid, nx, y, density))
        //     {
        //         grid.Swap(x, y, nx, y);
        //         grid._grid[nx, y].updateFlag = true;
        //     }
        // }
    }
}