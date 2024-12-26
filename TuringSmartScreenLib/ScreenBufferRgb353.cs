namespace TuringSmartScreenLib;

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0032
// ReSharper disable ConvertToAutoProperty
public sealed class ScreenBufferRgb353 : IScreenBuffer
{
    private readonly int width;

    private readonly int height;

    private byte[] buffer;

    public int Width => width;

    public int Height => height;

    internal byte[] Buffer => buffer;

    public ScreenBufferRgb353(int width, int height)
    {
        this.width = width;
        this.height = height;
        buffer = ArrayPool<byte>.Shared.Rent(width * height * 2);
        buffer.AsSpan().Clear();
    }

    public void Dispose()
    {
        if (buffer.Length > 0)
        {
            ArrayPool<byte>.Shared.Return(buffer);
            buffer = [];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPixel(int x, int y, byte r, byte g, byte b)
    {
        var rgb = ((r >> 3) << 11) | ((g >> 2) << 5) | (b >> 3);
        var offset = ((y * width) + x) * 2;
        BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(offset), (short)rgb);
    }

    public void Clear() => Clear(0, 0, 0);

    public void Clear(byte r, byte g, byte b)
    {
        var rgb = ((r >> 3) << 11) | ((g >> 2) << 5) | (b >> 3);
        BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), (short)rgb);

        var length = 2;
        var size = Width * Height * 2;
        while (length < size - length)
        {
            buffer.AsSpan(0, length).CopyTo(buffer.AsSpan(length));

            length += length;
        }

        if (length < size)
        {
            buffer.AsSpan(0, size - length).CopyTo(buffer.AsSpan(length));
        }
    }
}
// ReSharper restore ConvertToAutoProperty
#pragma warning restore IDE0032
