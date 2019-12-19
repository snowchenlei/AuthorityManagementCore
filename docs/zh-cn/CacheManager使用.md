[官网](http://cachemanager.michaco.net/)
[Github](https://github.com/MichaCo/CacheManager)
[示例](https://github.com/MichaCo/CacheManager/tree/dev/samples)

1. 首先在`.Net Core Mvc`项目中安装`CacheManager.Microsoft.Extensions.Configuration`
```
    Install-Package CacheManager.Microsoft.Extensions.Configuration -Version 1.2.0
```
2. 创建`cache.json`文件示例内容如下，具体配置参考[官方文档](http://cachemanager.michaco.net/documentation/CacheManagerConfiguration#microsoft.extensions.configuration)：
```json
{
  "$schema": "http://cachemanager.michaco.net/schemas/cachemanager.json#",
  "cacheManagers": [
    {
      "name": "simpleMemoryCache",
      "handles": [
        {
          "knownType": "Dictionary"
        }
      ]
    }
  ]
}
```
3. 修改`Program.cs`文件
```csharp
    WebHost.CreateDefaultBuilder(args)
        // 附加配置文件(避免单一配置文件过大)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("上一步所创建的文件名称.扩展名", true);
        })
```
4. 修改`Startup.cs`：
```csharp
    public class Startup
    {
        public readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region CacheManager缓存

            services.AddLogging(c => c.AddConsole().AddDebug().AddConfiguration(Configuration));
            // using the new overload which adds a singleton of the configuration to services and the configure method to add logging
            //services.AddCacheManagerConfiguration(Configuration, cfg => cfg.WithMicrosoftLogging(services));
            services.AddCacheManagerConfiguration(Configuration);
            // any other type will be this. Configurastion used will be the one defined by AddCacheManagerConfiguration earlier.
            services.AddCacheManager();

            #endregion CacheManager缓存
        }
    }
```
5. 在需要的项目中引入`CacheManager.Core`包并注入即可：
```
    Install-Package CacheManager.Core -Version 1.2.0
```
```csharp
    public class User
    {
        //构造注入具体的CacheManager
        public User(ICacheManager<DateTime> cache)
        { }
    }
```

**注意：注入的泛型类相同时才会为一个实例，否则会出现取不到值的情况。如：**
```csharp
    public class A
    {
        public A(ICacheManager<DateTime> cache)
        { 
            cache.Add('key', DateTime.Now);
        }
    }
    public class B
    {
        public B(ICacheManager<DateTime?> cache1, ICacheManager<DateTime> cache2)
        {
            // 这里是取不到A类cache存入的值的。
            cache1.Get('key');
            // 这里可以取到A类cache存入的值。
            cache2.Get('key');
        }
    }
```