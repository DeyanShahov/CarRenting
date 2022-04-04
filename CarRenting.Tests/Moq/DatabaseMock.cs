﻿using CarRenting.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarRenting.Tests.Moq
{
    public static class DatabaseMock
    {
        public static CarRentingDbContext Instance 
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<CarRentingDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new CarRentingDbContext(dbContextOptions);
            }
        }
    }
}
