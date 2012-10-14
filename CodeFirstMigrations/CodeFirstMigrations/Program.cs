using CodeFirstMigrations.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstMigrations
{
	class Program
	{
		static void Main(string[] args)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());

			var context = new Context();
			var orders = context.Orders
				.Include(o => o.Items.Select(oi => oi.Item))
				.ToList();
			foreach (var order in orders)
				Console.WriteLine(order.ToString());

			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}

	class Order
	{
		public int OrderID { get; set; }
		public DateTime OrderedOn { get; set; }
		public List<OrderItem> Items { get; set; }

		public Order()
		{
			this.Items = new List<OrderItem>();
		}
		
		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("OrderID: {0}\n", this.OrderID);
			sb.AppendFormat("OrderedOn: {0}\n", this.OrderedOn);

			foreach(var orderItem in Items)
				sb.AppendFormat("OrderItemID: {0}, ItemNumber: {1}, Quantity: {2}, Price: {3:c2}\n", 
					orderItem.OrderItemID, orderItem.Item.ItemNumber, orderItem.Quantity, orderItem.Price);

			sb.AppendLine();

			return sb.ToString();
		}
	}
	class OrderItem
	{
		public int OrderItemID { get; set; }
		public int OrderID { get; set; }
		public Order Order { get; set; }
		//public string Item { get; set; }
		public int ItemID { get; set; }
		public Item Item { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
	class Item
	{
		public int ItemID { get; set; }
		public string ItemNumber { get; set; }
	}

	class Context : DbContext
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Item> Items { get; set; }

		public Context()
			: base("name=Database")
		{
		}
	}
}
