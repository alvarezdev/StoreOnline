using System.Text.Json;
using StoreOnline.Domain.Dto;
using StoreOnline.Domain.Entities;
using StoreOnline.Infrastructure.Ports;
using StoreOnline.Domain.Ports;
using Microsoft.Extensions.DependencyInjection;
using StoreOnline.Application.Products;
using StoreOnline.Domain.Enums;
using StoreOnline.Api.Tests.Builders;
using StoreOnline.Api.Tests.Anticorruption;

namespace StoreOnline.Api.Tests;

public class BuyApiTest
{
    [Fact]
    public async Task PostBuyClientsSuccess()
    {            
        await using var webApp = new ApiApp();
        var serviceCollection = webApp.GetServiceCollection();

        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
        Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);; 

        var carRequest = await repository.AddAsync(car);
        await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

        BuyRegisterCommand buyCommand = new(carRequest.Id, carDtoTest.Price, PaymentMethod.CreditCar); 
        var client = webApp.CreateClient();
        var request = await client.PostAsJsonAsync("/api/buy/", buyCommand);
        request.EnsureSuccessStatusCode(); 
        var deserializeOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };  
        var responseData =  JsonSerializer.Deserialize<BuyDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);

        var url = request.Headers.Location!.OriginalString;
        Assert.Equal("/api/buy/", url);
        Assert.True(responseData is not null);
        Assert.IsType<BuyDto>(responseData);                              
    }

    [Fact]
    public async Task PostClientsFailureByProductId()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            var serviceCollection = webApp.GetServiceCollection();

            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);

            var carRequest = await repository.AddAsync(car);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

            BuyRegisterCommand buyCommand = new(Guid.Empty, carRequest.Price, PaymentMethod.CreditCar); 

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/buy/", buyCommand);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The car id must not be empty"));
        }
    }

    [Fact]
    public async Task PostClientsFailureByPrice()
    {
        HttpResponseMessage request = default!;
        try
        {
            await using var webApp = new ApiApp(); 

            var serviceCollection = webApp.GetServiceCollection();

            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            CarDtoTest carDtoTest = new CarDtoTest.Builder().Build();
            Car car = CarTestFactory.FromCarDtoTestToModel(carDtoTest);

            var carRequest = await repository.AddAsync(car);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

            BuyRegisterCommand buyCommand = new(carRequest.Id, 0, PaymentMethod.CreditCar); 

            var client = webApp.CreateClient();

            request = await client.PostAsJsonAsync("/api/buy/", buyCommand);             
            request.EnsureSuccessStatusCode();                 
            Assert.Fail("Should not get here");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The price must be greater than 0"));
        }
    }
}

