using System;

namespace EntityLayer
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime MovementDate { get; set; }
        public string Description { get; set; } // "İlk Giriş", "Stok Ekleme" vs.
    }
}