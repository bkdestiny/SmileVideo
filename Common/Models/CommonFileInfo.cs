using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CommonFileInfo
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string FullName { get; set; }

        public long Length { get; set; }

        public string? DirctoryName { get; set; }
        private CommonFileInfo()
        {

        }
        public CommonFileInfo(FileInfo fileInfo) {
            Name = fileInfo.Name;
            Extension=fileInfo.Extension;
            FullName = fileInfo.FullName;
            Length = fileInfo.Length;
            DirctoryName = fileInfo.DirectoryName;
        }
    }
}
