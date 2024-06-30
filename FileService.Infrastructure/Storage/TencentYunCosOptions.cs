using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.Storage
{
    public record TencentYunCosOptions(string secretId,string secretKey,string region,Buckets buckets);

    public class Buckets
    {
        public string Default { get; set; }
    }
}
