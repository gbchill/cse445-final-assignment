using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TravelPlanningServices
{
    public class WordFilter : IWordFilter
    {
        // list of common stop words to filter out
        private readonly HashSet<string> _stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "a", "an", "and", "are", "as", "at", "be", "by", "for", "from",
            "has", "he", "in", "is", "it", "its", "of", "on", "that", "the",
            "to", "was", "were", "will", "with", "i", "me", "my", "myself",
            "we", "our", "ours", "ourselves", "you", "your", "yours", "yourself",
            "yourselves", "this", "these", "those", "am", "been", "being", "have",
            "having", "do", "does", "did", "doing", "would", "should", "could",
            "ought", "i'm", "you're", "he's", "she's", "it's", "we're", "they're",
            "i've", "you've", "we've", "they've", "i'd", "you'd", "he'd", "she'd",
            "we'd", "they'd", "i'll", "you'll", "he'll", "she'll", "we'll", "they'll",
            "isn't", "aren't", "wasn't", "weren't", "hasn't", "haven't", "hadn't",
            "doesn't", "don't", "didn't", "won't", "wouldn't", "shan't", "shouldn't",
            "can't", "cannot", "couldn't", "mustn't", "let's", "that's", "who's",
            "what's", "here's", "there's", "when's", "where's", "why's", "how's",
            "or", "not"
        };

        public string FilterStopWords(string inputText)
        {
            // check for null or empty input
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return string.Empty;
            }

            try
            {
                // remove html/xml tags
                string textWithoutTags = RemoveHtmlTags(inputText);

                // split text into words
                string[] words = Regex.Split(textWithoutTags, @"\W+")
                                      .Where(w => !string.IsNullOrEmpty(w))
                                      .ToArray();

                // filter out stop words
                string[] filteredWords = words.Where(word => !_stopWords.Contains(word))
                                             .ToArray();

                // join words back together with spaces
                return string.Join(" ", filteredWords);
            }
            catch (Exception ex)
            {
                // log the error
                Console.WriteLine($"Error in FilterStopWords: {ex.Message}");
                return "Error processing text: " + ex.Message;
            }
        }

        public string[] GetTopContentWords(string inputText, int count)
        {
            if (string.IsNullOrWhiteSpace(inputText) || count <= 0)
            {
                return new string[0];
            }

            try
            {
                // first filter out stop words
                string filteredText = FilterStopWords(inputText);

                // split into words
                string[] words = Regex.Split(filteredText, @"\W+")
                                      .Where(w => !string.IsNullOrEmpty(w))
                                      .ToArray();

                // count word frequencies using a dictionary
                Dictionary<string, int> wordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                foreach (string word in words)
                {
                    // increment count if word exists in dictionary, otherwise add with count 1
                    if (wordCounts.ContainsKey(word))
                    {
                        wordCounts[word]++;
                    }
                    else
                    {
                        wordCounts[word] = 1;
                    }
                }

                // get top N words by frequency
                return wordCounts.OrderByDescending(pair => pair.Value)
                                .Take(count)
                                .Select(pair => pair.Key)
                                .ToArray();
            }
            catch (Exception ex)
            {
                // log the error
                Console.WriteLine($"Error in GetTopContentWords: {ex.Message}");
                return new[] { "Error processing text: " + ex.Message };
            }
        }

        public int CountContentWords(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return 0;
            }

            try
            {
                // first filter out stop words
                string filteredText = FilterStopWords(inputText);

                // count remaining words
                string[] words = Regex.Split(filteredText, @"\W+")
                                      .Where(w => !string.IsNullOrEmpty(w))
                                      .ToArray();

                return words.Length;
            }
            catch (Exception ex)
            {
                // log the error 
                Console.WriteLine($"Error in CountContentWords: {ex.Message}");
                return -1; // indicate error
            }
        }

        private string RemoveHtmlTags(string html)
        {
            // remove html/xml tags using regex
            string withoutTags = Regex.Replace(html, "<.*?>", string.Empty);

            // decode html entities
            withoutTags = System.Net.WebUtility.HtmlDecode(withoutTags);

            return withoutTags;
        }
    }
}