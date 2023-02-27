using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordFrequencyReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filePaths = { "file1.txt", "file2.txt", "file3.txt" };

            Func<string, IEnumerable<string>> tokenizer = text =>
            {
                // Токенізація тексту на окремі слова
                return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            };

            Func<IEnumerable<string>, IDictionary<string, int>> frequencyCounter = tokens =>
            {
                // Підрахунок частоти кожного слова
                var frequencyMap = new Dictionary<string, int>();
                foreach (var token in tokens)
                {
                    if (frequencyMap.ContainsKey(token))
                    {
                        frequencyMap[token]++;
                    }
                    else
                    {
                        frequencyMap[token] = 1;
                    }
                }
                return frequencyMap;
            };

            Action<IDictionary<string, int>> printFrequencyReport = frequencyMap =>
            {
                // Виведення звіту із статистикою
                Console.WriteLine("Word frequency report:");
                Console.WriteLine("=======================");
                foreach (var entry in frequencyMap.OrderByDescending(entry => entry.Value))
                {
                    Console.WriteLine("{0}: {1}", entry.Key, entry.Value);
                }
            };

            // Обчислення частоти слів у кожному файлі та створення загального звіту
            var frequencyMaps = new List<IDictionary<string, int>>();
            foreach (var filePath in filePaths)
            {
                var text = File.ReadAllText(filePath);
                var tokens = tokenizer(text);
                var frequencyMap = frequencyCounter(tokens);
                frequencyMaps.Add(frequencyMap);
            }
            var mergedFrequencyMap = frequencyMaps.SelectMany(map => map)
                .GroupBy(entry => entry.Key)
                .ToDictionary(group => group.Key, group => group.Sum(entry => entry.Value));
            printFrequencyReport(mergedFrequencyMap);

            Console.ReadLine();
        }
    }
}
