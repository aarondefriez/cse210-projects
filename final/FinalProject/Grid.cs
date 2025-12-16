using System.Drawing.Imaging;
using System.Numerics;
using System.Security.Principal;

public class Grid
{
    ParticleRules[] rules = new ParticleRules[6];
    // 0,0 is top left to follow winform format
    public Particle[,] _grid;

    Bitmap bitmap;

    public Grid(int width, int height)
    {
        _grid = new Particle[width, height];
        _grid = new Particle[width, height];
        InitializeRules();
        InitializeGrid(_grid);
    }

    public void InitializeGrid(Particle[,] grid)
    {
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x,y] = new Particle(0); // Populate grid with air
            }
        }
        bitmap = new Bitmap(_grid.GetLength(0), _grid.GetLength(1), PixelFormat.Format32bppArgb);
    }
    // Initilize rules for different particles in order
    public void InitializeRules()
    {
        rules[0] = new EmptyRules(); // Air
        rules[1] = new SandRules(); // Sand
        rules[2] = new LiquidRules(); // Water
        rules[3] = new GasRules(); // Gas
        rules[4] = new SeedRules(); // Seed
        rules[5] = new VineRules(); // Vine
    }

    public void Step()
    {
        // First itteration check where pixels want to move
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for(int x = 0; x < _grid.GetLength(0); x++)
            {
                byte material = _grid[x, y].GetMaterial();
                rules[material].UpdateParticle(this, x, y);
            }
        }
        // Second itteration move the pixels
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for(int x = 0; x < _grid.GetLength(0); x++)
            {
                if(_grid[x, y].GetMaterial() != 0)
                {
                    ref Particle p = ref _grid[x, y];
                    Vector2 v = p.velocity;
                    if(Program.debug) // Used for debug
                        p.oldVelocity = v;
                    p.velocity = Vector2.Zero; // Clear velocity
                    Swap(x, y, (int)v.X, (int)v.Y); // Swap pixels
                }
            }
        }
    }
    public void Swap(int x1, int y1, int x2, int y2)
    {
        x2 = x2 + x1;
        y2 = y2 + y1;
        Particle temp = _grid[x1, y1];
        _grid[x1, y1] = _grid[x2, y2];
        _grid[x2, y2] = temp;
    }
    public void AddPixel(int _x, int _y, int radius, byte material)
    {
        int r = radius;
        // Clamp to prevent out of bounds errors
        int startX = Math.Max(0, _x - r);
        int endX   = Math.Min(_grid.GetLength(0) - 1, _x + r);
        int startY = Math.Max(0, _y - r);
        int endY   = Math.Min(_grid.GetLength(1) - 1, _y + r);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = endY; y >= startY; y--)
            {
                if (_grid[x, y].GetMaterial() == 0) // Fill if its air
                    _grid[x, y] = new Particle(material);
                else if(material == 0) // if eraesing erase any pixel regardless of material
                    _grid[x, y] = new Particle(material);
            }
        }
    }
    public Bitmap ToBitmap()
    {
        int width = _grid.GetLength(0);
        int height = _grid.GetLength(1);

        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height),
                                        ImageLockMode.WriteOnly,
                                        PixelFormat.Format32bppArgb);

        unsafe
        {
            byte* ptr = (byte*)data.Scan0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Particle p = _grid[x, y];
                    int idx = y * data.Stride + x * 4;
                    ptr[idx + 0] = (byte)(p._color & 0xFF);       // B
                    ptr[idx + 1] = (byte)((p._color >> 8) & 0xFF);  // G
                    ptr[idx + 2] = (byte)((p._color >> 16) & 0xFF); // R
                    ptr[idx + 3] = (byte)((p._color >> 24) & 0xFF); // A
                    if (Program.debug && p.GetMaterial()!= 0)
                    {
                        byte xv = (byte)p.oldVelocity.X;
                        byte yv = (byte)p.oldVelocity.Y;
                        bool dir = p.GetEndBit();
                        ptr[idx + 0] = (byte)(0xFF);       // B
                        ptr[idx + 1] = (byte)(0x00);  // G
                        ptr[idx + 2] = (byte)(dir ? 0x00: 0xFF); // R
                        ptr[idx + 3] = (byte)(0xFF); // A
                    }
                }
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }
}