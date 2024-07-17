using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class FileHelper
    {
        /// <summary>
        /// 创建文件并返回流对象
        /// </summary>
        /// <param name="folderPath">文件目录</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isAppend">true:已存在文件,则先删除原文件,false:继续写入原文件</param>
        /// <returns></returns>
        public static Stream? Create(string folderPath, string fileName, bool isAppend = true)
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
        /// <summary>
        /// 删除文件夹以及文件夹下的所有文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <exception cref="Exception"></exception>
        public static void DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                string[] childFilePaths = Directory.GetFiles(folderPath);
                foreach (string childFilePath in childFilePaths)
                {
                    if (File.Exists(childFilePath))
                    {
                        File.Delete(childFilePath);
                    }
                }
                string[] childFolderPaths = Directory.GetDirectories(folderPath);
                foreach (string childFolderPath in childFolderPaths)
                {
                    if (Directory.Exists(childFolderPath))
                    {
                        DeleteFolder(childFolderPath);
                    }
                }
                Directory.Delete(folderPath);
            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="Exception"></exception>
        public static void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 查询文件夹下的所有文件
        /// </summary>
        ///<param name="filePathList"></param>
        /// <param name="folderPath"></param>
        /// <param name="extensionNames"></param>
        public static List<CommonFileInfo> QueryAllFilePathByFolderPath(string folderPath, List<string>? requiredExtensionList = null)
        {
            List<CommonFileInfo> fileList = new List<CommonFileInfo>();
            GetAllFilesByFolderPath(fileList, folderPath);
            if (requiredExtensionList != null && requiredExtensionList.Count > 0)
            {
                //filePathList = filePathList.Where(filePath => extensionNameList.Any(name=>name==filePath.Substring(filePath.LastIndexOf('.')+1))).ToList();
                fileList = fileList.Where(f => requiredExtensionList.Any(e => e == f.Extension)).ToList();
            }
            return fileList;
        }
        private static void GetAllFilesByFolderPath(List<CommonFileInfo> fileList, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }
            //文件
            string[] childFilePaths = Directory.GetFiles(folderPath);
            foreach (string childFilePath in childFilePaths)
            {
                if (File.Exists(childFilePath))
                {
                    FileInfo file = new FileInfo(childFilePath);
                    if (file.Exists)
                    {
                        fileList.Add(new CommonFileInfo(file));
                    }
                }
            }
            //文件夹
            string[] childFolderPaths = Directory.GetDirectories(folderPath);
            foreach (string childFolderPath in childFolderPaths)
            {
                if (Directory.Exists(childFolderPath))
                {
                    GetAllFilesByFolderPath(fileList, childFolderPath);
                }
            }
        }
    }
}
