using System;

namespace EntityLayer
{
    // Bu sınıf tek başına kullanılmayacak, diğerlerine miras verecek.
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}