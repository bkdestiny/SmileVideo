namespace Common.JWT
{
    public class JWTOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }//密钥
        public int ExpireSeconds { get; set; }//Token过期时间 秒
    }
}
