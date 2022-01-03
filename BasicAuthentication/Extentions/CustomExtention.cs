using BasicAuthentication.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BasicAuthentication.Extentions
{
    public static class CustomExtention
    {
        public static IServiceCollection AddCustomSwaggerDocWithBasicAuthentication(this IServiceCollection services, OpenApiInfo openApiInfo)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc(openApiInfo.Version, openApiInfo);
                swagger.AddSecurityDefinition("Basic", new OpenApiSecurityScheme()
                    { 
                        Name = "Basic Authentication",
                        Description = "Basic authentication with user and password.",
                        In = ParameterLocation.Header,
                        Scheme = "Basic",
                        Type = SecuritySchemeType.Http
                    });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        { 
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Id = "Basic",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new string[] { }
                        }
                    });
            });
            return services;
        }

        public static IServiceCollection AddCustomBasicAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication",null);
                

            return services;
        }
    }
}
