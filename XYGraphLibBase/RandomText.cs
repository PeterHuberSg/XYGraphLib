using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XYGraphLib {

  /// <summary>
  /// Stores a string and a value
  /// </summary>
  public record StringValue(string Text, double Value); 


  /// <summary>
  /// Creates random strings based on English letter probability
  /// </summary>
  public static class RandomText {

    static string randomToChar = "aaaaaaaaaaaaaaaaaaaabbbcccccccdddddddddddeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeefffffggggghhhhhhhhhhhhhhhiiiiiiiiiiiiiiiiijkllllllllllmmmmmmnnnnnnnnnnnnnnnnnoooooooooooooooooooppppqrrrrrrrrrrrrrrrsssssssssssssssstttttttttttttttttttttttuuuuuuuvvwwwwwwxyyyyyz      ";


    /// <summary>
    /// returns an array of randomly created strings.
    /// /// </summary>
    /// <param name="stringsCount">number of strings created</param>
    /// <param name="maxStringLength">max number of chars in random string</param>
    public static string[] GetStrings(int stringsCount, int maxStringLength) {
      var random = new Random();
      var bytes = new byte[maxStringLength];
      var stringBuilder = new StringBuilder(maxStringLength);
      var strings = new string[stringsCount];
      for (int stringsIndex = 0; stringsIndex < stringsCount; stringsIndex++) {
        var charCount = random.Next(maxStringLength-1)+1;
        var bytesSpan = bytes.AsSpan()[0..charCount];
        random.NextBytes(bytesSpan);
        foreach (var charByte in bytesSpan) {
          stringBuilder.Append(randomToChar[charByte]);
        }
        strings[stringsIndex] = stringBuilder.ToString();
        stringBuilder.Clear();
      }
      return strings;
    }


    /// <summary>
    /// returns an array of randomly created strings. Each string is in a tuple together with a random value, which
    /// fluctuates around 0.
    /// /// </summary>
    /// <param name="stringsCount">number of strings created</param>
    /// <param name="maxStringLength">max number of chars in random string</param>
    public static StringValue[] GetStringValues(int stringsCount, int maxStringLength) {
      var random = new Random();
      var bytes = new byte[maxStringLength];
      var stringBuilder = new StringBuilder(maxStringLength);
      var stringValues = new StringValue[stringsCount];
      var dataValue = 0.0;
      for (int stringIndex = 0; stringIndex < stringsCount; stringIndex++) {
        var charCount = random.Next(maxStringLength-1)+1;
        var bytesSpan = bytes.AsSpan()[0..charCount];
        random.NextBytes(bytesSpan);
        foreach (var charByte in bytesSpan) {
          stringBuilder.Append(randomToChar[charByte]);
        }
        dataValue += random.NextDouble() - 0.5;
        stringValues[stringIndex] = new StringValue(stringBuilder.ToString(), dataValue);
        stringBuilder.Clear();
      }
      return stringValues;
    }
  }
}
