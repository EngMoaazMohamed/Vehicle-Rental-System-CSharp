using System;
using System.Collections.Generic;

interface IRentable
{
    double CalcCost(int days);
    string GetInfo();
    string GetVehicleType();
}

abstract class Vehicle : IRentable
{
    private string id;
    private string brand;
    private string model;
    private int year;
    private double dailyRate;
    private bool available;

    public Vehicle(string id, string brand, string model, int year, double dailyRate)
    {
        this.id = id;
        this.brand = brand;
        this.model = model;
        this.year = year;
        this.dailyRate = dailyRate;
        this.available = true;
    }

    public string Id { get { return id; } }
    public string Brand { get { return brand; } }
    public string Model { get { return model; } }
    public int Year { get { return year; } }
    public double DailyRate { get { return dailyRate; } set { dailyRate = value; } }
    public bool Available { get { return available; } set { available = value; } }

    public string GetInfo()
    {
        return brand + " " + model + " (" + year + ")";
    }

    public abstract string GetVehicleType();
    public abstract string GetExtras();

    public virtual double CalcCost(int days)
    {
        return dailyRate * days;
    }

    public virtual void Display()
    {
        Console.WriteLine("  ID       : " + id);
        Console.WriteLine("  Type     : " + GetVehicleType());
        Console.WriteLine("  Vehicle  : " + GetInfo());
        Console.WriteLine("  Rate/day : $" + dailyRate);
        Console.WriteLine("  Extras   : " + GetExtras());
        Console.WriteLine("  Status   : " + (available ? "Available" : "Rented"));
    }
}

class Car : Vehicle
{
    private int seats;
    private string transmission;

    public Car(string id, string brand, string model, int year,
               double dailyRate, int seats, string transmission)
        : base(id, brand, model, year, dailyRate)
    {
        this.seats = seats;
        this.transmission = transmission;
    }

    public int Seats { get { return seats; } }
    public string Transmission { get { return transmission; } }

    public override string GetVehicleType() { return "Car"; }
    public override string GetExtras() { return seats + " seats, " + transmission; }

    public override double CalcCost(int days)
    {
        return base.CalcCost(days);
    }
}

class SUV : Car
{
    private bool has4WD;

    public SUV(string id, string brand, string model, int year,
               double dailyRate, int seats, bool has4WD)
        : base(id, brand, model, year, dailyRate, seats, "Automatic")
    {
        this.has4WD = has4WD;
    }

    public override string GetVehicleType() { return "SUV"; }

    public override string GetExtras()
    {
        return base.GetExtras() + (has4WD ? ", 4WD" : "");
    }

    public override double CalcCost(int days)
    {
        double baseCost = base.CalcCost(days);
        double surcharge = has4WD ? days * 15 : 0;
        return baseCost + surcharge;
    }
}

class Truck : Vehicle
{
    private int payload;

    public Truck(string id, string brand, string model, int year,
                 double dailyRate, int payload)
        : base(id, brand, model, year, dailyRate)
    {
        this.payload = payload;
    }

    public override string GetVehicleType() { return "Truck"; }
    public override string GetExtras() { return payload + "T payload"; }

    public override double CalcCost(int days)
    {
        return base.CalcCost(days) * 1.1;
    }
}

class Motorcycle : Vehicle
{
    private int cc;

    public Motorcycle(string id, string brand, string model, int year,
                      double dailyRate, int cc)
        : base(id, brand, model, year, dailyRate)
    {
        this.cc = cc;
    }

    public override string GetVehicleType() { return "Motorcycle"; }
    public override string GetExtras() { return cc + "cc"; }

    public override double CalcCost(int days)
    {
        return base.CalcCost(days) * 0.9;
    }
}

class Customer
{
    private string id;
    private string name;
    private string phone;
    private string license;
    private int rentalCount;

    public Customer(string id, string name, string phone, string license)
    {
        this.id = id;
        this.name = name;
        this.phone = phone;
        this.license = license;
        this.rentalCount = 0;
    }

    public string Id { get { return id; } }
    public string Name { get { return name; } }
    public string Phone { get { return phone; } }
    public string License { get { return license; } }
    public int RentalCount { get { return rentalCount; } }

    public void AddRental()
    {
        rentalCount++;
    }

    public double GetDiscount()
    {
        if (rentalCount >= 5) return 0.10;
        if (rentalCount >= 3) return 0.05;
        return 0.0;
    }

    public void Display()
    {
        Console.WriteLine("  ID      : " + id);
        Console.WriteLine("  Name    : " + name);
        Console.WriteLine("  Phone   : " + phone);
        Console.WriteLine("  License : " + license);
        Console.WriteLine("  Rentals : " + rentalCount);
        Console.WriteLine("  Discount: " + (GetDiscount() * 100) + "%");
    }
}

class Rental
{
    private static int counter = 1;

    private string id;
    private Vehicle vehicle;
    private Customer customer;
    private string startDate;
    private int days;
    private string status;
    private double totalCost;

