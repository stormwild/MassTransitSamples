using System;

namespace WorkerSample.Modules
{
    public class AwsOptions
    {
        /// <summary>
        /// us-east-1
        /// </summary>
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }

        /// <summary>
        /// hello
        /// </summary>
        public string SqsQueueName { get; set; }

        /// <summary>
        /// WorkerSample_Contracts-Hello
        /// </summary>
        public string SnsTopicName { get; set; }
    }
}