using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Core.Constans
{
    public class DbConnectionString
    {
        #if DEBUG
        public const string connectionString = "C:\\Users\\Konrad\\Documents\\database\\app.db";
        #else
        public static readonly string connectionString = $"{Directory.GetCurrentDirectory()}\\app.db";
        #endif
    }
}