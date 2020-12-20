using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HonestyBar.Tests.Unit
{
    public class ControllerDesignTests
    {
        [Fact]
        public void Controller_actions_should_return_tasks()
        {
            var controllers = GetControllerTypes();

            foreach (var controller in controllers)
            {
                var actions = GetApiActionMethods(controller);

                foreach (var methodInfo in actions)
                {
                    Assert.True(ReturnTypeIsGenericAsyncType(methodInfo), $"{controller.Name}.{methodInfo.Name} should return a Task<T> (e.g. Task<IActionResult>).");
                }
            }
        }

        [Fact]
        public void Async_controller_actions_should_use_cancellation_token()
        {
            var controllers = GetControllerTypes();

            foreach (var controller in controllers)
            {
                var actions = GetApiActionMethods(controller);

                foreach (var methodInfo in actions)
                {
                    var hasCancellationToken = methodInfo.GetParameters()
                        .Any(x => x.ParameterType == typeof(CancellationToken));

                    Assert.True(hasCancellationToken, $"{controller.Name}.{methodInfo.Name} should have a CancellationToken parameter.");
                }
            }
        }

        private static IEnumerable<MethodInfo> GetApiActionMethods(IReflect controller)
        {
            var actions =
                controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => m.GetCustomAttributes(typeof(HttpGetAttribute), true).Any()
                                || m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any()
                                || m.GetCustomAttributes(typeof(HttpPutAttribute), true).Any()
                                || m.GetCustomAttributes(typeof(HttpHeadAttribute), true).Any()
                                || m.GetCustomAttributes(typeof(HttpDeleteAttribute), true).Any());
            return actions;
        }

        private static IEnumerable<Type> GetControllerTypes()
        {
            var assembly = typeof(Startup).Assembly;
            var controllers = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase));
            return controllers;
        }

        private static bool ReturnTypeIsGenericAsyncType(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType.IsGenericType 
                   && (methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                       || methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>));
        }
    }
}