    public Rental(Vehicle vehicle, Customer customer, string startDate, int days)
    {
        this.id = "R" + counter.ToString().PadLeft(3, '0');
        counter++;

        this.vehicle = vehicle;
        this.customer = customer;
        this.startDate = startDate;
        this.days = days;
        this.status = "Active";

        double baseCost = vehicle.CalcCost(days);
        double discount = customer.GetDiscount();
        this.totalCost = baseCost * (1 - discount);
    }

    public string Id { get { return id; } }
    public Vehicle Vehicle { get { return vehicle; } }
    public Customer Customer { get { return customer; } }
    public string StartDate { get { return startDate; } }
    public int Days { get { return days; } }
    public string Status { get { return status; } set { status = value; } }
    public double TotalCost { get { return totalCost; } }

    public string GetEndDate()
    {
        DateTime start = DateTime.Parse(startDate);
        return start.AddDays(days).ToString("yyyy-MM-dd");
    }

    public void PrintReceipt()
    {
        Console.WriteLine("\n========== RENTAL RECEIPT ==========");
        Console.WriteLine("  Rental ID  : " + id);
        Console.WriteLine("  Vehicle    : " + vehicle.GetInfo());
        Console.WriteLine("  Type       : " + vehicle.GetVehicleType());
        Console.WriteLine("  Customer   : " + customer.Name);
        Console.WriteLine("  Start Date : " + startDate);
        Console.WriteLine("  End Date   : " + GetEndDate());
        Console.WriteLine("  Days       : " + days);
        Console.WriteLine("  Base Cost  : $" + vehicle.CalcCost(days).ToString("F2"));

        if (customer.GetDiscount() > 0)
            Console.WriteLine("  Discount   : " + (customer.GetDiscount() * 100) + "%");

        Console.WriteLine("  TOTAL COST : $" + totalCost.ToString("F2"));
        Console.WriteLine("  Status     : " + status);
        Console.WriteLine("====================================");
    }
}

class RentalManager
{
    private List<Vehicle> fleet = new List<Vehicle>();
    private List<Customer> customers = new List<Customer>();
    private List<Rental> rentals = new List<Rental>();

    public void AddVehicle(Vehicle v)
    {
        fleet.Add(v);
    }

    public void AddCustomer(Customer c)
    {
        customers.Add(c);
    }

    public Vehicle FindVehicle(string id)
    {
        return fleet.Find(v => v.Id == id);
    }

    public Customer FindCustomer(string id)
    {
        return customers.Find(c => c.Id == id);
    }

    public void ShowFleet()
    {
        Console.WriteLine("\n============== FLEET ==============");

        foreach (Vehicle v in fleet)
        {
            v.Display();
            Console.WriteLine("  ----------------------------------");
        }
    }

