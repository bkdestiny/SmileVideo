using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Common.EFcore;
using Common.JWT;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Common.DistributeCache;
using Common.Filters;
using DotNetCore.CAP;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Logging;
using Serilog;
using Exceptionless;
using Exceptionless.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Common.Sms;
namespace Initializer
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureDbConfiguration(this WebApplicationBuilder builder)
        {

        }


        /// <summary>
        /// 配置额外服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="initOptions"></param>
        public static void ConfigureExtraServices(this WebApplicationBuilder builder, InitializerOptions initOptions)
        {
            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;
            var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
            services.RunModuleInitializers(assemblies);
            try
            {
                #region 集中式日志 Exceptionless 其它方案:ELK、自己部署日志服务器
                ExceptionlessClient.Default.Startup("hw4SGRNRrkgY1BwXJ2LERakjUC7iHsKVJZa8sOpK");
                //ExceptionlessClient.Default.Configuration.SetDefaultMinLogLevel(Exceptionless.Logging.LogLevel.Info);//默认最低级别是Warning
                #endregion

                #region 日志
                services.AddLogging(builder =>
                {
                    Log.Logger = new LoggerConfiguration().
                    MinimumLevel.Information().
                    WriteTo.File(initOptions.LogFilePath,//写入文件
                    rollingInterval:RollingInterval.Day, //日志按天保存
                    rollOnFileSizeLimit:true, //限制单个文件的最大长度
                    fileSizeLimitBytes:1*1024*1024, //单个文件最大长度 byte
                    encoding:System.Text.Encoding.UTF8, //文件字符编码
                    retainedFileCountLimit: 10 //最大保存文件数
                    ).
                    WriteTo.Exceptionless().//写入Exceptionless
                    WriteTo.Console().//写入控制台
                    CreateLogger();
                    builder.AddSerilog();
                });

                #endregion

                #region 批量配置DbContext
                string? connStr = configuration.GetConnectionString("Default");
                services.AddAllDbContexts(ctx =>
                {
                    //连接字符串如果放到appsettings.json中，会有泄密的风险
                    //如果放到UserSecrets中，每个项目都要配置，很麻烦
                    //因此这里推荐放到环境变量中。
                    ctx.UseSqlServer(connStr);
                }, assemblies);
                #endregion

                #region 认证授权
                //只要需要校验Authentication报文头的地方（非IdentityService.WebAPI项目）也需要启用这些
                //IdentityService项目e还需要启用AddIdentityCore
                services.AddAuthorization();
                services.AddAuthentication();
                JWTOptions jwtOpt = configuration.GetSection("JWT").Get<JWTOptions>();//获取JWT配置
                services.Configure<JWTOptions>(configuration.GetSection("JWT").Bind);
                services.AddJWTAuthentication(jwtOpt);//配置JWT
                services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();//自定义响应异常
                //启用Swagger中的【Authorize】按钮。这样就不用每个项目的AddSwaggerGen中单独配置了
                services.Configure<SwaggerGenOptions>(c =>
                {
                    c.AddAuthenticationHeader();
                });
                #endregion

                #region 全局拦截器注册
                builder.Services.AddMvc(opt =>
                {
                    opt.Filters.Add<IdempotentFilter>();//幂等性拦截
                    opt.Filters.Add<LoggingFiliter>();
                    opt.Filters.Add<CommonExceptionFilter>();//全局异常拦截
                    opt.Filters.Add<TransactionScopeFilter>();//默认开启事务
                });
                #endregion

                //FluentValidation.AspNetCore Begin
                //不使用内置的数据校验,使用第三方的FluentValidation
                //builder.Services.AddFluentValidationAutoValidation(); //不开启自动校验,手动校验和处理返回数据
                builder.Services.AddFluentValidationClientsideAdapters();
                builder.Services.AddValidatorsFromAssemblies(assemblies);
                //FluentValidation.AspNetCore End

                #region 解决跨域问题
                //Cors 配置前端的url
                services.AddCors(options =>
                {
                    //更好的在Program.cs中用绑定方式读取配置的方法：https://github.com/dotnet/aspnetcore/issues/21491
                    //不过比较麻烦。
                    string? corsOptions = configuration.GetSection("CorsOrigins").Value;
                    options.AddDefaultPolicy(builder => builder.WithOrigins(corsOptions.Split(",",StringSplitOptions.RemoveEmptyEntries))
                            .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                });
                #endregion

                #region 搭配nginx获取客户端真实IP地址和端口号
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.All;
                });
                #endregion


                #region 分布式缓存
                /*  services.AddStackExchangeRedisCache(options => {

    ​		        options.Configuration = "localhost";
    ​		        options.InstanceName = "xxx_";

                });*/
                //StackRedis
                string? redisConn = configuration.GetValue<string>("Redis:ConnectionString");
                string? instanceName = configuration.GetValue<string>("Redis:InstanceName");
                services.AddSingleton<IDistributeCacheService>(new RedisService(redisConn, instanceName, 0));
                #endregion


                #region 消息中间件
                services.AddCap(x =>
                {
                    //使用SqlServer数据库,连接地址请依实际修改
                    x.UseSqlServer(connStr);
                    //如果你使用的ADO.NET，根据数据库选择进行配置：
                    //x.UseSqlServer("数据库连接字符串");
                    //x.UseMySql("数据库连接字符串");
                    //x.UsePostgreSql("数据库连接字符串");

                    //如果你使用的 MongoDB，你可以添加如下配置：
                    //x.UseMongoDB("ConnectionStrings");  //注意，仅支持MongoDB 4.0+集群
                    RabbitMQOptions rabbitMQOptions = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
                    x.UseRabbitMQ(mq =>
                    {
                        mq.HostName = rabbitMQOptions.HostName; //RabitMq服务器地址，依实际情况修改此地址
                        mq.Port = rabbitMQOptions.Port;
                        mq.UserName = rabbitMQOptions.UserName; //RabbitMq账号
                        mq.Password = rabbitMQOptions.Password; //RabbitMq密码

                    });
                });
                #endregion

                #region 限流
                //需要存储速率限制计数器和ip规则
                services.AddMemoryCache();
                #region IP限流
                services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("RateLimiting"));//常规配置
                services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));//ip规则
                #endregion

                #region 客户端ID限流
                //services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("RateLimiting"));//常规配置
                //services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));//客户端规则
                #endregion
                // 注入计数器和规则存储
                services.AddInMemoryRateLimiting();
                //services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
                //services.AddDistributedRateLimiting<RedisProcessingStrategy>();
                //services.AddRedisRateLimiting();
                services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                #endregion

                #region 短信服务 阿里云
                AliyunSmsOptions? aliyunOptions= configuration.GetSection("AliyunSms").Get<AliyunSmsOptions>();
                services.AddSingleton<ISms>(new AliyunSms(aliyunOptions!));
                #endregion
            }
            catch (NullReferenceException e1)
            {
                throw new Exception("缺少相关配置");
            }
            catch (Exception e2)
            {
                throw new Exception("系统初始化失败,请检查相关配置");
            }
        }
    }
}
