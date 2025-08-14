namespace Area23.At.Mono.Calc
{
    public enum RPNType
    {
        Invalid = -1,
        Number = 0,
        MathOp1 = 1,
        MathOp2 = 2,
        BracketOpening = 3,
        BracketClosing = 4
    }

    public enum RPNRad
    {
        DEG = 0,
        RAD = 1,
        GRD = 2
    }

}