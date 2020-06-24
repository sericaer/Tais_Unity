using System;
using SyntaxAnaylize;

internal class Expr_Exception : Exception
{
    internal Expr_Exception(string info, Value value) : base(info)
    {

    }

    public override string Message
    {
        get
        {
            return $"{base.Message} when parse {value}";
        }
    }

    internal Value value;
}
