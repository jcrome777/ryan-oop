using System;
using System.Collections.Generic;
using System.IO;

public interface IFileHandler
{
    void LoadFromFile();
    void SaveToFile();
}

public interface IProductOperations
{
    void AddProduct();
    void DeleteProduct();
    void UpdateProductQuantity();
    void SellProduct();
    void ViewProducts();
    void SearchProduct();
}

public abstract class ExceptionHandler
{
    public void HandleException(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

abstract class ProductBase
{
    public string Name { get; set; }
    public abstract int Quantity { get; set; }
    public DateTime DateAdded { get; set; }
}

class Product : ProductBase
{
    public override int Quantity { get; set; }
    public decimal Price { get; set; }

    public Product(string name, int quantity, DateTime dateAdded, decimal price)
    {
        Name = name;
        Quantity = quantity;
        DateAdded = dateAdded;
        Price = price;
    }
}

class SalesInvoice : ProductBase
{
    public override int Quantity { get; set; }
    public DateTime DateSold { get; set; }

    public SalesInvoice(string productName, int quantitySold, DateTime dateSold)
    {
        Name = productName;
        Quantity = quantitySold;
        DateSold = dateSold;
    }
}

class Program : ExceptionHandler, IFileHandler, IProductOperations
{
    static List<ProductBase> inventory = new List<ProductBase>();
    static List<SalesInvoice> salesHistory = new List<SalesInvoice>();

    static void Main(string[] args)
    {
        Program program = new Program();
        program.LoadFromFile();

        while (true)
        {
            Console.WriteLine("\t\t\t\t*******************************************");
            Console.WriteLine("\t\t\t\t\tACERVUS INVENTORY SYSTEM");
            Console.WriteLine("\t\t\t\t\t1. Add Product");
            Console.WriteLine("\t\t\t\t\t2. Delete Product");
            Console.WriteLine("\t\t\t\t\t3. Update Product Quantity");
            Console.WriteLine("\t\t\t\t\t4. View Products");
            Console.WriteLine("\t\t\t\t\t5. Sell Product");
            Console.WriteLine("\t\t\t\t\t6. Sales Report");
            Console.WriteLine("\t\t\t\t\t7. Save inventory");
            Console.WriteLine("\t\t\t\t\t8. Search Product");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\t\t\t\t\t9. Exit");
            Console.ResetColor();
            Console.Write("\n\t\t\t\t\tSelect an option (1-9): ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                program.HandleException(() =>
                {
                    switch (choice)
                    {
                        case 1:
                            program.AddProduct();
                            break;
                        case 2:
                            program.DeleteProduct();
                            break;
                        case 3:
                            program.UpdateProductQuantity();
                            break;
                        case 4:
                            program.ViewProducts();
                            break;
                        case 5:
                            program.SellProduct();
                            break;
                        case 6:
                            program.SalesReport();
                            break;
                        case 7:
                            program.SaveToFile(); // Save inventory and sales history to files
                            break;
                        case 8:
                            program.SearchProduct();
                            break;
                        case 9:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                });
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option (1-9).");
            }

            Console.WriteLine("\t\t\t\t\tPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public void LoadFromFile()
    {
        if (File.Exists("report.txt"))
        {
            string[] lines = File.ReadAllLines("report.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    string name = parts[0];
                    int quantity;
                    if (int.TryParse(parts[1], out quantity))
                    {
                        DateTime dateAdded;
                        if (DateTime.TryParse(parts[2], out dateAdded))
                        {
                            decimal price;
                            if (decimal.TryParse(parts[3], out price))
                            {
                                Product product = new Product(name, quantity, dateAdded, price);
                                inventory.Add(product);
                            }
                        }
                    }
                }
            }
        }

        if (File.Exists("sales_report.txt"))
        {
            string[] lines = File.ReadAllLines("sales_report.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                {
                    string productName = parts[0];
                    int quantitySold;
                    if (int.TryParse(parts[1], out quantitySold))
                    {
                        DateTime dateSold;
                        if (DateTime.TryParse(parts[2], out dateSold))
                        {
                            SalesInvoice invoice = new SalesInvoice(productName, quantitySold, dateSold);
                            salesHistory.Add(invoice);
                        }
                    }
                }
            }
        }
    }

    public void SaveToFile()
    {
        List<string> inventoryLines = new List<string>();
        foreach (var product in inventory)
        {
            string line = $"{product.Name},{product.Quantity},{product.DateAdded},{(product is Product p ? p.Price.ToString() : "")}";
            inventoryLines.Add(line);
        }
        File.WriteAllLines("report.txt", inventoryLines);

        List<string> salesHistoryLines = new List<string>();
        foreach (var invoice in salesHistory)
        {
            string line = $"{invoice.Name},{invoice.Quantity},{invoice.DateSold}";
            salesHistoryLines.Add(line);
        }
        File.WriteAllLines("sales_report.txt", salesHistoryLines);
    }

    public void AddProduct()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\t\tAdd Product");
        Console.Write("\t\t\t\t\tEnter product name: ");
        string name = Console.ReadLine();
        Console.Write("\t\t\t\t\tEnter product quantity: ");
        if (int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.Write("\t\t\t\t\tEnter product price: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                DateTime dateAdded = DateTime.Now;
                Product product = new Product(name, quantity, dateAdded, price);
                inventory.Add(product);
                Console.WriteLine("\t\t\t\t\tProduct added successfully.");
            }
            else
            {
                Console.WriteLine("\t\t\t\t\tInvalid price. Please enter a valid number.");
            }
        }
        else
        {
            Console.WriteLine("\t\t\t\t\tInvalid quantity. Please enter a valid number.");
        }
    }

    public void DeleteProduct()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\t\tDelete Product");
        Console.Write("\t\t\t\t\tEnter the product name to delete: ");
        string name = Console.ReadLine();
        ProductBase productToDelete = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (productToDelete != null)
        {
            inventory.Remove(productToDelete);
            Console.WriteLine($"{name} deleted from the inventory.");
        }
        else
        {
            Console.WriteLine("\t\t\t\t\tProduct not found in the inventory.");
        }
    }

    public void UpdateProductQuantity()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\t\tUpdate Product Quantity");
        Console.Write("\t\t\t\t\tEnter the product name to update: ");
        string name = Console.ReadLine();
        ProductBase productToUpdate = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (productToUpdate != null)
        {
            Console.Write("\t\t\t\t\tEnter new quantity: ");
            if (int.TryParse(Console.ReadLine(), out int newQuantity))
            {
                productToUpdate.Quantity = newQuantity;
                Console.WriteLine("\t\t\t\t\tQuantity updated successfully.");
            }
            else
            {
                Console.WriteLine("\t\t\t\t\tInvalid quantity. Please enter a valid number.");
            }
        }
        else
        {
            Console.WriteLine("\t\t\t\t\tProduct not found in the inventory.");
        }
    }

