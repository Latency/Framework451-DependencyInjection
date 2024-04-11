// ****************************************************************************
// Project:  DependencyInjection.ConsoleApp.Example
// File:     Dao.cs
// Author:   Latency McLaughlin
// Date:     04/11/2024
// ****************************************************************************

namespace DependencyInjection.ConsoleApp.Example;

public class Dao : IDao
{
    private const string Content = "This is DI sample";
    public        string GetWriter() => Content;
}