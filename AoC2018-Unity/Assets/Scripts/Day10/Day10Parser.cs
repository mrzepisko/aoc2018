using System.Text.RegularExpressions;
using Unity.Mathematics;

internal class Day10Parser {
    private const char EMPTY = '.';
    private const char POINT = '#';
    private const string pattern = @"(?>position=\<(?'pos_x'[\s-]\d+),\s+?(?'pos_y'[\s-]\d+)\>)\s+(?>velocity=\<(?'vel_x'[\s-]\d+),\s+?(?'vel_y'[\s-]\d+)\>)";


    public static int4[] ParseInput(string input) {
        if (input == null) return new int4[0];
        RegexOptions options = RegexOptions.Multiline;
        var matches = Regex.Matches(input, pattern, options);
        int4[] result = new int4[matches.Count];
        for (int i = 0; i < matches.Count; i++) {
            Match m = matches[i];
            result[i] = new int4(
                int.Parse(m.Groups["pos_x"].Value),
                int.Parse(m.Groups["pos_y"].Value),
                int.Parse(m.Groups["vel_x"].Value),
                int.Parse(m.Groups["vel_y"].Value));
        }
        return result;
    }
}
