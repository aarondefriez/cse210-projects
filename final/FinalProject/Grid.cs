using System.Drawing.Imaging;
using System.Numerics;

public class Grid
{
    ParticleRules[] rules = new ParticleRules[4];
    // 0,0 is top left to follow winform format
    public Particle[,] _grid;
    public Particle[,] _grid1;

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
                grid[x,y] = new Particle(0);
            }
        }
        bitmap = new Bitmap(_grid.GetLength(0), _grid.GetLength(1), PixelFormat.Format32bppArgb);
    }

    public void InitializeRules()
    {
        rules[0] = new EmptyRules();
        rules[1] = new SandRules();
        rules[2] = new EmptyRules();
        rules[3] = new LiquidRules();
    }

    public void Step()
    {
        // First itteration check where pixels want to move
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for(int x = 0; x < _grid.GetLength(0); x++)
            {
                byte material = _grid[x, y]._material;
                rules[material].UpdateParticle(this, x, y);
            }
        }
        // Second itteration move the pixels
        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for(int x = 0; x < _grid.GetLength(0); x++)
            {
                if(_grid[x, y]._material == 1)
                {
                    Vector2 v = _grid[x, y].velocity;
                    _grid[x, y].oldVelocity = _grid[x, y].velocity;
                    _grid[x, y].velocity = Vector2.Zero;
                    Swap(x, y, (int)v.X, (int)v.Y);
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
                if (_grid[x, y]._material == 0)
                    _grid[x, y] = new Particle(material);
                else if(material == 0) // if eraesing
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
                    if (Program.debug && p._material!= 0)
                    {
                        byte xv = (byte)p.oldVelocity.X;
                        byte yv = (byte)p.oldVelocity.Y;
                        ptr[idx + 0] = (byte)(xv * 5);       // B
                        ptr[idx + 1] = (byte)(0xFF);  // G
                        ptr[idx + 2] = (byte)(yv * 5); // R
                        ptr[idx + 3] = (byte)(0xFF); // A
                    }
                }
            }
        }

        bitmap.UnlockBits(data);
        return bitmap;
    }
}