[Github](https://github.com/aliostad/CacheCow)
[示例](https://github.com/aliostad/CacheCow/tree/master/samples)

1. 首先在`.Net Core Mvc`项目中安装`CacheCow.Server.Core.Mvc`
```
    Install-Package CacheCow.Server.Core.Mvc -Version 2.4.4
```
2. 修改`Startup.cs`：
```csharp
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpCachingMvc();
        }
    }
```
3. 在需要的控制器上加`HttpCacheFactoryAttribute`即可：
```csharp
    [HttpCacheFactory(500)]// 缓存500s
    public async Task<IActionResult> Get(int id) { }
```
某些数据是不能缓存固定时间的，需要在数据修改或删除时通知把缓存删除。如一个商品已经下架了，但缓存时间没到导致许多人继续购买了此商品会造成一些不必要的麻烦。
```csharp
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpCachingMvc();
            services.AddQueryProviderAndExtractorForViewModelMvc<GetUserForEditOutput, TimedETagQueryUserRepository, UserETagExtractor>(false);
        }
    }
```
```csharp
    public class TimedETagQueryCarRepository : ITimedETagQueryProvider<IEnumerable<Car>>, ITimedETagQueryProvider<Car>
    {
        private readonly ICarRepository _repository;

        public TimedETagQueryCarRepository(ICarRepository repository)
        {
            _repository = repository;
        }

        public void Dispose()
        {
            // nothing
        }

        public Task<TimedEntityTagHeaderValue> QueryAsync(HttpContext context)
        {
            int? id = null;
            var routeData = context.GetRouteData();
            if (routeData.Values.ContainsKey("id"))
                id = Convert.ToInt32(routeData.Values["id"]);

            if (id.HasValue) // Get one car
            {
                var car = _repository.GetCar(id.Value);
                if (car != null)
                    return Task.FromResult(new TimedEntityTagHeaderValue(car.LastModified.ToETagString()));
                else
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
            }
            else // all cars
            {
                return Task.FromResult(new TimedEntityTagHeaderValue(_repository.GetMaxLastModified().ToETagString(_repository.GetCount())));
            }
        }
    }
```
