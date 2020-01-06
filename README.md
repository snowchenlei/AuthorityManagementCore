# 后台权限管理系统
开发环境：VS2017 + .NET Core2.1 + EF Core(Code First) + SQL Server(LocalDb)

| 技术       | 地址                                              |
| :--------- | :------------------------------------------------ |
| .NET Core  | https://docs.microsoft.com/zh-cn/aspnet           |
| EF Core    | https://docs.microsoft.com/zh-cn/ef/#pivot=efcore |
| AdminLTE   | https://adminlte.io/                              |
| JQuery     | http://jquery.com/                                |
| Bootstrap4 | https://getbootstrap.com/                         |

#### 项目结构如下图所示
![image](/docs/zh-cn/images/project-structure.png)
#### 运行说明
```git
git clone -b v2 https://github.com/snowchenlei/AuthorityManagementCore.git
```
1. 设置`Snow.AuthorityManagement.Web.Mvc`为启动项
2. 修改`appsettings.json`文件`Connection String`数据库连接字符串
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=AuthCore;Trusted_Connection=True"
  }
}
```
3. 打开`程序包管理控制台`：`工具=>NuGet包管理器=>程序包管理控制台`
4. 设置`Snow.AuthorityManagement.EntityFrameworkCore`为默认项目，执行`update-database`命令初始化数据库【[详情](https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)】
5. CTRL+F5运行项目输入用户名：`admin`；密码：`123456`进入系统。
#### 必要说明
关于项目授权，不了解者可移步[官方文档](https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/policies?view=aspnetcore-3.1)
