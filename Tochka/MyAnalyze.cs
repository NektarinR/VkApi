using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tochka
{
    public class MyAnalyze : ITextAnalyze
    {
        /*
            Схема работы:
                Снала создается словарь с символами указанными в template
                Происходит подсчет символом коллекции texts, и тут же считается общее число символов
                В конце производится вычисление частотности
        */

        /// <summary>
        /// Функция считает частотность букв 
        /// </summary>
        /// <param name="texts">Коллекция текстов</param>
        /// <param name="template">Набор символом, для которых считается частотность</param>
        /// <returns></returns>
        public IDictionary<char, double> FindFrequency(IEnumerable<string> texts, string template)
        {
            var result = new Dictionary<char, double>();
            uint count = 0;
            foreach (var ch in template)
                if (!result.ContainsKey(ch))
                    result.Add(ch, 0);
            foreach (var t in texts)
            {
                count += (uint)t.Length;
                foreach (var symbol in t)
                    if (result.ContainsKey(symbol))
                        result[symbol]++;
            }
            if (count != 0)
                foreach (var elemnt in template)
                    result[elemnt] = Math.Round(result[elemnt]/count, 5, MidpointRounding.AwayFromZero);
            return result;
        }
    }
}
