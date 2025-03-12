using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StringStericQuestion : MonoBehaviour
{
    // Start is called before the first frame update
    public string a = "a*d*F*H";
    public string b = "bcegh";
    string fraction = "8/12, 15/25, 100/10, 9/3, 0/5";
    void Start()
    {

    }
    [ContextMenu("Answers")]
    public void Answers()
    {
        string temp = ReturnMergedStrings(a, b);
        Debug.Log(temp);

        char mostrepeating = MostRepeatingCharacter("aaaaaaccddggrreessssfgg", true);
        char mostnonrepeating = MostRepeatingCharacter("aaaaaaccddggrreessssfgg");
        int gcd = GCD(8, 12);
        Debug.Log(mostrepeating + $"{gcd}  " + mostnonrepeating);

        string fracLinq = SimplifyFractionsbyLinq(fraction);
        string fracForEach = SimplifyFractionsbyFoeEach(fraction);
        Debug.Log($"{fracForEach},    {fracLinq}");
    }

    private string ReturnMergedStrings(string pattern, string placement)
    {
        char[] charArr = pattern.ToCharArray();
        int charArrindex = 0;

        for (int i = 0; i < charArr.Length; i++)
        {
            if (charArr[i] == '*' && charArrindex < placement.Length)
            {
                charArr[i] = placement[charArrindex++];
                charArrindex++;
            }
        }
        return new string(charArr);
    }


    private char MostRepeatingCharacter(string pattern, bool Isrepeating = false)
    {
        Dictionary<char, int> stringdic = new Dictionary<char, int>();

        for (int i = 0; i < pattern.Length; i++)
        {
            if (stringdic.ContainsKey(pattern[i]))
            {
                stringdic[pattern[i]]++;
            }
            else
            {
                stringdic[pattern[i]] = 1;
            }
        }
        char repeatedelement = '_';
        int mostrepeatednumber = 0;
        if (Isrepeating)
        {


            foreach (var item in pattern)
            {
                if (stringdic[item] > mostrepeatednumber)
                {
                    repeatedelement = item;
                    mostrepeatednumber = stringdic[item];
                }
            }


        }
        else
        {
            foreach (var item in pattern)
            {
                if (stringdic[item] == 1)
                {
                    repeatedelement = item;

                }
            }

        }
        return repeatedelement;
    }

    int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return Math.Abs(a);
    }


    public string SimplifyFractionsbyLinq(string input)
    {
        string[] fractions = input.Split(',');

        var result = fractions.Select(fraction =>
        {
            string[] parts = fraction.Trim().Split("/");
            int numerator = int.Parse(parts[0]);
            int Denumerator = int.Parse(parts[1]);
            if (numerator == 0) return "0/1";
            int gcd = GCD(numerator, Denumerator);
            return $"{numerator / gcd}/{Denumerator / gcd}";
        });

        return string.Join(",", result);
    }
    public string SimplifyFractionsbyFoeEach(string input)
    {
        string[] fractions = input.Split(',');
        List<string> result = new List<string>();

        foreach (var fraction in fractions)
        {
            string[] part = fraction.Trim().Split("/");
            int numerator = int.Parse(part[0]);
            int Denumerator = int.Parse(part[1]);
            string temp;
            if (numerator == 0) temp = "0";
            else
            {
                int Gcd = GCD(numerator, Denumerator);
                if (Denumerator / Gcd == 1) temp = $"{numerator / Gcd}";
                else
                    temp = $"{numerator / Gcd}/{Denumerator / Gcd}";
            }


            result.Add(temp);
        }
        return string.Join(",", result);
    }

    [ContextMenu("ReverseWordString")]
    public void ReverseWordString()
    {
        string input = "Hello there   world";
        char[] chars = new char[input.Length];
        int index = 0;
        for (int i = input.Length - 1; i >= 0; i--)
        {
            chars[index] = input[i];
            if (index < input.Length)
            {
                index++;
            }

        }
        int start = 0;
        for (int i = 0; i <= chars.Length; i++)
        {
            if (i == chars.Length || chars[i] == ' ')
            {
                Reverse(chars, start, i - 1);
                start = i + 1;
            }
        }
        char[] cn = new char[input.Length];
        int index2 = 0;
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == ' ')
            {
                continue;
            }
            else
            {
                cn[index2] = chars[i];
                index2++;
            }
        }
        Debug.Log(new string(cn));
    }
    void Reverse(char[] chars, int start, int end)
    {
        while (start < end)
        {
            char c = chars[start];
            chars[start] = chars[end];
            chars[end] = c;
            start++;
            end--;
        }

    }
}
