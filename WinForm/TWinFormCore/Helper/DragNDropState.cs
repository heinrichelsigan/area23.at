using System.ComponentModel;

namespace Area23.At.WinForm.TWinFormCore.Helper
{
    [DefaultValue(None)]
    public enum DragNDropState
    {
        None = 0x0,
        DragEnter = 0x1,
        DragOver = 0x2,
        DragLeave = 0x4,
        Drop = 0x8
    }

}
