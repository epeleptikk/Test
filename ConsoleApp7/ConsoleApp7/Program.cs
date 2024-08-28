using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7;

class Program
{
    static void Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Perevozka;Username=postgres;Password=Packardbell345");

        using (var context = new ApplicationDbContext(optionsBuilder.Options))
        {
            var excelReader = new ExcelReader(context);
            excelReader.ReadExcelFile("Задание 3 Сорокин.xlsx");
        }
    }

}




