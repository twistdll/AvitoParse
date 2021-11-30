using System.Collections.Generic;
using System.Linq;

namespace avitoParse
{
    static class AdsListConstructer
    {
        public static List<string> Construct(List<string> links, List<string> names, List<string> prices)
        {
            int numbersColumnSize = links.Count().ToString().Length;
            int linksColumnSize = links.Max(e => e.Length);
            int namesColumnSize = names.Max(e => e.Length);
            int pricesColumnSize = prices.Max(e => e.Length);

            List<string> adsList = new List<string>();

            adsList.Add($"№    {string.Concat(Enumerable.Repeat(" ", numbersColumnSize - 1))}|" +
                        $"Ссылка{string.Concat(Enumerable.Repeat(" ", linksColumnSize - 6))}|" +
                        $"Название{string.Concat(Enumerable.Repeat(" ", namesColumnSize - 8))}|" +
                        $"Цена{string.Concat(Enumerable.Repeat(" ", pricesColumnSize - 4))}");

            adsList.Add(string.Concat(Enumerable.Repeat("-", adsList[0].Length + 1)));

            for (int i = 0; i < links.Count; i++)
            {
                adsList.Add($"[{(i + 1)}]{string.Concat(Enumerable.Repeat(" ", (numbersColumnSize - (i+1).ToString().Length) + 2))}|" +
                            $"{links[i] + string.Concat(Enumerable.Repeat(" ", (linksColumnSize - links[i].Length)))}|" +
                            $"{names[i] + string.Concat(Enumerable.Repeat(" ", (namesColumnSize - names[i].Length)))}|" +
                            $"{prices[i] + string.Concat(Enumerable.Repeat(" ", (pricesColumnSize - prices[i].Length)))}|");
            }

            return adsList;
        }
    }
}
