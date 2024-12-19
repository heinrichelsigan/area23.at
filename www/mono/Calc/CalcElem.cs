using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Calc
{
    public abstract class CalcElem
    {
        protected internal string _elem = string.Empty;
        

        public CalcElem(string elem)
        {
            if (string.IsNullOrEmpty(elem))
                throw new InvalidOperationException("null or empty math operators are not allowed");
            _elem = elem;
            if (_elem == null || _elem.Length < 1)
                throw new InvalidOperationException(String.Format("{0} is a null or empty string element.", elem));
        }       
    }

    public abstract class MathElement : CalcElem
    {
        protected internal static string[] validElems = { "" };

        public virtual string Elem { get => _elem; }

        public MathElement(string elem) : base(elem) 
        {
            if (!validElems.Contains(_elem))
                throw new InvalidOperationException(String.Format("{0} is not a valid prooved element.", elem));
        }

        internal virtual bool Validate()
        {
            return (_elem != null && _elem.Length > 0 && validElems.Contains(_elem));
        }
    }

    public class MathOpUnary : MathElement
    {        
        protected internal static new string[] validElems = { 
            "±", "x²", "²", "x³", "³", "2ⁿ", "10ⁿ", "!",
            "ln", "log", "log10", "ld", "1/x",
            "√", "∛", "∜", "abs", "|x|", "%", "‰",
            "sin", "cos", "tan", "cot", "asin", "acos", "atan", "acot" };

        public MathOpUnary(string elem) : base(elem) { }

        internal override bool Validate()
        {
            return (_elem != null && _elem.Length > 0 && validElems.Contains(_elem));
        }
    }

    public class MathOpBinary : MathElement
    {
        protected internal static new string[] validElems = { "+", "-", "*", "×", "/", "÷", 
            "^", "xⁿ", "mod", "ⁱ√", "sqrti", "logₕ𝒂", "log&#x2095;a", "bloga" };

        public MathOpBinary(string elem) : base(elem) { }

        internal override bool Validate()
        {
            return (_elem != null && _elem.Length > 0 && validElems.Contains(_elem));
        }
    }


    public class MathNumber : MathElement
    {
        internal static string validChars = "0123456789ABCDEF.,";
        protected internal static new string[] validElems = { "ℇ", "π" };

        public MathNumber(string elem) : base(elem) { }

        internal override bool Validate()
        {
            string parseNumber = string.Empty;
            if (_elem != null && _elem.Length > 0)
            {
                parseNumber = _elem.ToString();
                if (_elem.StartsWith("-") && _elem.Length > 1 && (_elem[1] != '0' || _elem[2] == '.'))
                    parseNumber = _elem.TrimStart('-');
                               
                if (validElems.Contains(parseNumber))
                    return true;

                parseNumber = parseNumber.Trim(validChars.ToCharArray());
                if (parseNumber == string.Empty || parseNumber.Length == 0)
                    return true;
            }
            return false;
        }

        internal static int ValidateAtStart(string term)
        {
            string parseNumber = string.Empty;
            int reaLen = 0;

            if (term != null && term.Length > 0)
            {                                
                parseNumber = term.ToString();
                if (term.StartsWith("-") && term.Length > 1 && (term[1] != '0' || term[2] == '.'))
                {
                    parseNumber = term.TrimStart('-');
                    reaLen++;
                }

                if (parseNumber[0] == 'ℇ' || parseNumber[0] == 'π')
                    return ++reaLen;
                
                int len = parseNumber.Length;
                
                parseNumber = parseNumber.TrimStart(validChars.ToCharArray());
                if (parseNumber.Length != len)
                {
                    reaLen += (len - parseNumber.Length);
                    return reaLen;
                }

                reaLen = 0;
            }

            return reaLen;
        }
    }
}