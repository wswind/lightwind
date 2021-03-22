# lightwind

lightweight help libraries for dotnet.

install with nuget:
```
dotnet add package Lightwind.Asyncinterceptor
dotnet add package Lightwind.DynamicProxyExtension
```

## Lightwind.Asyncinterceptor

`Lightwind.Asyncinterceptor` is a help library for async interceptors to use with Castle.Core,
which is inspired by :  
 <https://stackoverflow.com/a/39784559/7726468>  
Just create your own async interceptor class inherited from AsyncinterceptorBase to use it.

AsyncinterceptorBase running processes:  
![](./doc/img/AsyncinterceptorBase-Running-Processes.png)

sample: <https://github.com/wswind/lightwind/tree/master/samples/AutofacAsyncInterceptor>


## Lightwind.DynamicProxyExtension

`Lightwind.DynamicProxyExtension` is a help library to use Castle.Core with Asp.NET Core Default DI.

sample: <https://github.com/wswind/lightwind/tree/master/samples/MSDIWorkWithCastle>