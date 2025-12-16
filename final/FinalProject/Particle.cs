using System.Numerics;

public struct Particle
{
    /*  KEY
    0 = Air
    1 = Sand - density = 6
    2 = Water - desnity = 3
    3 = Gas - density = 0
    4 = Seed - density = 4
    5 = Plant
    6 = Flame
    7 = Stone
    */
    static readonly uint[] _materialColor = new uint[]
    {
        0x00000000, // empty
        0xffb8b154, // sand
        0xff4787ed, // water
        0x50505050, // gas
        0xff6b6336, // seed
        0xff2e7025 // vine
    };
    static readonly byte[] _materialDensity = new byte[]
    {
        0, // empty
        6, // sand
        3, // water
        1, // gas
        4, // seed
        255 // vine
    };
    private byte _material;
    // 3 bits lifetime -- 3 bits density -- 1 bit fuel - 1 bit water flipping
    private byte variables;
    public uint _color;
    public Vector2 velocity = new Vector2(0, 0);
    public Vector2 oldVelocity = new Vector2(0, 0); // Used for debug
    const int Bitmask = 1;
    public Particle(byte material)
    {
        Random random = new Random();
        _material = material;
        if(material == 1)
            _color = AddRandomOffset(_materialColor[1], random, 40);
        else
            _color = _materialColor[material];
        // initialize variables;
        variables = (byte)(_materialDensity[material] << 5);
        if(material == 2 || material == 3)
        {
            int randomDir = random.Next(2);
            variables |= (byte)(randomDir << 0);
        }
    }
    public static uint AddRandomOffset(uint baseColor, Random rand, int maxOffset = 20)
    {
        // Extract the color info
        byte a = (byte)((baseColor >> 24) & 0xFF);
        byte r = (byte)((baseColor >> 16) & 0xFF);
        byte g = (byte)((baseColor >> 8) & 0xFF);
        byte b = (byte)(baseColor & 0xFF);

        // add random offsets
        int random = rand.Next(-maxOffset, maxOffset + 1);
        r = (byte)Math.Clamp(r + random, 0, 255);
        g = (byte)Math.Clamp(g + random, 0, 255);
        b = (byte)Math.Clamp(b + random, 0, 255);

        // return finished color
        return (uint)((a << 24) | (r << 16) | (g << 8) | b);
    }
    
    public byte GetDensity()
    {
        return (byte)(variables >> 5);
    }

    public bool GetEndBit()
    {
        return (variables & 1) == 1;
    }
    public void SetEndBit()
    {
        variables ^= Bitmask;
    }
    public byte GetMaterial()
    {
        return _material;
    }
}