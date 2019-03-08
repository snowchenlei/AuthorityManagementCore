# 后台权限管理系统
开发环境：VS2017 + .NET Core2.1 + EF Core(Code First) + SQL Server(LocalDb)
#### 项目结构如下图所示
![image](/snowchenlei/AuthorityManagementCore/raw/master/docs/zh-cn/images/project-structure.png)
#### 运行说明
```git
git clone https://github.com/snowchenlei/AuthorityManagementCore.git
```
1. 设置`Snow.AuthorityManagement.Web`为启动项
2. 修改`appsettings.json`文件`DefaultConnection`数据库连接字符串
3. 打开`程序包管理控制台`：`工具=>NuGet包管理器=>程序包管理控制台`
![image](/snowchenlei/AuthorityManagementCore/raw/master/docs/zh-cn/images/package-console.png)
4. 设置`Snow.AuthorityManagement.Data`为默认项目，执行`update-database`命令初始化数据库
![image](/snowchenlei/AuthorityManagementCore/raw/master/docs/zh-cn/images/install-database.png)