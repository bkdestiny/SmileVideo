using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class FileHelper
    {
        public static Stream? Create(string folderPath, string fileName, bool isAppend = false)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                Stream? stream = null;
                string fileFullPath = folderPath + "/" + fileName;
                if (File.Exists(fileFullPath))
                {
                    if (!isAppend)
                    {
                        File.Delete(fileFullPath);
                        stream = File.Create(fileFullPath);
                    }
                    else
                    {
                        stream = File.OpenRead(fileFullPath);
                    }
                }
                else
                {
                    stream = File.Create(fileFullPath);
                }
                return stream;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public static void DeleteFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    string[] childFilePaths=Directory.GetFiles(folderPath);
                    foreach(string childPath in childFilePaths){
                        if (Directory.Exists(childPath)) {
                            DeleteFolder(childPath);
                        }
                        else
                        {
                            File.Delete(childPath);
                        }
                    }
                    Directory.Delete(folderPath);
                }
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }   
        }
        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e) { 
                throw new Exception(e.Message);
            }
            
        }
    }
}
