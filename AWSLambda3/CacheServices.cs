using StackExchange.Redis.Extensions.Core.Abstractions;

namespace AWSLambda3
{
    public class CacheServices : ICacheService
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public CacheServices(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public async Task<Student> DoSomethingAsync()
        {
            var studentCached = await _redisCacheClient.Db0.GetAsync<Student>("student");

            if (studentCached is not null) return studentCached;

            var createdStudent = new Student()
            {
                Id = Guid.NewGuid(),
                ContactDetails = new ContactDetails()
                {
                    Email = "example@test.com",
                    Phone = "02 0000-2222"
                },
                Name = "Dummy Name"
            };

            var cached = await _redisCacheClient.Db0.AddAsync("student", createdStudent, TimeSpan.FromMilliseconds(1200));

            if (cached) Console.WriteLine("Dado Salvo no Cache");

            return createdStudent;
        }
    }
}
