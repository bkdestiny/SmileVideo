﻿namespace Initializer
{
    public class InitializerOptions
    {
        public string ApplicationName { get; set; } = "Default";
        public string LogFilePath { get; set; }

        //用于EventBus的QueueName，因此要维持“同一个项目值保持一致，不同项目不能冲突”
        public string EventBusQueueName { get; set; }

        
    }
}
