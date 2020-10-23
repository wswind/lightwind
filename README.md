# ligthtwind

a light weighted framework for dotnet

1. Lightwind.Asyncinterceptor : A AsyncinterceptorBase Class inspired by : <https://stackoverflow.com/a/39784559/7726468>
2. Lightwind.DbConnection : DbConnectionFactory classes and extensions.

to install:
```
dotnet add package Lightwind.Asyncinterceptor
dotnet add package Lightwind.DbConnection
```

to use AsyncinterceptorBase:

override the functions: BeforeProceed() , AfterProceedSync() , AfterProceedAsync()

to use DbConnectionFactory：

1. set dbconnection string in appsettings.json 
2. pass the dbconnection string name to DbConnectionFactoryExtensions.AddSingletonDbConnectionFactory() 
3. use IDbConnectionFactory with di

samples: <https://github.com/wswind/lightwind/tree/master/samples>