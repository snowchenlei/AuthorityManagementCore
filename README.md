# 后台权限管理系统
开发环境：VS2017 + .NET Core2.1 + EF Core(Code First) + SQL Server(LocalDb)
| 技术            | 地址                                                                                                                            |
| :-------------- | :------------------------------------------------------------------------------------------------------------------------------ |
| .NET Core       | https://docs.microsoft.com/zh-cn/aspnet/?view=aspnetcore-2.1#pivot=core                                                         |
| EF Core         | https://docs.microsoft.com/zh-cn/ef/#pivot=efcore                                                                               |
| SQL Server      | https://docs.microsoft.com/zh-cn/sql/sql-server/sql-server-technical-documentation?toc=..%2ftoc%2ftoc.json&view=sql-server-2017 |
| AdminLTE        | https://adminlte.io/                                                                                                            |
| JQuery          | http://jquery.com/                                                                                                              |
| Bootstrap3      | https://v3.bootcss.com/                                                                                                         |
| Bootstrap-Table | https://bootstrap-table.com/                                                                                                    |

#### 项目结构如下图所示
![image](/docs/zh-cn/images/project-structure.png)
#### 运行说明
```git
git clone https://github.com/snowchenlei/AuthorityManagementCore.git
```
1. 设置`Snow.AuthorityManagement.Web`为启动项
2. 修改`appsettings.json`文件`DefaultConnection`数据库连接字符串
3. 打开`程序包管理控制台`：`工具=>NuGet包管理器=>程序包管理控制台`
![image](/docs/zh-cn/images/package-console.png)
1. 设置`Snow.AuthorityManagement.Data`为默认项目，执行`update-database`命令初始化数据库
![image](/docs/zh-cn/images/install-database.png)