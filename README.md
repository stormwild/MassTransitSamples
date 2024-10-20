# MassTransit Example

## MassTransit

MassTransit is a free, open-source distributed application framework for .NET. MassTransit makes it easy to create applications and services that leverage message-based communication, and provides a consistent API for a variety of message transports. MassTransit is Apache 2.0 licensed.

## Pre-requisites

- .NET 6.0 SDK
- AWS Account

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio or Visual Studio Code
3. Update the `appsettings.json` file with your AWS credentials
4. Ensure the AWS credentials have the following permissions:

   ```json
    {
        "Version": "2012-10-17",
        "Statement": [
            {
                "Sid": "SqsAccess",
                "Effect": "Allow",
                "Action": [
                    "sqs:SetQueueAttributes",
                    "sqs:ReceiveMessage",
                    "sqs:CreateQueue",
                    "sqs:DeleteMessage",
                    "sqs:SendMessage",
                    "sqs:GetQueueUrl",
                    "sqs:GetQueueAttributes",
                    "sqs:ChangeMessageVisibility"
                ],
                "Resource": [
                    "arn:aws:sqs:*:xxxxxxxxx123:*"
                ]
            },
            {
                "Sid": "SnsAccess",
                "Effect": "Allow",
                "Action": [
                    "sns:GetTopicAttributes",
                    "sns:CreateTopic",
                    "sns:Publish",
                    "sns:Subscribe"
                ],
                "Resource": [
                    "arn:aws:sns:*:xxxxxxxxx123:*"
                ]
            },
            {
                "Sid": "SnsListAccess",
                "Effect": "Allow",
                "Action": "sns:ListTopics",
                "Resource": "*"
            }
        ]
    }
   ```

   Note: `sqs:CreateQueue` can be removed if the queue is created manually in the AWS Console.

   ```csharp
   // Program.cs
                    services.AddMassTransit(x =>
                    {

                        x.UsingAmazonSqs((context, cfg) =>
                        {
                            var awsOptions = context.GetRequiredService<IOptions<AwsOptions>>().Value;

                            cfg.Host(awsOptions.Region, h =>
                            {
                                h.AccessKey(awsOptions.AccessKey);
                                h.SecretKey(awsOptions.SecretKey);
                            });
                            
                            // For an existing queue, use the following configuration, remove ConfigureEndpoints
                            // cfg.ConfigureEndpoints(context);

                            // Specify the queue name directly
                            cfg.ReceiveEndpoint(awsOptions.SqsQueueName, e =>
                            {
                                // Optionally disable the default topic binding
                                // e.ConfigureConsumeTopology = false;
                                // e.PublishFaults = false;

                                e.ConfigureConsumer<HelloConsumer>(context);
                            });
                        });
                    });

   ```

5. Run the project