    public void SellProduct()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\t\tSell Product");
        Console.Write("\t\t\t\t\tEnter the product name to sell: ");
        string name = Console.ReadLine();
        ProductBase productToSell = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (productToSell != null)
        {
            Console.Write($"\t\t\t\t\tEnter quantity of {name} to sell: ");
            if (int.TryParse(Console.ReadLine(), out int sellQuantity))
            {
                if (sellQuantity <= productToSell.Quantity)
                {
                    DateTime dateSold = DateTime.Now;
                    decimal totalSale = (productToSell is Product p ? p.Price : 0) * sellQuantity; // Calculate the total sale
                    productToSell.Quantity -= sellQuantity;
                    SalesInvoice invoice = new SalesInvoice(name, sellQuantity, dateSold);
                    salesHistory.Add(invoice);
                    Console.WriteLine($"\t\t\t\t\tSold {sellQuantity} {name} " +
                        $"\n\n\n\t\t\t\t\tfor a total of {totalSale} PESOS.");
                }
                else
                {
                    Console.WriteLine("\t\t\t\t\tInsufficient quantity in the inventory.");
                }
            }
            else
            {
                Console.WriteLine("\t\t\t\t\tInvalid quantity. Please enter a valid number.");
            }
        }
        else
        {
            Console.WriteLine("\t\t\t\t\tProduct not found in the inventory.");
        }
    }

    public void ViewProducts()
    {
        Console.Clear();
        Console.WriteLine("View Products");
        if (inventory.Count == 0)
        {
            Console.WriteLine("Inventory is empty.");
        }
        else
        {
            Console.WriteLine("Product\t\tQuantity\tDate Added\tPrice");
            Console.WriteLine("===================================================");
            foreach (var product in inventory)
            {
                Console.WriteLine($"{product.Name}\t\t{product.Quantity}\t\t{product.DateAdded}\t\t{(product is Product p ? p.Price.ToString() : "")}");
            }
        }
    }

    public void SalesReport()
    {
        Console.Clear();
        Console.WriteLine("Sales Report");
        if (salesHistory.Count == 0)
        {
            Console.WriteLine("No sales made yet.");
        }
        else
        {
            Console.WriteLine("Product\t\tQuantity Sold\tDate Sold");
            Console.WriteLine("===============================================");
            decimal totalSales = 0;

            foreach (var invoice in salesHistory)
            {
                Console.WriteLine($"{invoice.Name}\t\t{invoice.Quantity}\t\t{invoice.DateSold}");
                ProductBase soldProduct = inventory.Find(p => p.Name.Equals(invoice.Name, StringComparison.OrdinalIgnoreCase));
                if (soldProduct is Product p)
                {
                    decimal productTotalSale = p.Price * invoice.Quantity;
                    totalSales += productTotalSale;
                }
            }

            Console.WriteLine("\nTotal Sales: " + totalSales + " PESOS");
        }
    }

    public void SearchProduct()
    {
        Console.Clear();
        Console.WriteLine("\t\t\t\t\tSearch Product");
        Console.Write("\t\t\t\t\tEnter the product name to search: ");
        string name = Console.ReadLine();
        ProductBase foundProduct = inventory.Find(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        if (foundProduct != null)
        {
            Console.WriteLine("\t\t\t\t\tProduct found in the inventory:");
            Console.WriteLine($"\t\t\t\t\t{foundProduct.Name}\t\t{foundProduct.Quantity}\t\t{foundProduct.DateAdded}\t\t{(foundProduct is Product p ? p.Price.ToString() : "")}");
        }
        else
        {
            Console.WriteLine("\t\t\t\t\tProduct not found in the inventory.");
        }
    }

}
