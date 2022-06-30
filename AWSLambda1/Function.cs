using Amazon.Lambda.Core;
using ServiceStack.Redis;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda1;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        try
        {
            RedisEndpoint redisEndpoint = new("127.0.0.1", 6379) { Db = 0L };

            using (var redis = new RedisClient(redisEndpoint))
            {
                
                LambdaLogger.Log("redis client created");

                var user = new Person { Id = Guid.NewGuid(), Name = input };

                redis.Set("user", user, TimeSpan.FromSeconds(10));
                LambdaLogger.Log("user added");

                var allUsers = redis.Get<Person>("user");
                LambdaLogger.Log("Retrieved users");
                return JsonSerializer.Serialize(allUsers);
            }
        }
        catch (Exception e)
        {
            LambdaLogger.Log(e.StackTrace);
            return "Oops! something went wrong";
        }
    }

    internal class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}
