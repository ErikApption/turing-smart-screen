namespace TuringSmartScreenLib;

internal abstract class ScreenWrapperRevisionB : ScreenBase
{
    private readonly TuringSmartScreenRevisionB screen;

    protected ScreenWrapperRevisionB(TuringSmartScreenRevisionB screen)
        : base(screen.Width, screen.Height, ScreenOrientation.Portrait)
    {
        this.screen = screen;
    }

    public override void Dispose() => screen.Dispose();

    public override void Reset()
    {
        // Do Nothing
    }

    public override void Clear()
    {
        // TODO Emulation ?
    }

    public override void ScreenOff()
    {
        // Emulation
        SetBrightness(0);
    }

    public override void ScreenOn()
    {
        // Emulation
        SetBrightness(100);
    }

    public override void SetBrightness(byte level) => screen.SetBrightness(CalcBrightness(level));

    protected abstract byte CalcBrightness(byte value);

    protected override bool IsRotated(ScreenOrientation orientation) =>
        orientation is ScreenOrientation.Landscape or ScreenOrientation.ReverseLandscape;

    protected override bool SetOrientation(ScreenOrientation orientation)
    {
        switch (orientation)
        {
            case ScreenOrientation.Portrait:
            case ScreenOrientation.ReversePortrait:
                screen.SetOrientation(TuringSmartScreenRevisionB.Orientation.Portrait);
                return true;
            case ScreenOrientation.Landscape:
            case ScreenOrientation.ReverseLandscape:
                screen.SetOrientation(TuringSmartScreenRevisionB.Orientation.Landscape);
                return true;
        }

        return false;
    }

    public override IScreenBuffer CreateBuffer(int width, int height) => new ScreenBufferBgr353(width, height);

    public override bool DisplayBuffer(int x, int y, IScreenBuffer buffer) => screen.DisplayBitmap(x, y, ((ScreenBufferBgr353)buffer).Buffer, buffer.Width, buffer.Height, IsReverse());

    private bool IsReverse() => Orientation is ScreenOrientation.ReversePortrait or ScreenOrientation.ReverseLandscape;
}

internal sealed class ScreenWrapperRevisionB0 : ScreenWrapperRevisionB
{
    public ScreenWrapperRevisionB0(TuringSmartScreenRevisionB screen)
        : base(screen)
    {
    }

    protected override byte CalcBrightness(byte value) => value == 0 ? (byte)0 : (byte)1;
}

internal sealed class ScreenWrapperRevisionB1 : ScreenWrapperRevisionB
{
    public ScreenWrapperRevisionB1(TuringSmartScreenRevisionB screen)
        : base(screen)
    {
    }

    protected override byte CalcBrightness(byte value) => (byte)((float)value / 100 * 255);
}
