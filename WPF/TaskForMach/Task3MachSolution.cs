using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.TaskForMach;

public class Task3MachSolution
{
    public static string GetSolution()
    {
        var sb = new StringBuilder();
        bool[] vars = { false, true };

        sb.AppendLine(" x | y | z | f(x, y, z)");
        sb.AppendLine("---------------------");

        List<string> minterms = new List<string>();
        List<string> maxterms = new List<string>();

        foreach (bool x in vars)
        {
            foreach (bool y in vars)
            {
                foreach (bool z in vars)
                {
                    bool fx = ((x ^ true) && (y ^ true) && !z) || (y && z);
                    sb.AppendLine($" {Convert.ToInt32(x)} | {Convert.ToInt32(y)} | {Convert.ToInt32(z)} | {Convert.ToInt32(fx)}");
                    
                    if (fx)
                    {
                        minterms.Add($"{Convert.ToInt32(x)}{Convert.ToInt32(y)}{Convert.ToInt32(z)}");
                    }
                    else
                    {
                        maxterms.Add($"{Convert.ToInt32(x)}{Convert.ToInt32(y)}{Convert.ToInt32(z)}");
                    }
                }
            }
        }

        sb.AppendLine("\nСДНФ:");
        List<string> sdnf = new List<string>();
        foreach (var minterm in minterms)
        {
            string term = "";
            if (minterm[0] == '1') term += "x"; else term += "!x";
            if (minterm[1] == '1') term += "y"; else term += "!y";
            if (minterm[2] == '1') term += "z"; else term += "!z";
            sdnf.Add(term);
        }
        sb.AppendLine(string.Join(" | ", sdnf));

        sb.AppendLine("\nСКНФ:");
        List<string> sknf = new List<string>();
        foreach (var maxterm in maxterms)
        {
            string term = "(";
            if (maxterm[0] == '0') term += "x | "; else term += "!x | ";
            if (maxterm[1] == '0') term += "y | "; else term += "!y | ";
            if (maxterm[2] == '0') term += "z"; else term += "!z";
            term += ")";
            sknf.Add(term);
        }
        sb.AppendLine(string.Join(" & ", sknf));

        sb.AppendLine("\nМДНФ:");
        string mdnf = MinimizeMinterms(minterms);
        sb.AppendLine(mdnf);

        return sb.ToString();
    }

    private static string MinimizeMinterms(List<string> minterms)
    {
        List<string> groups = new List<string>();
        groups.AddRange(minterms);

        while (true)
        {
            List<string> newGroups = new List<string>();
            bool combined = false;

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = i + 1; j < groups.Count; j++)
                {
                    string combinedTerm = CombineTerms(groups[i], groups[j]);
                    if (combinedTerm != null)
                    {
                        newGroups.Add(combinedTerm);
                        combined = true;
                    }
                }
            }

            if (!combined)
                break;

            groups = new List<string>(newGroups);
        }

        List<string> minimizedTerms = new List<string>();
        foreach (var term in groups)
        {
            string minimizedTerm = "";
            if (term[0] != '-') minimizedTerm += term[0] == '1' ? "x" : "!x";
            if (term[1] != '-') minimizedTerm += term[1] == '1' ? "y" : "!y";
            if (term[2] != '-') minimizedTerm += term[2] == '1' ? "z" : "!z";
            minimizedTerms.Add(minimizedTerm);
        }

        return string.Join(" | ", minimizedTerms);
    }

    private static string CombineTerms(string term1, string term2)
    {
        int differences = 0;
        char[] combined = term1.ToCharArray();

        for (int i = 0; i < term1.Length; i++)
        {
            if (term1[i] != term2[i])
            {
                differences++;
                combined[i] = '-';
            }
        }

        if (differences == 1)
            return new string(combined);

        return null;
    }
}
