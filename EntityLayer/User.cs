using System;

namespace EntityLayer
{
    public class User : BaseEntity
    {
        // Veritabanındaki sütun adlarıyla (Username, Password, Role) birebir aynı yapıldı.
        // Bu sayede veri çekerken veya eklerken isim karmaşası yaşamazsın.
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        // ComboBox'ta veya listelerde nesne yerine direkt ismin görünmesi için:
        public override string ToString()
        {
            return Username;
        }
    }
}