using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.FileUtility
{
    class FileReader
    {
        public static string[] ReadCVSFileInParts(string pathToCVSFile, int skip, int countLines)
        {
            return File.ReadLines(pathToCVSFile).Skip(skip).Take(countLines).ToArray();
        }
    }
}
