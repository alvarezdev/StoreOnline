using System.Text.Json;
using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Entities;
using StoreOnline.Infrastructure.Ports;
using StoreOnline.Domain.Ports;
using Microsoft.Extensions.DependencyInjection;
using StoreOnline.Application.Products;
using StoreOnline.Api.Tests.Builders;
using StoreOnline.Api.Tests.Anticorruption;

namespace StoreOnline.Api.Tests;

public class CarApiTest
{
    [Fact]
    public async Task GetListClientsSuccess()
    {            
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();

        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var cars = CarTestFactory.Cars();
        foreach (var item in cars)
        {
            await repository.AddAsync(item);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
        }

        var client = webApp.CreateClient();  

        var list = await client.GetFromJsonAsync<List<CarDto>>("/api/list/");
        
        Assert.True(list is not null && list is IEnumerable<CarDto>);                         
    }

    [Fact]
    public async Task GetListIsSoldClientsSuccess()
    {            
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();

        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var cars = CarTestFactory.Cars();
        foreach (var item in cars)
        {
            await repository.AddAsync(item);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
        }

        var client = webApp.CreateClient();  

        var list = await client.GetFromJsonAsync<List<CarDto>>("/api/listSold/");

        Assert.True(list is not null && list is IEnumerable<CarDto>);                         
    }

    [Fact]
    public async Task PostCarClientsSuccess()
    {            
        await using var webApp = new ApiApp();

        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

        var client = webApp.CreateClient();
        var request = await client.PostAsJsonAsync("/api/product/", product);
        request.EnsureSuccessStatusCode();   
        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };  
        var responseData =  JsonSerializer.Deserialize<CarDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

        var url = request.Headers.Location!.OriginalString;
        Assert.Equal("/api/product/", url);
        Assert.True(responseData is not null);
        Assert.IsType<CarDto>(responseData);          
    }

    [Fact]
    public async Task PostClientsFailureByPrice()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            CarDtoTest carDtoTest = new CarDtoTest.Builder().SetPrice(0).Build();
            CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/product/", product);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The price must be greater than 0"));
        }
    }

    [Fact]
    public async Task PostClientsFailureByIsSold()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            CarDtoTest carDtoTest = new CarDtoTest.Builder().SetIsSold(true).Build();
            CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/product/", product);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The car must be registered as unsold"));
        }
    }

    [Fact]
    public async Task PostClientsFailureByBrand()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            CarDtoTest carDtoTest = new CarDtoTest.Builder().SetBrand("").Build();
            CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/product/", product);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The car brand must not be empty or null"));
        }
    }

    [Fact]
    public async Task PostClientsFailureByVersion()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            CarDtoTest carDtoTest = new CarDtoTest.Builder().SetVersion("").Build();
            CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/product/", product);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The car version must not be empty or null"));
        }
    }

    [Fact]
    public async Task PostClientsFailureByModel()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp();

            CarDtoTest carDtoTest = new CarDtoTest.Builder().SetModel(2017).Build();
            CarRegisterCommand product = CarTestFactory.FromCarDtoTestToCommand(carDtoTest);

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/product/", product);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The model must be older than 2018"));
        }
    }
}

