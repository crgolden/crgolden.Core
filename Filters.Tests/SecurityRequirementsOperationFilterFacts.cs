namespace Clarity.Core.Filters.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class SecurityRequirementsOperationFilterFacts
    {
        private readonly Operation _operation;

        public SecurityRequirementsOperationFilterFacts()
        {
            _operation = new Operation
            {
                OperationId = $"{Guid.NewGuid()}",
                Responses = new Dictionary<string, Response>()
            };
        }

        [Fact]
        public void Apply_Method_Roles()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithRoles));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            var role = Assert.Single(attributes["Bearer"]);
            Assert.Equal("AdminRole", role);
        }

        [Fact]
        public void Apply_Method_Policies()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithPolicies));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("AdminPolicy", policy);
        }

        [Fact]
        public void Apply_Method_AllowAnonymousAttribute()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithAllowAnonymous));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            Assert.Empty(_operation.Responses);
            Assert.Null(_operation.Security);
        }

        [Fact]
        public void Apply_Controller_Roles()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithRoles), nameof(ControllerWithRoles.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("UserRole", policy);
        }

        [Fact]
        public void Apply_Controller_Policies()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithPolicies), nameof(ControllerWithRoles.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("UserPolicy", policy);
        }

        [Fact]
        public void Apply_Controller_AllowAnonymousAttribute()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithAllowAnonymous), nameof(ControllerWithAllowAnonymous.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            Assert.Empty(_operation.Responses);
            Assert.Null(_operation.Security);
        }

        private static OperationFilterContext GetContext(IReflect controllerType, string methodName)
        {
            var methodInfo = controllerType.GetMethod(
                name: methodName,
                bindingAttr: BindingFlags.NonPublic | BindingFlags.Static);
            return new OperationFilterContext(
                apiDescription: new ApiDescription(),
                schemaRegistry: default,
                methodInfo: methodInfo);
        }

        private void AssertAuthorizeResponses()
        {
            Assert.Collection(_operation.Responses,
                response1 =>
                {
                    Assert.Equal("401", response1.Key);
                    var response = Assert.IsType<Response>(response1.Value);
                    Assert.Equal("Unauthorized", response.Description);
                },
                response2 =>
                {
                    Assert.Equal("403", response2.Key);
                    var response = Assert.IsType<Response>(response2.Value);
                    Assert.Equal("Forbidden", response.Description);
                });
        }

        private static class Controller
        {
            [Authorize(Roles = "AdminRole")]
            internal static void MethodWithRoles() { }

            [Authorize(Policy = "AdminPolicy")]
            internal static void MethodWithPolicies() { }

            [AllowAnonymous]
            internal static void MethodWithAllowAnonymous() { }
        }

        [Authorize(Roles = "UserRole")]
        private static class ControllerWithRoles
        {
            internal static void Method() { }
        }

        [Authorize(Policy = "UserPolicy")]
        private static class ControllerWithPolicies
        {
            internal static void Method() { }
        }

        [AllowAnonymous]
        private static class ControllerWithAllowAnonymous
        {
            internal static void Method() { }
        }
    }
}
