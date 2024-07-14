using Amazon.S3;
using Boutique.Application.Mappings;
using Boutique.Application.Services;
using Boutique.Application.Services.AdressServices;
using Boutique.Application.Services.CartServices;
using Boutique.Application.Services.CategoryServices;
using Boutique.Application.Services.CommentServices;
using Boutique.Application.Services.FavoriteServices;
using Boutique.Application.Services.OrderServices;
using Boutique.Application.Services.PaymentService;
using Boutique.Application.Services.ProductServices;
using Boutique.Application.Services.RabbitMQServices;
using Boutique.Application.Services.RefreshTokenServices;
using Boutique.Application.Services.SearchServices;
using Boutique.Application.Validators;
using Boutique.Domain.Common;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Boutique.Infrastructure.ExternalServices;
using Boutique.Infrastructure.Repositories.AdressRepositories;
using Boutique.Infrastructure.Repositories.CartRepositories;
using Boutique.Infrastructure.Repositories.CategoryRepositories;
using Boutique.Infrastructure.Repositories.CommentRepositories;
using Boutique.Infrastructure.Repositories.FavoriteRepositories;
using Boutique.Infrastructure.Repositories.OrderRepositories;
using Boutique.Infrastructure.Repositories.PaymentRepositories;
using Boutique.Infrastructure.Repositories.ProductRepositories;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;

const string AllowedOrigin = "*";

var builder = WebApplication.CreateBuilder(args);

#region HttpContextAccessor
builder.Services.AddHttpContextAccessor();
#endregion

#region DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Identity
builder.Services.AddIdentity<User,IdentityRole<Guid>>()
                           .AddEntityFrameworkStores<ApplicationDbContext>()
                           .AddDefaultTokenProviders();
#endregion

#region CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
#endregion

#region FluentValidation
builder.Services.AddControllers().AddFluentValidation(fv => {
    fv.RegisterValidatorsFromAssemblyContaining<AddressValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<PaymentValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<PaymentTransactionValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CartDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CartItemDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CreateCartDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CreateCommentDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CreateCommentReplyDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<LoginDtoValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<OrderDTOValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<OrderItemDTOValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<PaymentRequestDTOValidator>();
});
#endregion

#region Iyzico settings
builder.Services.Configure<IyzicoSettings>(builder.Configuration.GetSection("IyzicoSettings"));
#endregion

#region Redis
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
if(string.IsNullOrEmpty(redisConnectionString)) {
    throw new Exception("RedisConnection string is not configured correctly.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
builder.Services.AddSingleton<RedisCacheService>();
#endregion

#region Scoped Services
// JWT Service
builder.Services.AddScoped<VerificationService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<IAddressService,AddressService>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();
builder.Services.AddScoped<IAddressRepository,AddressRepository>();
builder.Services.AddScoped<IFavoriteRepository,FavoriteRepository>();

builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<ICommentService,CommentService>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IFavoriteService,FavoriteService>();

#endregion

#region SmsService
builder.Services.AddSingleton<SmsService>();
#endregion

#region ElasticSearch
builder.Services.Configure<ElasticsearchSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));

builder.Services.AddSingleton(sp => {
    var settings = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
    var connectionSettings = new ElasticsearchClientSettings(new Uri(settings.Uri))
        .Authentication(new Elastic.Transport.ApiKey(settings.ApiKey))
        .DisableDirectStreaming();
    return new ElasticsearchClient(connectionSettings);
});

builder.Services.AddScoped<IElasticsearchService,ElasticsearchService>();
#endregion

#region MongoDB
builder.Services.AddScoped<MongoDbContext>();
#endregion

#region AWS S3
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
    builder.Configuration["AWS:AccessKey"],
    builder.Configuration["AWS:SecretKey"]
);
awsOptions.Region = Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]);

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddSingleton<AwsS3Service>();
#endregion

#region RabbitMQ
builder.Services.AddHostedService<RabbitMQBackgroundService>();
#endregion

#region JWT
builder.Services.AddSingleton<JwtService>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

#region Authorization
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Admin",policy => policy.RequireRole("Admin"));
});
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

#region Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1",new OpenApiInfo { Title = "Boutique API",Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                            {
                                new OpenApiSecurityScheme {
                                    Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                            }});
});
#endregion


var app = builder.Build();

#region Middleware
app.UseSwagger();
app.UseSwaggerUI(swagger => {
    swagger.SwaggerEndpoint("/swagger/v1/swagger.json","Boutique Api v1");
});

if(app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();

    app.UseCors(builder => {
        builder.WithOrigins(AllowedOrigin)
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
