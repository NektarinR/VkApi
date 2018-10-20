using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tochka
{
    public interface ITextAnalyze
    {
        IDictionary<char, double> FindFrequency(IEnumerable<string> texts,string template);
    }
}
