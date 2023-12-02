using CustomerBackend.Database;
using CustomerBackend.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Runtime.CompilerServices;

namespace CustomerBackend.DbConverters
{
    public static class DBConrverters
    {
        public static Customers toDb(this CustomerCreateDTO model)
        {
            return new Customers()
            {
                Email = model.Email,   
                Name = model.Name
            };
        }

        public static void update(this Customers model, CustomerUpdateDTO update )
        {
            model.Email = update.Email ?? model.Email;
            model.Name = update.Name ?? model.Name;
        }

        public static Products toDb(this ProductsCreateDTO model)
        {
            return new Products()
            {
                Name = model.Name,
                Price = (float)model.Price,
            };
        }

        public static void update(this Products model, ProductsUpdateDTO update)
        {
            model.Name = update.Name ?? model.Name;
            model.Price = update.Price ?? model.Price;
            
        }
    }
}