    public void ShowAvailable()
    {
        Console.WriteLine("\n========= AVAILABLE VEHICLES =========");

        bool found = false;

        foreach (Vehicle v in fleet)
        {
            if (v.Available)
            {
                Console.WriteLine("  [" + v.Id + "] " + v.GetInfo() +
                                  " | " + v.GetVehicleType() +
                                  " | $" + v.DailyRate + "/day" +
                                  " | " + v.GetExtras());
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("  No vehicles available.");

        Console.WriteLine("======================================");
    }

    public void ShowCustomers()
    {
        Console.WriteLine("\n============ CUSTOMERS ============");

        foreach (Customer c in customers)
        {
            c.Display();
            Console.WriteLine("  ----------------------------------");
        }
    }

    public Rental CreateRental(string vehicleId, string customerId,
                               string startDate, int days)
    {
        Vehicle v = FindVehicle(vehicleId);
        Customer c = FindCustomer(customerId);

        if (v == null)
        {
            Console.WriteLine("  ERROR: Vehicle not found.");
            return null;
        }

        if (c == null)
        {
            Console.WriteLine("  ERROR: Customer not found.");
            return null;
        }

        if (!v.Available)
        {
            Console.WriteLine("  ERROR: Vehicle is already rented.");
            return null;
        }

        Rental r = new Rental(v, c, startDate, days);

        v.Available = false;
        c.AddRental();
        rentals.Add(r);

        Console.WriteLine("\n  Rental created successfully!");
        r.PrintReceipt();

        return r;
    }

    public void ReturnVehicle(string rentalId)
    {
        Rental r = rentals.Find(x => x.Id == rentalId);

        if (r == null)
        {
            Console.WriteLine("  ERROR: Rental not found.");
            return;
        }

        if (r.Status == "Returned")
        {
            Console.WriteLine("  ERROR: This rental is already returned.");
            return;
        }

        r.Status = "Returned";
        r.Vehicle.Available = true;

        Console.WriteLine("\n  Vehicle returned successfully!");
        Console.WriteLine("  Rental " + r.Id + " — $" + r.TotalCost.ToString("F2") + " collected.");
    }

    public void ShowRentals()
    {
        Console.WriteLine("\n============= RENTALS =============");

        if (rentals.Count == 0)
        {
            Console.WriteLine("  No rentals yet.");
            return;
        }

        foreach (Rental r in rentals)
        {
            Console.WriteLine("  [" + r.Id + "] " +
                              r.Vehicle.GetInfo() + " | " +
                              r.Customer.Name + " | " +
                              r.Days + " days | $" +
                              r.TotalCost.ToString("F2") + " | " +
                              r.Status);
        }

        Console.WriteLine("===================================");
    }

    public void ShowSummary()
    {
        int active = 0;
        int returned = 0;
        double revenue = 0;

        foreach (Rental r in rentals)
        {
            if (r.Status == "Active")
                active++;

            if (r.Status == "Returned")
            {
                returned++;
                revenue += r.TotalCost;
            }
        }

        Console.WriteLine("\n============= SUMMARY =============");
        Console.WriteLine("  Total fleet    : " + fleet.Count);
        Console.WriteLine("  Available      : " + fleet.FindAll(v => v.Available).Count);
        Console.WriteLine("  Total rentals  : " + rentals.Count);
        Console.WriteLine("  Active         : " + active);
        Console.WriteLine("  Returned       : " + returned);
        Console.WriteLine("  Revenue        : $" + revenue.ToString("F2"));
        Console.WriteLine("===================================");
    }
}

class Program
{
    static RentalManager manager = new RentalManager();

    static void Main(string[] args)
    {
        SeedData();

        bool running = true;

        while (running)
        {
            ShowMenu();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    manager.ShowFleet();
                    break;

                case "2":
                    manager.ShowAvailable();
                    break;

                case "3":
                    manager.ShowCustomers();
                    break;

                case "4":
                    RentVehicle();
                    break;

                case "5":
                    ReturnVehicle();
                    break;

                case "6":
                    manager.ShowRentals();
                    break;

                case "7":
                    manager.ShowSummary();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("  Invalid choice. Try again.");
                    break;
            }
        }

        Console.WriteLine("\n  Goodbye!");
    }

    static void ShowMenu()
    {
        Console.WriteLine("VEHICLE RENTAL SYSTEM        ");
        Console.WriteLine("1. Show full fleet              ");
        Console.WriteLine("2. Show available vehicles      ");
        Console.WriteLine("3. Show customers               ");
        Console.WriteLine("4. Rent a vehicle               ");
        Console.WriteLine("5. Return a vehicle             ");
        Console.WriteLine("6. Show all rentals             ");
        Console.WriteLine("7. Show summary & revenue       ");
        Console.WriteLine("0. Exit                         ");
        Console.Write("Enter choice: ");
    }

    static void RentVehicle()
    {
        manager.ShowAvailable();

        Console.Write("  Enter Vehicle ID: ");
        string vid = Console.ReadLine().Trim().ToUpper();

        manager.ShowCustomers();

        Console.Write("  Enter Customer ID: ");
        string cid = Console.ReadLine().Trim().ToUpper();

        Console.Write("  Enter start date (yyyy-MM-dd) [Enter = today]: ");
        string date = Console.ReadLine().Trim();

        if (date == "")
            date = DateTime.Now.ToString("yyyy-MM-dd");

        Console.Write("  Enter number of days: ");

        int days;

        if (!int.TryParse(Console.ReadLine(), out days) || days < 1)
        {
            Console.WriteLine("  Invalid number of days.");
            return;
        }

        manager.CreateRental(vid, cid, date, days);
    }

    static void ReturnVehicle()
    {
        manager.ShowRentals();

        Console.Write("  Enter Rental ID to return: ");
        string rid = Console.ReadLine().Trim().ToUpper();

        manager.ReturnVehicle(rid);
    }

    static void SeedData()
    {
        manager.AddVehicle(new Car("V001", "Toyota", "Camry", 2022, 45, 5, "Automatic"));
        manager.AddVehicle(new Car("V002", "Honda", "Civic", 2023, 38, 5, "Manual"));
        manager.AddVehicle(new Car("V003", "BMW", "3 Series", 2023, 85, 5, "Automatic"));

        manager.AddVehicle(new SUV("V004", "Ford", "Explorer", 2022, 70, 7, true));
        manager.AddVehicle(new SUV("V005", "Toyota", "Land Cruiser", 2023, 110, 8, true));

        manager.AddVehicle(new Truck("V006", "Ford", "F-150", 2022, 80, 2));
        manager.AddVehicle(new Truck("V007", "Mercedes", "Actros", 2021, 150, 20));

        manager.AddVehicle(new Motorcycle("V008", "Harley-Davidson", "Sportster", 2023, 55, 1200));
        manager.AddVehicle(new Motorcycle("V009", "Honda", "CBR500", 2022, 40, 500));

        manager.AddCustomer(new Customer("C001", "Ahmed Hassan", "01001234567", "DL-A12345"));
        manager.AddCustomer(new Customer("C002", "Sara Mohamed", "01112345678", "DL-B67890"));
        manager.AddCustomer(new Customer("C003", "Omar Ali", "01223456789", "DL-C11111"));
        manager.AddCustomer(new Customer("C004", "Nour Ibrahim", "01334567890", "DL-D22222"));

        Customer ahmed = manager.FindCustomer("C001");

        ahmed.AddRental();
        ahmed.AddRental();
        ahmed.AddRental();
    }
}
