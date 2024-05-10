//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

using Area23.At.Mono.Util;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Area23.At.Mono.Calc
{
    public class CalcTerm : CalcElem
    {
        internal List<CalcElem> terms = new List<CalcElem>();
        internal List<CalcElem> sterms = new List<CalcElem>();

        public CalcElem[] Terms { get => terms.ToArray(); }
        public CalcElem[] STerms { get => sterms.ToArray(); }

        public CalcTerm(string elem) : base(elem)
        {
            bool firstDone = false, proceeded = false;
            RPNType lastType = RPNType.Invalid;
            int numLen = 0;

            if (string.IsNullOrEmpty(elem))
                throw new InvalidOperationException("null or empty math operators are not allowed");
            _elem = elem;
            string tmpElem = (string)_elem.Clone();
            while (tmpElem.Length > 0 && !String.IsNullOrEmpty(tmpElem))
            {
                if (firstDone && !proceeded)
                    break;
                if (proceeded)
                    proceeded = false;

                foreach (string _mathOp1 in MathOpUnary.validElems)
                {
                    if (tmpElem.StartsWith(_mathOp1))
                    {
                        if (!firstDone || lastType == RPNType.MathOp2)
                        {
                            MathOpUnary mathOpUnary = new MathOpUnary(_mathOp1);
                            terms.Add(mathOpUnary);
                            tmpElem = tmpElem.Substring(_mathOp1.Length);
                            if (!firstDone)
                                firstDone = true;
                            lastType = RPNType.MathOp1;
                        }
                        else throw new InvalidOperationException(
                            String.Format("Unary operator {0} must be at start or follow binary operator: {1}.", _mathOp1, _elem));
                        proceeded = true;
                        break;
                    }
                }
                if ((numLen = MathNumber.ValidateAtStart(tmpElem)) > 0)
                {
                    string numStr = tmpElem.Substring(0, numLen);
                    if (!firstDone || lastType == RPNType.MathOp1 || lastType == RPNType.MathOp2)
                    {                        
                        MathNumber mathNumber = new MathNumber(numStr);
                        terms.Add(mathNumber);
                        tmpElem = tmpElem.Substring(numLen);
                        if (!firstDone)
                            firstDone = true;
                        lastType = RPNType.Number;
                    }
                    else throw new InvalidOperationException(
                        String.Format("Mathnumber {0} must follow Unary or Binary operator: {1}", numStr, _elem));
                    proceeded = true;
                }
                foreach (string _mathOp2 in MathOpBinary.validElems)
                {
                    if (tmpElem.StartsWith(_mathOp2))
                    {
                        if (lastType == RPNType.Number)
                        {
                            MathOpBinary mathOpBinary = new MathOpBinary(_mathOp2);
                            terms.Add(mathOpBinary);
                            tmpElem = tmpElem.Substring(_mathOp2.Length);
                            lastType = RPNType.MathOp2;
                        }
                        else throw new InvalidOperationException(
                            String.Format("Math binary operator {0} must follow math number: {1}", _mathOp2, _elem));
                        proceeded = true;
                        break;
                    }                    
                }
            }
        
            if (!Validate())
                throw new InvalidOperationException(String.Format("{0} is not a valid element.", _elem));

            sterms = new List<CalcElem>(terms);
        }

        internal override bool Validate()
        {
            bool firstDone = false;
            RPNType lastType = RPNType.Invalid;

            foreach (var termElem in terms)
            {
                if (termElem is MathOpUnary)
                {
                    if (!firstDone || lastType == RPNType.MathOp2)
                    {
                        if (!firstDone) firstDone = true;
                        lastType = RPNType.MathOp1;
                    }
                    else throw new InvalidOperationException(
                            String.Format("Unary operator {0} must be at start or follow binary operator: {1}.", termElem._elem, _elem));
                }
                if (termElem is MathNumber)
                {
                    if (!firstDone || lastType == RPNType.MathOp1 || lastType == RPNType.MathOp2)
                    {
                        if (!firstDone) firstDone = true;
                        lastType = RPNType.Number;
                    }
                    else throw new InvalidOperationException(
                        String.Format("Mathnumber {0} must follow Unary or Binary operator: {1}", termElem._elem, _elem));
                }
                if (termElem is MathOpBinary)
                {
                    if (lastType == RPNType.Number)
                    {
                        lastType = RPNType.MathOp2;
                    }
                    else throw new InvalidOperationException(
                           String.Format("Math binary operator {0} must follow math number: {1}", termElem._elem, _elem));
                }
            }

            return true;
        }

        public virtual bool Validate1()
        {
            bool firstDone = false, proceeded = false;
            RPNType lastType = RPNType.Invalid;
            int numLen = 0;
            terms.Clear();

            string tmpElem = (string)_elem.Clone();
            while (tmpElem.Length > 0 && !String.IsNullOrEmpty(tmpElem))
            {
                if (firstDone && !proceeded)
                    return false;
                if (proceeded)                     
                    proceeded = false;

                foreach (string _mathOp1 in MathOpUnary.validElems)
                {
                    if (tmpElem.StartsWith(_mathOp1))
                    {
                        if (!firstDone || lastType == RPNType.MathOp2)
                        {
                            MathOpUnary mathOpUnary = new MathOpUnary(_mathOp1);
                            terms.Add(mathOpUnary);
                            tmpElem = tmpElem.Substring(_mathOp1.Length);
                            if (!firstDone)
                                firstDone = true;
                            lastType = RPNType.MathOp1;
                        }
                        else throw new InvalidOperationException(
                            String.Format("Unary operator {0} must be at start or follow binary operator: {1}.", _mathOp1, _elem));
                        proceeded = true;
                        break;
                    }
                }
                if ((numLen = MathNumber.ValidateAtStart(tmpElem)) > 0)
                {
                    string numStr = tmpElem.Substring(0, numLen);
                    if (!firstDone || lastType == RPNType.MathOp1 || lastType == RPNType.MathOp2)
                    {
                        MathNumber mathNumber = new MathNumber(numStr);
                        terms.Add(mathNumber);
                        tmpElem = tmpElem.Substring(numLen);
                        if (!firstDone)
                            firstDone = true;
                        lastType = RPNType.Number;
                    }
                    else throw new InvalidOperationException(
                        String.Format("Mathnumber {0} must follow Unary or Binary operator: {1}", numStr, _elem));
                    proceeded = true;
                }
                foreach (string _mathOp2 in MathOpBinary.validElems)
                {
                    if (tmpElem.StartsWith(_mathOp2))
                    {
                        if (lastType == RPNType.Number)
                        {
                            MathOpBinary mathOpBinary = new MathOpBinary(_mathOp2);
                            terms.Add(mathOpBinary);
                            tmpElem = tmpElem.Substring(_mathOp2.Length);
                            lastType = RPNType.MathOp2;
                        }
                        else throw new InvalidOperationException(
                            String.Format("Math binary operator {0} must follow math number: {1}", _mathOp2, _elem));                        
                        proceeded = true;
                        break;
                    }
                }
            }

            return false;
        }

        public virtual List<CalcElem> BuildSubTerms()
        {
            int ti = 0, tj = 0, th = 0;
            string subTermStr;
            CalcTerm subTermUnary, subTermBinary;
            foreach (var subterm in sterms)
            {
                if (subterm is MathNumber)
                {
                    if (subterm.Elem == "π")
                        subterm._elem = Math.PI.ToString();
                    if (subterm.Elem == "ℇ")
                        subterm._elem = Math.E.ToString();
                    if (subterm.Elem == "ℇ")
                        subterm._elem = Math.E.ToString();
                    if (subterm.Elem == "∞")
                        subterm._elem = Int64.MaxValue.ToString();
                }
            }

            for (ti = 0; ti < sterms.Count; ti++)
            {
                if (STerms[ti] is MathOpUnary && ti < (STerms.Length - 1) && STerms[ti + 1] is MathNumber)
                {
                    if (STerms[ti].Elem == "!" || STerms[ti].Elem == "2ⁿ" || STerms[ti].Elem == "10ⁿ" || STerms[ti].Elem == "2ⁿ" ||
                        STerms[ti].Elem == "x²" || STerms[ti].Elem == "x³" || STerms[ti].Elem == "²" || STerms[ti].Elem == "³" ||
                        STerms[ti].Elem == "ln" || STerms[ti].Elem == "log" || STerms[ti].Elem == "log10" || STerms[ti].Elem == "ld" ||
                        STerms[ti].Elem == "1/x" || STerms[ti].Elem == "√" || STerms[ti].Elem == "∛" || STerms[ti].Elem == "∜")
                    {
                        subTermStr = STerms[ti].Elem + STerms[ti + 1].Elem;
                        subTermUnary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(ti + 1);
                        sterms.RemoveAt(ti);
                        sterms.Insert(ti, subTermUnary);
                    }
                }

                if (STerms[ti] is MathNumber && ti < (STerms.Length - 2) && STerms[ti + 1] is MathOpBinary && STerms[ti + 2] is MathNumber)
                {
                    if (STerms[ti + 1].Elem == "^" || STerms[ti + 1].Elem == "xⁿ" || STerms[ti + 1].Elem == "mod" ||
                        STerms[ti + 1].Elem == "ⁱ√" || STerms[ti + 1].Elem == "sqrti" || STerms[ti + 1].Elem == "xⁿ" ||
                        STerms[ti + 1].Elem == "logₕ𝒂" || STerms[ti + 1].Elem == "log&#x2095;&#x1d482;" || STerms[ti + 1].Elem == "bloga")
                    {
                        subTermStr = STerms[ti].Elem + STerms[ti + 1].Elem + STerms[ti + 2].Elem;
                        subTermBinary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(ti + 2);
                        sterms.RemoveAt(ti + 1);
                        sterms.RemoveAt(ti);
                        sterms.Insert(ti, subTermBinary);
                    }
                }
            }
            for (tj = 0; tj < sterms.Count; tj++)
            {
                if (STerms[tj] is MathOpUnary && tj < (STerms.Length - 1) && STerms[tj + 1] is MathNumber)
                {
                    if (STerms[tj].Elem == "sin" || STerms[tj].Elem == "cos" || STerms[tj].Elem == "tan" || STerms[tj].Elem == "cot" ||
                        STerms[tj].Elem == "asin" || STerms[tj].Elem == "acos" || STerms[tj].Elem == "atan" || STerms[tj].Elem == "acot")
                    {
                        subTermStr = STerms[tj].Elem + STerms[tj + 1].Elem;
                        subTermUnary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(tj + 1);
                        sterms.RemoveAt(tj);
                        sterms.Insert(tj, subTermUnary);
                    }
                }

                if (STerms[tj] is MathNumber && tj < (STerms.Length - 2) && STerms[tj + 1] is MathOpBinary && STerms[tj + 2] is MathNumber)
                {
                    if (STerms[tj + 1].Elem == "*" || STerms[tj + 1].Elem == "×" || STerms[tj + 1].Elem == "/" || STerms[tj + 1].Elem == "÷")
                    {
                        subTermStr = STerms[tj].Elem + STerms[tj + 1].Elem + STerms[tj + 2].Elem;
                        subTermBinary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(tj + 2);
                        sterms.RemoveAt(tj + 1);
                        sterms.RemoveAt(tj);
                        sterms.Insert(tj, subTermBinary);

                    }
                }
            }
            for (th = 0; tj < sterms.Count; th++)
            {
                if (STerms[th] is MathOpUnary && th < (STerms.Length - 1) && STerms[th + 1] is MathNumber)
                {
                    if (STerms[th].Elem == "%" || STerms[th].Elem == "‰" || STerms[th].Elem == "abs" || STerms[th].Elem == "|x|")
                    {
                        subTermStr = STerms[th].Elem + STerms[th + 1].Elem;
                        subTermUnary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(th + 1);
                        sterms.RemoveAt(th);
                        sterms.Insert(th, subTermUnary);
                    }
                }

                if (STerms[th] is MathNumber && th < (STerms.Length - 2) && STerms[th + 1] is MathOpBinary && STerms[th + 2] is MathNumber)
                {
                    if (STerms[th + 1].Elem == "+" || STerms[th + 1].Elem == "-")
                    {
                        subTermStr = STerms[th].Elem + STerms[th + 1].Elem + STerms[th + 2].Elem;
                        subTermBinary = new CalcTerm(subTermStr);
                        sterms.RemoveAt(th + 2);
                        sterms.RemoveAt(th + 1);
                        sterms.RemoveAt(th);
                        sterms.Insert(th, subTermBinary);
                    }
                }
            }
            return sterms;
        }

        public virtual List<CalcElem> EvaluateTerms(RPNRad rad = RPNRad.DEG)
        {
            int ti = 0, tj = 0, th = 0;
            long lresult = 1, lnum0 = 0, lnum1 = 0;
            double dresult = 1, dnum0 = 0, dnum1 = 0;
            CalcTerm resultTermNumber;

            for (ti = 0; ti < sterms.Count; ti++)
            {
                if (STerms[ti] is MathOpUnary && ti < (STerms.Length - 1) && STerms[ti + 1] is MathNumber)
                {
                    if (STerms[ti].Elem == "!" || STerms[ti].Elem == "2ⁿ" || STerms[ti].Elem == "10ⁿ" || STerms[ti].Elem == "2ⁿ" ||
                        STerms[ti].Elem == "x²" || STerms[ti].Elem == "x³" || STerms[ti].Elem == "²" || STerms[ti].Elem == "³" ||
                        STerms[ti].Elem == "ln" || STerms[ti].Elem == "log" || STerms[ti].Elem == "log10" || STerms[ti].Elem == "ld" ||
                        STerms[ti].Elem == "1/x" || STerms[ti].Elem == "√" || STerms[ti].Elem == "∛" || STerms[ti].Elem == "∜")
                    {
                        lnum0 = Convert.ToInt64(STerms[ti + 1].Elem);
                        dnum0 = Convert.ToDouble(STerms[ti + 1].Elem);
                        dresult = 1;
                        lresult = 1;

                        switch (STerms[ti].Elem)
                        {
                            case "!":
                                for (int ix = 1; ix < lnum0; ix++)
                                    lresult *= ix;
                                resultTermNumber = new CalcTerm(lresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "2ⁿ":
                                dresult = Math.Pow(10, dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "10ⁿ":
                                dresult = Math.Pow(10, dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "x²":
                            case "²":
                                dresult = dnum0 * dnum0;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "x³":
                            case "³":
                                dresult = dnum0 * dnum0 * dnum0;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "ln":
                                dresult = Math.Log(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "log10":
                                dresult = Math.Log10(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "ld":
                                dresult = (Math.Log10(dnum0) / Math.Log10(2));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "1/x":
                                dresult = (1 / dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "√":
                                dresult = Math.Sqrt(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "∛":
                                dresult = Math.Pow(dnum0, ((double)(1 / 3)));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "∜":
                                dresult = Math.Pow(dnum0, ((double)(1 / 4)));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                        }
                    }
                }

                if (STerms[ti] is MathNumber && ti < (STerms.Length - 2) && STerms[ti + 1] is MathOpBinary && STerms[ti + 2] is MathNumber)
                {
                    if (STerms[ti + 1].Elem == "^" || STerms[ti + 1].Elem == "xⁿ" || STerms[ti + 1].Elem == "mod" ||
                        STerms[ti + 1].Elem == "ⁱ√" || STerms[ti + 1].Elem == "sqrti" ||
                        STerms[ti + 1].Elem == "logₕ𝒂" || STerms[ti + 1].Elem == "log&#x2095;&#x1d482;" || STerms[ti + 1].Elem == "bloga")
                    {
                        dnum0 = Convert.ToDouble(STerms[ti].Elem);
                        dnum1 = Convert.ToDouble(STerms[ti + 2].Elem);
                        try
                        {
                            lnum0 = Convert.ToInt64(STerms[th].Elem);
                            lnum1 = Convert.ToInt64(STerms[th + 2].Elem);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                            lnum0 = 0;
                            lnum1 = 0;
                        }
                        dresult = 1;
                        lresult = 1;

                        switch (STerms[ti + 1].Elem)
                        {
                            case "mod":
                                lresult = lnum0 % lnum1;
                                resultTermNumber = new CalcTerm(lresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "^":
                            case "xⁿ":
                                dresult = Math.Pow(dnum0, dnum1);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "ⁱ√":
                            case "sqrti":
                                dresult = Math.Pow(dnum0, ((double)(1 / dnum1)));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                            case "logₕ𝒂":
                            case "log&#x2095;&#x1d482;":
                            case "bloga":
                                dresult = (Math.Log10(dnum0) / Math.Log10(dnum1));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti + 1);
                                sterms.RemoveAt(ti);
                                sterms.Insert(ti, resultTermNumber);
                                break;
                        }
                    }
                }
            }
            for (tj = 0; tj < sterms.Count; tj++)
            {
                if (STerms[tj] is MathOpUnary && tj < (STerms.Length - 1) && STerms[tj + 1] is MathNumber)
                {
                    if (STerms[tj].Elem == "sin" || STerms[tj].Elem == "cos" || STerms[tj].Elem == "tan" || STerms[tj].Elem == "cot" ||
                        STerms[tj].Elem == "asin" || STerms[tj].Elem == "acos" || STerms[tj].Elem == "atan" || STerms[tj].Elem == "acot")
                    {
                        lnum0 = Convert.ToInt64(STerms[tj + 1].Elem);
                        dnum0 = Convert.ToDouble(STerms[tj + 1].Elem);
                        dresult = 1;
                        lresult = 1;
                        if (STerms[tj].Elem == "sin" || STerms[tj].Elem == "cos" || STerms[tj].Elem == "tan" || STerms[tj].Elem == "cot")
                        {
                            if (rad == RPNRad.GRD)
                                dnum0 = dnum0 * Math.PI / 200;
                            else if (rad == RPNRad.DEG)
                                dnum0 = dnum0 * Math.PI / 180;
                        }
                        switch (STerms[tj].Elem)
                        {
                            case "sin":
                                dresult = Math.Sin(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "cos":
                                dresult = Math.Cos(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "tan":
                                dresult = Math.Tan(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "cot":
                                dresult = (1 / Math.Tan(dnum0));
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "asin":
                                dresult = Math.Asin(dnum0);
                                if (rad == RPNRad.GRD)
                                    dresult = dresult * 200 / Math.PI;
                                else if (rad == RPNRad.DEG)
                                    dresult = dresult * 180 / Math.PI;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "acos":
                                dresult = Math.Acos(dnum0);
                                if (rad == RPNRad.GRD)
                                    dresult = dresult * 200 / Math.PI;
                                else if (rad == RPNRad.DEG)
                                    dresult = dresult * 180 / Math.PI;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "atan":
                                dresult = Math.Atan(dnum0);
                                if (rad == RPNRad.GRD)
                                    dresult = dresult * 200 / Math.PI;
                                else if (rad == RPNRad.DEG)
                                    dresult = dresult * 180 / Math.PI;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "acot":
                                dresult = (1 / Math.Atan(1 / dnum0));
                                if (rad == RPNRad.GRD)
                                    dresult = dresult * 200 / Math.PI;
                                else if (rad == RPNRad.DEG)
                                    dresult = dresult * 180 / Math.PI;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                        }
                    }
                }

                if (STerms[tj] is MathNumber && tj < (STerms.Length - 2) && STerms[tj + 1] is MathOpBinary && STerms[tj + 2] is MathNumber)
                {
                    if (STerms[tj + 1].Elem == "*" || STerms[tj + 1].Elem == "×" || STerms[tj + 1].Elem == "/" || STerms[tj + 1].Elem == "÷")
                    {
                        dnum0 = Convert.ToDouble(STerms[tj].Elem);
                        dnum1 = Convert.ToDouble(STerms[tj + 2].Elem);
                        try
                        {
                            lnum0 = Convert.ToInt64(STerms[th].Elem);
                            lnum1 = Convert.ToInt64(STerms[th + 2].Elem);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                            lnum0 = 0;
                            lnum1 = 0;
                        }
                        dresult = 1;
                        lresult = 1;

                        switch (STerms[tj + 1].Elem)
                        {
                            case "*":
                            case "×":
                                dresult = dnum0 * dnum1;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                            case "/":
                            case "÷ⁿ":
                                dresult = dnum0 / dnum1;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj + 1);
                                sterms.RemoveAt(tj);
                                sterms.Insert(tj, resultTermNumber);
                                break;
                        }
                    }
                }
            }
            for (th = 0; th < sterms.Count; th++)
            {
                if (STerms[th] is MathOpUnary && th < (STerms.Length - 1) && STerms[th + 1] is MathNumber)
                {
                    if (STerms[th].Elem == "%" || STerms[th].Elem == "‰" || STerms[th].Elem == "abs" || STerms[th].Elem == "|x|")
                    {
                        lnum0 = Convert.ToInt64(STerms[th + 1].Elem);
                        dnum0 = Convert.ToDouble(STerms[th + 1].Elem);
                        dresult = 1;
                        lresult = 1;
                        switch (STerms[th].Elem)
                        {
                            case "%":
                                dresult = dnum0 / 100;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th);
                                sterms.Insert(th, resultTermNumber);
                                break;
                            case "‰":
                                dresult = dnum0 / 1000;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th);
                                sterms.Insert(th, resultTermNumber);
                                break;
                            case "abs":
                            case "|x|":
                                dresult = Math.Abs(dnum0);
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th);
                                sterms.Insert(th, resultTermNumber);
                                break;
                        }
                    }
                }

                if (STerms[th] is MathNumber && th < (STerms.Length - 2) && STerms[th + 1] is MathOpBinary && STerms[th + 2] is MathNumber)
                {
                    if (STerms[th + 1].Elem == "+" || STerms[th + 1].Elem == "-")
                    {                        
                        dnum0 = Convert.ToDouble(STerms[th].Elem);
                        dnum1 = Convert.ToDouble(STerms[th + 2].Elem);
                        try
                        {
                            lnum0 = Convert.ToInt64(STerms[th].Elem);
                            lnum1 = Convert.ToInt64(STerms[th + 2].Elem);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                            lnum0 = 0;
                            lnum1 = 0;
                        }
                        dresult = 1;
                        lresult = 1;
                    
                        switch (STerms[th + 1].Elem)
                        {
                            case "+":
                                dresult = dnum0 + dnum1;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th);
                                sterms.Insert(th, resultTermNumber);
                                break;
                            case "-":                           
                                dresult = dnum0 - dnum1;
                                resultTermNumber = new CalcTerm(dresult.ToString());
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th + 1);
                                sterms.RemoveAt(th);
                                sterms.Insert(th, resultTermNumber);
                                break;
                        }
                    }
                }
            }
            return sterms;
        }

        public override string ToString()
        {
            string calcTermString = string.Empty;
            foreach (var sterm in sterms)
            {
                calcTermString += sterm.Elem;
            }
            return calcTermString;
        }


    }
}