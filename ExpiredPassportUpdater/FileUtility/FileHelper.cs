using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.FileUtility
{
    class FileHelper
    {
        public static void BZipUnarchiveFromNewFile(string pathToCompressedFile, string pathToCreateDecompressedFile)
        {
            try
            {

                using (FileStream fileToDecompressAsStream = new FileStream(pathToCompressedFile, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream decompressedStream = File.Create(pathToCreateDecompressedFile))
                    {
                            BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception("ArchiveHelper: Unarchive file in folder error", ex);
            }
        }

        public static void DeleteFileIfExist(string pathToDeletedFile)
        {
            if (File.Exists(pathToDeletedFile))
                File.Delete(pathToDeletedFile);
        }

        public static int GetCountLinesFromCVSFile(string pathToCVSFile)
        {
            return File.ReadLines(pathToCVSFile).Count();
        }

    }
}
