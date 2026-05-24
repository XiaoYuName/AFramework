using System.Text;

public class StringUtil {
    private static readonly StringBuilder _sb = new StringBuilder();

    public static string Concat(string param1, string param2)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }

    public static string Concat(string param1, string param2, long param3)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }

    public static string Concat(string param1, string param2, string param3)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }

    public static string Concat(string param1, string param2, string param3, string param4)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);
        _sb.Append(param4);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }
    public static string Concat(string param1, string param2, string param3, string param4, string param5)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);
        _sb.Append(param4);
        _sb.Append(param5);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }

    public static string Concat(string param1, string param2, string param3, string param4, string param5, string param6)
    {
        _sb.Length = 0;

        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);
        _sb.Append(param4);
        _sb.Append(param5);
        _sb.Append(param6);

        var ret = _sb.ToString();

        _sb.Length = 0;

        return ret;
    }

    /// <summary>
    /// 金币数量显示,万,亿
    /// </summary>
    /// <param name="goldNum"></param>
    /// <param name="holdNum"></param>
    /// <returns></returns>
    public static string goldShow(int goldNum,int holdNum = 2)
    {
        string result = goldNum.ToString();
        int powNum = (int)System.Math.Pow(10, holdNum);
        if (goldNum > 10000 * 10000)
        {
            result = (float)((int)((float)goldNum / (10000 * 10000) * powNum)) / powNum  + "亿";
            return result;
        }
        if (goldNum > 10000 )
        {
            result = (float)((int)((float)goldNum / (10000) * powNum)) / powNum + "万";
            return result;
        }
        return result;
    }
}
