using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CipherServices.Services;
using Microsoft.EntityFrameworkCore;
using CipherServices.Data;

namespace CipherServices
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddRazorPages().AddRazorPagesOptions(options =>
      {
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
      });
services.AddDbContext<MessageContext>(options =>
  options.UseSqlite(Configuration.GetConnectionString("MessageContext")));

services.AddTransient<IDecrypter, Decrypter>();
services.AddTransient<IEncrypter, Encrypter>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
      });
    }
  }
}
