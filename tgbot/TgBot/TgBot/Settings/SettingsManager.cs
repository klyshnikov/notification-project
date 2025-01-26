using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TgBot.Settings;

internal static class SettingsManager
{
    internal static int MAX_REPEAT_COUNT_WHILE_USE_TG_API = 5;
    internal static int TIMEUOT_WHILE_REPEAT_USE_TG_API = 2000;
    internal static string TOCKEN_PATH = "../../../Settings/tocken";

    internal static string TOCKEN = GetTocken();

    private static string GetTocken()
    {
        var tocken = File.ReadAllText(TOCKEN_PATH);
        tocken = tocken.Trim([' ', '\n', '\r', '\t']);
        return tocken;
    }
}
