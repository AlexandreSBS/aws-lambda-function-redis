using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Diagnostics;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda3;

public class Function
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;
    private readonly Stopwatch stopwatch;

    public Function()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        _services = new ServiceCollection();
    }
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<Student> FunctionHandler()
    {
        try
        {
            ConfigureServices(_services);

            var result = await _services.BuildServiceProvider().GetService<ICacheService>().DoSomethingAsync();

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        finally
        {
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        }

    }

    private void ConfigureServices(IServiceCollection services)
    {
        var redisConfiguration = _configuration.GetSection("Redis").Get<RedisConfiguration>();
        services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

        services.AddSingleton<ICacheService, CacheServices>();
    }

}
