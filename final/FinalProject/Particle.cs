using System.Numerics;

public struct Particle
{
    /*  KEY
    0 = Air
    1 = Sand - density = 6
    2 = Water - desnity = 3
    3 = Stone
    4 = Flame
    5 = Gas - density = 0
    6 = Seed - density = 4
    7 = Plant
    */
    static readonly uint[] _materialColor = new uint[4]
    {
        0x00000000, // empty
        0xffb8b154, // sand
        0xff4787ed, // water
        0xff4707ed // oil
    };
    static readonly byte[] _materialDensity = new byte[4]
    {
        0, // empty
        6, // sand
        3, // water
        2 // oil
    };
    public byte _material;
    // 3 bits lifetime -- 3 bits density -- 1 bit color variance - 1 bit fuel
    public byte variables;
    public uint _color;
    public Vector2 velocity = new Vector2(0, 0);
    public Vector2 oldVelocity = new Vector2(0, 0);
    public bool updateFlag = false;
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
}