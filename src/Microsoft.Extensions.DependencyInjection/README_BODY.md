<hr>

## Navigation
* <a href="#introduction">Introduction</a>
* <a href="#projects">History</a>
* <a href="#usage">Usage</a>
* <a href="#installation">Installation</a>
* <a href="#license">License</a>

<hr>

<h2><a name="introduction">Introduction</a></h2>

> The code is same as .net core's Microsoft.Extensions.DependencyInjection. I just re-compiled the code to framewrok 4.5.1 and add two project to help use it.

> In the solution, it includes some examples it shows how to use the DI in console application, asp.net Mvc or webform.

<h2><a name="projects">Projects</a></h2>
+ Microsoft.Extensions.DependencyInjection
+ Microsoft.Extensions.DependencyInjection.Abstractions
+ Microsoft.Extensions.DependencyInjection.ConsoleApp
+ Microsoft.Extensions.DependencyInjection.SystemWeb

> In this solution, The both of projects `Microsoft.Extensions.DependencyInjection` and `Microsoft.Extensions.DependencyInjection.Abstractions` thest code is same as Microsoft's [DependencyInjection](https://github.com/aspnet/DependencyInjection)

> The both of projects `Microsoft.Extensions.DependencyInjection.ConsoleApp` and `Microsoft.Extensions.DependencyInjection.SystemWeb` contains some usage codes it can help us to use the library in different solution (Console Application, WinForm, WebForm or Asp.net Mvc).

<h1><a name="usage">How to use it</a></h1>

## Console Application (or WinForm)
If you want to use the DI project in your console application or Winform, First the project need to reference `Microsoft.Extensions.DependencyInjection`,`Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.DependencyInjection.ConsoleApp` projects. Second, you just need to add small codes in Program.Main, the code exmaple:
```
static void Main(string[] args)
{
    //Init the DI container
    DependencyInjectionStartup.Initialize(services =>
    {
        services.AddSingleton<IDao, Dao>();
    });

    //Get instnace by service type from DI container
    var dao = DIProviderInstance.ProviderInstance.GetRequiredService<IDao>();

    Console.WriteLine(dao.GetWriter());
    Console.ReadKey();
}
```
## Asp.net Mvc (or Asp.net WebAPI)
If you want to use the DI project in your web application (Asp.net Mvc or Asp.net WebAPI), you need to have some Front-facing step.
+ Add nuget package Microsoft.Owin.Host.SystemWeb.
+ Add reference `Microsoft.Extensions.DependencyInjection`,`Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.DependencyInjection.SystemWeb`
+ Add a Owin Startup class.
+ Extends the abstract class DependencyInjectionStartup (Included in Microsoft.Extensions.DependencyInjection.SystemWeb).
+ Add class Startup's construct method and add below code
```
base.Initialize();
```
+ Implement the abstract method ServiceConfiguration, you can add object to DI container in the method.
+ Add middleware in method Configuration.
Example code:
```
public class Startup : DependencyInjectionStartup
{
    public Startup()
    {
        base.Initialize();
    }

    public void Configuration(IAppBuilder app)
    {
        app.Use<DependencyInjectionMiddleware>(base.services);
    }

    protected override void ServiceConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IDao, Dao>();
    }
}

//Get object instance by service Type from DI container.
var dao = ServiceResolve.ResolveOwinFactory(HttpContext.GetOwinContext()).GetRequiredService<IDao>();
```
## WebForm
If you want to use the DI project in your web application (Asp.net WebForm), you need to have some Front-facing step.
+ Add nuget package Microsoft.Owin.Host.SystemWeb.
+ Add reference `Microsoft.Extensions.DependencyInjection`,`Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.DependencyInjection.SystemWeb`
+ Add a Owin Startup class.
+ Extends the abstract class DependencyInjectionStartup (Included in Microsoft.Extensions.DependencyInjection.SystemWeb).
+ Add class Startup's construct method and add below code
```
base.Initialize();
```
+ Implement the abstract method ServiceConfiguration, you can add object to DI container in the method.
+ Add middleware in method Configuration.
Example code:
```
public class Startup : DependencyInjectionStartup
{
    public Startup()
    {
        base.Initialize();
    }

    public void Configuration(IAppBuilder app)
    {
        app.Use<DependencyInjectionMiddleware>(base.services);
    }

    protected override void ServiceConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IDao, Dao>();
    }
}

//Get object instance by service Type from DI container.
var dao = ServiceResolve.ResolveHttpFactory(HttpContext.Current).GetRequiredService<IDao>();
```

<h2><a name="installation">Installation</a></h2>

This library can be installed using NuGet found [here](https://www.nuget.org/packages/Framework451-DependencyInjection/).

<h2><a name="license">License</a></h2>

The source code for the site is licensed under the MIT license, which you can find in
the [MIT-LICENSE].txt file.

All graphical assets are licensed under the
[Creative Commons Attribution 3.0 Unported License](https://creativecommons.org/licenses/by/3.0/).

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job.)

   [GNU LESSER GENERAL PUBLIC LICENSE]: <http://www.gnu.org/licenses/lgpl-3.0.en.html>
   [MSDN article]: <https://msdn.microsoft.com/en-us/library/c5b8a8f9(v=vs.100).aspx>
   [MIT-License]: <http://choosealicense.com/licenses/mit/>