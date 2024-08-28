using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ConsoleApp7;

public class ExcelReader
{
    private readonly ApplicationDbContext _context;

    public ExcelReader(ApplicationDbContext context)
    {
        _context = context;
    }

    public void ReadExcelFile(string filePath)
    {
        using (var spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheets = workbookPart.Workbook.Sheets.OfType<Sheet>().ToList();
            foreach (var sheet in sheets)
            {
                Console.WriteLine($"Reading data from sheet '{sheet.Name}'...");

                var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1); // Skip header row

                if (sheet.Name == "Drivers")
                {
                    foreach (var row in rows)
                    {
                        var cells = row.Elements<Cell>().ToList();
                        if (cells.Count < 5)
                        {
                            Console.WriteLine($"Skipping row with insufficient data in Drivers sheet.");
                            continue;
                        }

                        var driver = new Driver
                        {
                            DriverID = int.Parse(GetCellValue(cells[0], workbookPart)),
                            FirstName = GetCellValue(cells[1], workbookPart),
                            LastName = GetCellValue(cells[2], workbookPart),
                            PhoneNumber = GetCellValue(cells[3], workbookPart),
                            VehicleID = int.Parse(GetCellValue(cells[4], workbookPart))
                        };

                        _context.Drivers.Add(driver);

                        // Output data to console
                        Console.WriteLine($"Driver: ID={driver.DriverID}, Name={driver.FirstName} {driver.LastName}, Phone={driver.PhoneNumber}, VehicleID={driver.VehicleID}");
                    }
                }
                else if (sheet.Name == "Routes")
                {
                    foreach (var row in rows)
                    {
                        var cells = row.Elements<Cell>().ToList();
                        if (cells.Count < 3)
                        {
                            Console.WriteLine($"Skipping row with insufficient data in Routes sheet.");
                            continue;
                        }

                        var route = new Route
                        {
                            RouteID = int.Parse(GetCellValue(cells[0], workbookPart)),
                            RouteName = GetCellValue(cells[1], workbookPart),
                            VehicleID = int.Parse(GetCellValue(cells[2], workbookPart))
                        };

                        _context.Routes.Add(route);

                        // Output data to console
                        Console.WriteLine($"Route: ID={route.RouteID}, Name={route.RouteName}, VehicleID={route.VehicleID}");
                    }
                }
                else if (sheet.Name == "Vehicles")
                {
                    foreach (var row in rows)
                    {
                        var cells = row.Elements<Cell>().ToList();
                        if (cells.Count < 4)
                        {
                            Console.WriteLine($"Skipping row with insufficient data in Vehicles sheet.");
                            continue;
                        }

                        var vehicle = new Vehicle
                        {
                            VehicleID = int.Parse(GetCellValue(cells[0], workbookPart)),
                            VehicleNumber = GetCellValue(cells[1], workbookPart),
                            VehicleType = GetCellValue(cells[2], workbookPart),
                            NumberOfSeats = int.Parse(GetCellValue(cells[3], workbookPart))
                        };

                        _context.Vehicles.Add(vehicle);

                        // Output data to console
                        Console.WriteLine($"Vehicle: ID={vehicle.VehicleID}, Number={vehicle.VehicleNumber}, Type={vehicle.VehicleType}, Seats={vehicle.NumberOfSeats}");
                    }
                }
                else if (sheet.Name == "Tickets")
                {
                    foreach (var row in rows)
                    {
                        var cells = row.Elements<Cell>().ToList();
                        if (cells.Count < 6)
                        {
                            Console.WriteLine($"Skipping row with insufficient data in Tickets sheet.");
                            continue;
                        }

                        var ticketID = int.Parse(GetCellValue(cells[0], workbookPart));

                        // Parsing and converting date
                        var dateString = GetCellValue(cells[1], workbookPart);
                        DateOnly? purchaseDate = ParseDateOnly(dateString);

                        if (purchaseDate == null)
                        {
                            Console.WriteLine($"Skipping invalid date string: '{dateString}'");
                            continue;
                        }

                        var ticketType = GetCellValue(cells[2], workbookPart);
                        var price = decimal.Parse(GetCellValue(cells[3], workbookPart));
                        var passengerID = int.Parse(GetCellValue(cells[4], workbookPart));
                        var vehicleID = int.Parse(GetCellValue(cells[5], workbookPart));

                        // Check if VehicleID exists in Vehicles table
                        var existingVehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleID);

                        if (existingVehicle == null)
                        {
                            // If VehicleID doesn't exist, add the corresponding vehicle
                            Console.WriteLine($"Vehicle with ID={vehicleID} does not exist. Please add it to Vehicles table.");
                            continue; // Skip adding ticket until corresponding vehicle is added
                        }

                        var ticket = new Ticket
                        {
                            TicketID = ticketID,
                            PurchaseDate = purchaseDate.Value,
                            TicketType = ticketType,
                            Price = price,
                            PassengerID = passengerID,
                            VehicleID = vehicleID
                        };

                        _context.Tickets.Add(ticket);

                        // Output data to console
                        Console.WriteLine($"Ticket: ID={ticketID}, PurchaseDate={purchaseDate}, Type={ticketType}, Price={price}, PassengerID={passengerID}, VehicleID={vehicleID}");
                    }
                }
                else if (sheet.Name == "Passengers")
                {
                    foreach (var row in rows)
                    {
                        var cells = row.Elements<Cell>().ToList();
                        if (cells.Count < 2)
                        {
                            Console.WriteLine($"Skipping row with insufficient data in Passengers sheet.");
                            continue;
                        }

                        var passengerID = int.Parse(GetCellValue(cells[0], workbookPart));
                        var hasTicket = ParseBool(GetCellValue(cells[1], workbookPart));

                        // Check if PassengerID already exists in database
                        var existingPassenger = _context.Passengers.FirstOrDefault(p => p.PassengerID == passengerID);

                        if (existingPassenger == null)
                        {
                            var passenger = new Passenger
                            {
                                PassengerID = passengerID,
                                HasTicket = hasTicket
                            };

                            _context.Passengers.Add(passenger);

                            // Output data to console
                            Console.WriteLine($"Passenger: ID={passengerID}, HasTicket={hasTicket}");
                        }
                        else
                        {
                            // Optionally add logic for updating or skipping duplicate
                            Console.WriteLine($"Passenger with ID={passengerID} already exists. Skipping...");
                        }
                    }
                }

                // Save changes to database after processing each sheet
                _context.SaveChanges();
            }
        }
    }

    private string GetCellValue(Cell cell, WorkbookPart workbookPart)
    {
        var value = cell.CellValue?.Text;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable
                   .Elements<SharedStringItem>().ElementAt(int.Parse(value)).InnerText;
        }

        return value;
    }

    private bool ParseBool(string value)
    {
        if (value.ToLower() == "да" || value.ToLower() == "yes" || value == "1")
        {
            return true;
        }
        if (value.ToLower() == "нет" || value.ToLower() == "no" || value == "0")
        {
            return false;
        }
        throw new FormatException($"String '{value}' was not recognized as a valid Boolean.");
    }

    private DateOnly? ParseDateOnly(string dateTimeString)
    {
        Console.WriteLine($"Parsing date string: {dateTimeString}");

        if (double.TryParse(dateTimeString, out double oaDate))
        {
            try
            {
                var date = DateTime.FromOADate(oaDate).Date;
                var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
                Console.WriteLine($"Parsed date from numeric value: {dateOnly}");
                return dateOnly;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error parsing date from numeric value: {ex.Message}");
                return null;
            }
        }

        if (DateOnly.TryParseExact(dateTimeString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateOnlyResult))
        {
            Console.WriteLine($"Parsed date from string: {dateOnlyResult}");
            return dateOnlyResult;
        }

        Console.WriteLine($"Failed to parse date: {dateTimeString}");
        return null;
    }
}
