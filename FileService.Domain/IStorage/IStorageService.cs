﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.IStorage
{
    public interface IStorageService
    {
        Task<Uri> SaveAsync(string key, string srcPath);

        //Task<Uri> SaveFolderAsync(string key,string srcPath);

        Task<Uri> SaveHlsAsync(string prefixKey, string srcPath);
    }
}
