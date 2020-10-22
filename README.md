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
2. and pass the config name of it to DbConnectionFactoryExtensions.AddSingletonDbConnectionFactory() 
3. use di to create IDbConnectionFactory