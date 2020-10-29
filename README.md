# ligthtwind

light weighted help libraries for dotnet

1. Lightwind.Asyncinterceptor : A AsyncinterceptorBase Class inspired by : <https://stackoverflow.com/a/39784559/7726468>
2. Lightwind.DbConnection : DbConnectionFactory classes and extensions.

install with nuget:
```
dotnet add package Lightwind.Asyncinterceptor
dotnet add package Lightwind.DbConnection
```

samples: <https://github.com/wswind/lightwind/tree/master/samples>

AsyncinterceptorBase running processes:

![](https://img2020.cnblogs.com/blog/1114902/202010/1114902-20201023111551158-771913593.png)

processes of using DbConnectionFactory：

1. set dbconnection string in appsettings.json 
2. pass the dbconnection string name to DbConnectionFactoryExtensions.AddSingletonDbConnectionFactory() 
3. use IDbConnectionFactory with di