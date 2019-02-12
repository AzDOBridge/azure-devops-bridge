using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AzDoBridgeUnitTests.Utilities
{
    class TestUtilities
    {
        public static T EntityFromFile<T>(string path, string filename)
        {
            var json = File.ReadAllText(Path.Combine(path, filename));
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
