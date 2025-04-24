using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Parsing
{
    internal class CustomErrorListener<TSymbol> : IAntlrErrorListener<TSymbol>
    {
        #region IAntlrErrorListener<TSymbol> Implementation
        public void SyntaxError(TextWriter output, IRecognizer recognizer, TSymbol offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new Exception($"line {line}:{charPositionInLine} {msg}");
        }
        #endregion


    }
}
