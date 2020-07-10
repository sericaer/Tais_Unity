using System;
using SyntaxAnaylize;

internal class Expr_Exception : Exception
{
    internal Expr_Exception(string info) : base(info)
    {

    }

    internal Expr_Exception(string info, Value value) : base(info)
    {

    }

    internal Expr_Exception(string info, Value value, Exception parent) : base(info, parent)
    {

    }

    public override string Message
    {
        get
        {
            if(value != null)
            {
                return $"{base.Message} when parse {value} in line:{value.line}";
            }
            else
            {
                return base.Message;
            }
        }
    }

    internal Value value;
}
