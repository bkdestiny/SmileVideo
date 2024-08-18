using AsmResolver.PE.DotNet.ReadyToRun;
using Azure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Common.Utils
{
    public class FfmpegHelper
    {
        /// <summary>
        /// 转换mp4文件为hls
        /// </summary>
        /// <param name="mp4Path">mp4文件绝对路径</param>
        /// <param name="outputFolder">输出hls文件夹路径</param>
        /// <returns>m3u8文件路径</returns>
        /// <exception cref="Exception"></exception>
        public static string? ConvertMp4ToHls(string mp4Path, string outputFolder)
        {
            string tsFilePath = "";
            try
            {
                if (mp4Path.LastIndexOf(".mp4") == -1)
                {
                    return null;
                }
                if (!Directory.Exists(outputFolder))
                {
                    //输出hls文件夹不存在，创建文件夹
                    Directory.CreateDirectory(outputFolder);
                }
                /**
                * 先将MP4文件转换为一个Ts文件
                * ffmpeg -y -i abc.mp4 -vcodec copy -acodec copy -vbsf h264_mp4toannexb abc.ts
                * */
                tsFilePath = outputFolder + "/playTs.ts";
                string convertMp4ToTsCommand = $"-y -i {mp4Path} -vcodec copy -acodec copy -vbsf h264_mp4toannexb {tsFilePath}";
                // 设置启动信息
                ProcessStartInfo convertMp4ToTsStartInfo = new ProcessStartInfo("ffmpeg")
                {
                    Arguments = convertMp4ToTsCommand,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                Process process;
                int exitCode = 0;
                string errorOutput = "";
                string standardOutput = "";
                using (process = new Process())
                {
                    // 将启动信息赋给进程对象
                    process.StartInfo = convertMp4ToTsStartInfo;
                    // 启动 FFmpeg 进程
                    process.Start();
                    errorOutput = process.StandardError.ReadToEnd();
                    standardOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
                if (exitCode != 0)
                {
                    throw new Exception("转换为ts文件失败,"+errorOutput);
                }
                Console.WriteLine(mp4Path + "转换为ts文件成功,输出信息："+standardOutput);
                /**
                * 再把Ts文件切片并生成m3u8文件，10秒一个切片
                * ffmpeg -i abc.ts -c copy -map 0 -f segment -segment_list playlist.m3u8 -segment_time 10 abc%03d.ts
                * */
                string m3u8FilePath = outputFolder + "/playlist.m3u8";
                string tsPartFilePath = outputFolder + "/playTs%03d.ts";
                string convertTsToM3u8Command = $"-i {tsFilePath} -c copy -map 0 -f segment -segment_list {m3u8FilePath} -segment_time 10 {tsPartFilePath}";
                ProcessStartInfo convertTsToM3u8StartInfo = new ProcessStartInfo("ffmpeg")
                {
                    Arguments = convertTsToM3u8Command,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                using (process = new Process())
                {
                    process.StartInfo = convertTsToM3u8StartInfo;
                    process.Start();
;                   errorOutput = process.StandardError.ReadToEnd();
                    standardOutput = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
                if (exitCode != 0)
                {
                    throw new Exception(tsFilePath + "切片失败," + errorOutput);
                }
                Console.WriteLine(tsFilePath + "切片成功,输出信息：" + standardOutput);
                return m3u8FilePath;
            }
            catch (Exception ex)
            {
                throw new Exception("mp4文件转换Hls格式失败,"+ex.Message);
            }
            finally
            {
                //不保留第一次转换的ts文件
                FileHelper.DeleteFile(tsFilePath);
            }
        }
    }
}
